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
        // W - Move up:
        if (Input.GetKeyDown("W"))
        {
            //Debug.Log("Moving up...");
            Vector3 directionVec = new Vector3(-1, 1, 0);
            Vector3 movementVector = PacStudent.transform.position + Vector3.one;

            animator.SetFloat("Horizontal", 0.0f);
            animator.SetFloat("Vertical", 1.0f);
            tweener.AddTween(PacStudent.transform, PacStudent.transform.position, (PacStudent.transform.position + Vector3.one), 1.0f);
            ResetTime();
        }

        // A - Move left:
        if (Input.GetKeyDown("A"))
        {
            //Debug.Log("Moving left...");
            animator.SetFloat("Horizontal", -1.0f);
            animator.SetFloat("Vertical", 0.0f);
            tweener.AddTween(PacStudent.transform, PacStudent.transform.position, new Vector3(-10.22f, 3.1f, 0.0f), 1f);
        }

        // S - Move down:
        if (Input.GetKeyDown("S"))
        {
            //Debug.Log("Moving down...");
            animator.SetFloat("Horizontal", 0.0f);
            animator.SetFloat("Vertical", -1.0f);
            tweener.AddTween(PacStudent.transform, PacStudent.transform.position, new Vector3(-8.3f, 3.1f, 0.0f), 1f);
        }

        // D - Move right:
        if (Input.GetKeyDown("D"))
        {
            isMoving = true;
            //Debug.Log("Moving right...");
            animator.SetFloat("Horizontal", 1.0f);
            animator.SetFloat("Vertical", 0.0f);
            tweener.AddTween(PacStudent.transform, PacStudent.transform.position, new Vector3(-8.3f, 4.34f, 0.0f), 1f);
            movementAudio.mute = false;
            movementAudio.loop = true;
        }

    }
    private void ResetTime()
    {
        lastTime = -1f;
        timer = 0.0f;
    }
}
