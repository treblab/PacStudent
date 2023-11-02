using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Services.Analytics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{
    [SerializeField] private GameObject PacStudent;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource movementAudio;

    public AudioSource collisionAudio;
    public AudioClip[] collisions;

    private Tweener tweener;
    private LevelGrid levelGrid;
    private UIManager uiManager;

    private char lastInput;
    private char currentInput;

    public ParticleSystem dustParticles;
    public ParticleSystem wallCollisionParticles;

    public Tilemap pelletTilemap;
    [SerializeField] private Transform leftTeleporter;
    [SerializeField] private Transform rightTeleporter;

    private GameObject ghostControllerObj;
    private GhostController ghostController;


    // Map of keys and their corresponding directions
    private Dictionary<char, Vector2> charToDirection = new Dictionary<char, Vector2>
    {
        { 'W', Vector2.up },
        { 'A', Vector2.left },
        { 'S', Vector2.down },
        { 'D', Vector2.right }
    };

    // To record KeyCodes as chars
    private Dictionary<KeyCode, char> keyToChar = new Dictionary<KeyCode, char>
    {
        { KeyCode.W, 'W' },
        { KeyCode.A, 'A' },
        { KeyCode.S, 'S' },
        { KeyCode.D, 'D' }
    };

    // To rotate dust particles based on PacStudent movement
    private Dictionary<Vector2, float> directionToRotation = new Dictionary<Vector2, float>
    {
        { Vector2.up, 0f },
        { Vector2.down, 180f },
        { Vector2.left, 90f },
        { Vector2.right, -90f }
    };

    void Start()
    {
        tweener = GetComponent<Tweener>();
        levelGrid = new LevelGrid();
        uiManager = GameObject.Find("Managers").GetComponent<UIManager>();
        ghostControllerObj = GameObject.Find("GhostController");
        ghostController = ghostControllerObj.GetComponent<GhostController>();
    }

    void Update()
    {
        if (!isLerping())
        {
            TryMoveInDirection(lastInput);

            // if (!TryMoveInDirection(lastInput))
            // {
                // TryMoveInDirection(currentInput);
            // }
        }
        getPlayerInput();
        playMovementAnimAndAudio();
    }

    private bool TryMoveInDirection(char direction)
    {
        if (charToDirection.ContainsKey(direction))
        {
            Vector3 movementVector = levelGrid.TryMove(charToDirection[direction], PacStudent.transform.position);
            if (movementVector != PacStudent.transform.position) // This checks if the move was valid
            {
                MovePacStudent(movementVector, charToDirection[direction]);
                currentInput = direction;
                return true;
            }
        }
        return false;
    }

    private void getPlayerInput()
    {
        if (PacStudent == null) return;

        foreach (var key in keyToChar.Keys)
        {
            if (Input.GetKeyDown(key))
            {
                lastInput = keyToChar[key];
            }
        }
    }

    private void MovePacStudent(Vector3 targetPosition, Vector2 direction)
    {
        Vector3 gridToWorldPosVec = gridToWorldPos(targetPosition);
        SetAnimatorDirection(direction);
        tweener.AddTween(PacStudent.transform, PacStudent.transform.position, gridToWorldPosVec, 0.2f);

        // Rotate the dustParticles based on the direction
        if (directionToRotation.ContainsKey(direction))
        {
            dustParticles.transform.rotation = Quaternion.Euler(0, 0, directionToRotation[direction]);
        }
    }

    private void SetAnimatorDirection(Vector2 direction)
    {
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
    }

    private Vector3 gridToWorldPos(Vector3 gridPos)
    {
        return new Vector3(gridPos.x, -gridPos.y);
    }

    private bool isLerping()
    {
        bool isTweening = tweener.IsTweening(PacStudent.transform);
        animator.SetBool("isLerping", isTweening);

        return isTweening;
    }

    private void playMovementAnimAndAudio()
    {
        if (!isLerping())
        {
            if (movementAudio.isPlaying)
            {
                movementAudio.Stop();
                dustParticles.Stop();
            }
            animator.SetBool("isLerping", false);
        }
        else
        {
            if (!movementAudio.isPlaying)
            {
                movementAudio.Play();
                dustParticles.Play();
            }
            animator.SetBool("isLerping", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("wall"))
        {
            Debug.Log("PacStudent has collided with a wall. ");
            collisionAudio.clip = collisions[0];
            collisionAudio.volume = 2.0f;
            collisionAudio.Play();
            wallCollisionParticles.Play();
        }

        if (collision.CompareTag("pellet"))
        {
            Debug.Log("PacStudent has eaten a pellet. Position: ");
            collisionAudio.clip = collisions[1];
            collisionAudio.volume = 0.1f;
            collisionAudio.Play();
            
            uiManager.addScore(10);

            // Convert the world position of the collision to a cell position on the Tilemap

            // Get the collision point in world space
            Vector3 hitPosition = collision.ClosestPoint(transform.position);

            // Convert the world position to a cell position on the Tilemap
            Vector3Int cellPosition = pelletTilemap.WorldToCell(hitPosition);

            // Optionally, get the world position of the center of the tile
            Vector3 tileCenterPosition = pelletTilemap.GetCellCenterWorld(cellPosition);


            Debug.Log("Pellet cell position: " + cellPosition);
            Debug.Log("Pellet world position: " + tileCenterPosition);

            // Set the tile at that cell position to null (i.e., remove the tile)
            pelletTilemap.SetTile(cellPosition, null);
        }

        if (collision.CompareTag("teleporter"))
        {
            // To allow PacStudent to teleport, end any tweens.
            tweener.removeTween(PacStudent.transform);

            if (collision.transform == leftTeleporter)
            {
                Debug.Log("PacStudent has collided with the left teleporter. ");
                // Move PacStudent to just outside the right teleporter's position - otherwise he will teleport forever
                PacStudent.transform.position = new Vector3(26.5f, -14);
                lastInput = 'A';
            }
            else if (collision.transform == rightTeleporter)
            {
                Debug.Log("PacStudent has collided with the right teleporter. ");
                // Move PacStudent to just outside the left teleporter's position
                PacStudent.transform.position = new Vector3(1.5f, -14);
                lastInput = 'D';
            }
        }

        if (collision.CompareTag("cherry"))
        {
            Debug.Log("PacStudent has eaten a cherry. ");
            collisionAudio.clip = collisions[1];
            collisionAudio.volume = 0.1f;
            collisionAudio.Play(); // Adding sound to cherry collision to make it more fun :)

            uiManager.addScore(100);
            Destroy(collision.gameObject); 
        }

        if (collision.CompareTag("powerPellet"))
        {
            Debug.Log("PacStudent has collided with a power pellet. ");
            Destroy(collision.gameObject);
            ghostController.PowerPelletEaten();
            uiManager.startGhostTimer();
        }
 
    }
}