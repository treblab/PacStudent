using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Services.Analytics;
using UnityEditor;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    [SerializeField] private GameObject PacStudent;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource movementAudio;
    public AudioClip collisionSound;

    private Tweener tweener;
    private LevelGrid levelGrid;

    private char lastInput;
    private char currentInput;

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

    void Start()
    {
        tweener = GetComponent<Tweener>();
        levelGrid = new LevelGrid();
        PacStudent = GameObject.FindWithTag("PacStudent");
        animator = PacStudent.GetComponent<Animator>();
        movementAudio = PacStudent.GetComponent<AudioSource>();
        movementAudio.Play();
    }

    void Update()
    {
        if (!isLerping())
        {
            if (!TryMoveInDirection(lastInput))
            {
                TryMoveInDirection(currentInput);
            }
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
            }
            animator.SetBool("isLerping", false);
        }
        else
        {
            if (!movementAudio.isPlaying)
            {
                movementAudio.Play();
            }
            animator.SetBool("isLerping", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("wall"))
        {

        }
    }
}