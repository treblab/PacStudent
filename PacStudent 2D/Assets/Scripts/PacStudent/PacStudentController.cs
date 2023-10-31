using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Services.Analytics;
using UnityEditor;
using UnityEngine;

public class PacStudentController: MonoBehaviour
{
    [SerializeField] private GameObject PacStudent;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource movementAudio;

    private Tweener tweener;

    bool isMoving;

    private char lastInput;
    // private char currentInput;
    private LevelGrid levelGrid;
    // bool isLerping;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = true;

        tweener = GetComponent<Tweener>();
        levelGrid = new LevelGrid();

        PacStudent = GameObject.FindWithTag("PacStudent");

        animator = PacStudent.GetComponent<Animator>();
        animator.SetFloat("Horizontal", 0.0f);
        animator.SetFloat("Vertical", 0.0f);

        movementAudio = PacStudent.GetComponent<AudioSource>();
        movementAudio.mute = true; // audio muted at the start as PacStudent is idle.
    }

    // Update is called once per frame
    void Update()
    {
        getPlayerInput();
    }

    private void getPlayerInput()
    {
        if (PacStudent != null)
        {
            movementAudio.mute = false;
            movementAudio.loop = true;
            isMoving = true;

            // W - Move up:
            if (Input.GetKeyDown(KeyCode.W))
            {
                // Inverted with Vector2.Down as the grid is inverted when compared to Unity's world coordinates.
                Vector3 movementVector = levelGrid.TryMove(Vector2.down, PacStudent.transform.position);
                Vector3 gridToWorldPosVec = gridToWorldPos(movementVector);

                Debug.Log("Movement vector: " + movementVector);
                Debug.Log("GridToWorld Vec: " + gridToWorldPosVec);

                if (movementVector != PacStudent.transform.position) // This checks if the move was valid
                {
                    animator.SetFloat("Horizontal", 0.0f);
                    animator.SetFloat("Vertical", 1.0f);

                    // Inversion for same reason as before.
                    tweener.AddTween(PacStudent.transform, PacStudent.transform.position, gridToWorldPosVec, 0.2f);
                    lastInput = 'W';
                }
            }

            // A - Move left:
            if (Input.GetKeyDown(KeyCode.A))
            {
                Vector3 movementVector = levelGrid.TryMove(Vector2.left, PacStudent.transform.position);
                Vector3 gridToWorldPosVec = gridToWorldPos(movementVector);

                Debug.Log("Movement vector: " + movementVector);
                Debug.Log("GridToWorld Vec: " + gridToWorldPosVec);

                if (movementVector != PacStudent.transform.position) // This checks if the move was valid
                {
                    animator.SetFloat("Horizontal", -1.0f);
                    animator.SetFloat("Vertical", 0.0f);

                    tweener.AddTween(PacStudent.transform, PacStudent.transform.position, gridToWorldPosVec, 0.2f);
                    lastInput = 'A';
                }
            }

            // S - Move down:
            if (Input.GetKeyDown(KeyCode.S))
            {
                Vector3 movementVector = levelGrid.TryMove(Vector2.up, PacStudent.transform.position);
                Vector3 gridToWorldPosVec = gridToWorldPos(movementVector);

                Debug.Log("Movement vector: " + movementVector);
                Debug.Log("GridToWorld Vec: " + gridToWorldPosVec);

                if (movementVector != PacStudent.transform.position) // This checks if the move was valid
                {
                    animator.SetFloat("Horizontal", 0.0f);
                    animator.SetFloat("Vertical", -1.0f);

                    tweener.AddTween(PacStudent.transform, PacStudent.transform.position, gridToWorldPosVec, 0.2f);
                    lastInput = 'S';
                }
            }

            // D - Move right:
            if (Input.GetKeyDown(KeyCode.D))
            {
                Vector3 movementVector = levelGrid.TryMove(Vector2.right, PacStudent.transform.position);
                Vector3 gridToWorldPosVec = gridToWorldPos(movementVector);

                Debug.Log("Movement vector: " + movementVector);
                Debug.Log("GridToWorld Vec: " + gridToWorldPosVec);

                if (movementVector != PacStudent.transform.position) // This checks if the move was valid
                {
                    animator.SetFloat("Horizontal", 1.0f);
                    animator.SetFloat("Vertical", 0.0f);

                    tweener.AddTween(PacStudent.transform, PacStudent.transform.position, gridToWorldPosVec, 0.2f);
                    lastInput = 'D';
                }
            }
        }
    }

    // Created as the world pos is negative and grid pos is positive (inverted)
    private Vector3 gridToWorldPos(Vector3 gridPos)
    {
        Vector3 worldPos = new Vector3(gridPos.x, -gridPos.y);
        return worldPos;
    }
}
