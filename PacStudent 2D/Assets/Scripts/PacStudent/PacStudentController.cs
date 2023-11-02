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
    public Transform leftTeleporter;
    public Transform rightTeleporter;

    private GameObject ghostControllerObj;
    private GhostController ghostController;

    private bool pacStudentDead = false; // PacStudent will always be alive at the start of the game.
    public ParticleSystem pacStudentDeathParticles;

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

    // To ensure PacStudent doesnt move until the countdown is finished:
    private bool pacStudentCanMove = false;

    // For GAME OVER:
    private bool ateAllPellets = false;
    private int pelletsEaten;

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
        if (!pacStudentCanMove) return;

        if (!isLerping())
        {
            TryMoveInDirection(lastInput);

            // Uncomment this for smoother movement (not according to the marking criteria)
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
            collisionAudio.clip = collisions[0];
            collisionAudio.volume = 2.0f;
            collisionAudio.Play();
            wallCollisionParticles.Play();
        }

        if (collision.CompareTag("pellet"))
        {
            collisionAudio.clip = collisions[1];
            collisionAudio.volume = 0.1f;
            collisionAudio.Play();
            uiManager.addScore(10);
            ++pelletsEaten;

            // Convert the world position of the collision to a cell position on the Tilemap

            // Get the collision point in world space
            Vector3 hitPosition = collision.ClosestPoint(transform.position);

            // Convert the world position to a cell position on the Tilemap
            Vector3Int cellPosition = pelletTilemap.WorldToCell(hitPosition);

            // Optionally, get the world position of the center of the tile
            Vector3 tileCenterPosition = pelletTilemap.GetCellCenterWorld(cellPosition);

            // Remove the tile/destroty the pellet
            pelletTilemap.SetTile(cellPosition, null);
        }

        if (collision.CompareTag("teleporter"))
        {
            // To allow PacStudent to teleport, end any tweens.
            tweener.removeTween(PacStudent.transform);

            if (collision.transform == leftTeleporter)
            {
                // Move PacStudent to just outside the right teleporter's position - otherwise he will teleport forever
                PacStudent.transform.position = new Vector3(26.5f, -14);
                lastInput = 'A';
            }
            else if (collision.transform == rightTeleporter)
            {
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
            Destroy(collision.gameObject);
            ghostController.PowerPelletEaten();
            uiManager.startGhostTimer();
        }

        if (collision.CompareTag("normalGhost"))
        {
            Debug.Log("PacStudent has collided with a Ghost in walking state. ");

            // Declare as dead and have death effect play for a set amount of time (3s)
            pacStudentDead = true;
            ParticleSystem deathEffectInstance = Instantiate(pacStudentDeathParticles, transform.position, Quaternion.identity);
            Destroy(deathEffectInstance, 3.0f);
            uiManager.removeLives();

            // Re-instantiate PacStudent at the top-left, and wait for player input.
            RespawnPacStudent();
            pacStudentDead = false;
        }

        if (collision.CompareTag("scaredGhost") || collision.CompareTag("recoveringGhost"))
        {
            Debug.Log("PacStudent has collided with a Ghost in Scared State. ");
            
            ghostController.scaredGhostEaten(collision.gameObject);
            uiManager.addScore(300);
        }

    }

    //80% band - helper methods below:
    private void RespawnPacStudent()
    {
        tweener.removeTween(PacStudent.transform);
        PacStudent.transform.position = new Vector3(1,-1);
    }

    public void togglePacStudentMovement(bool canMove)
    {
        pacStudentCanMove = canMove;
    }

    public bool eatenAllPellets()
    {
        if (pelletsEaten == 218) // There are 218 regular pellets in the game - if he has eaten them all, he wins.
        {
            ateAllPellets = true;
            return ateAllPellets;
        }
        return ateAllPellets;
    }
}