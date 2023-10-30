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

    float timer;
    float lastTime;
    bool isMoving;

    private char lastInput;
    private char currentInput;

    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
        isMoving = true;

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
    private void ResetTime()
    {
        lastTime = -1f;
        timer = 0.0f;
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
                //Debug.Log("Moving up...");
                Vector3 movementVector = PacStudent.transform.position + Vector3.up;
                animator.SetFloat("Horizontal", 0.0f);
                animator.SetFloat("Vertical", 1.0f);
                tweener.AddTween(PacStudent.transform, PacStudent.transform.position, movementVector, 0.25f);
                ResetTime();
            }

            // A - Move left:
            if (Input.GetKeyDown(KeyCode.A))
            {
                //Debug.Log("Moving left...");
                Vector3 movementVector = PacStudent.transform.position + Vector3.left;
                animator.SetFloat("Horizontal", -1.0f);
                animator.SetFloat("Vertical", 0.0f);
                tweener.AddTween(PacStudent.transform, PacStudent.transform.position, movementVector, 0.25f);
            }

            // S - Move down:
            if (Input.GetKeyDown(KeyCode.S))
            {
                //Debug.Log("Moving down...");
                Vector3 movementVector = PacStudent.transform.position + Vector3.down;
                animator.SetFloat("Horizontal", 0.0f);
                animator.SetFloat("Vertical", -1.0f);
                tweener.AddTween(PacStudent.transform, PacStudent.transform.position, movementVector, 0.25f);
            }

            // D - Move right:
            if (Input.GetKeyDown(KeyCode.D))
            {
                //Debug.Log("Moving right...");
                Vector3 movementVector = PacStudent.transform.position + Vector3.right;
                animator.SetFloat("Horizontal", 1.0f);
                animator.SetFloat("Vertical", 0.0f);
                tweener.AddTween(PacStudent.transform, PacStudent.transform.position, movementVector, 0.25f);
            }
        }
    }
}
