using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Services.Analytics;
using UnityEditor;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject PacStudent;
    [SerializeField] private Animator animator;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if ((int)timer > lastTime) // execute movement every second
        {
            lastTime = (int)timer; // update time

            // Move right:
            if (lastTime == 1f)
            {
                isMoving = true;
                //Debug.Log("Moving right...");
                animator.SetFloat("Horizontal", 1.0f);
                animator.SetFloat("Vertical", 0.0f);
                tweener.AddTween(PacStudent.transform, PacStudent.transform.position, new Vector3(-8.3f, 4.34f, 0.0f), 1f);

            }

            // Move down:
            if (lastTime == 2f)
            {
                //Debug.Log("Moving down...");
                animator.SetFloat("Horizontal", 0.0f);
                animator.SetFloat("Vertical", -1.0f);
                tweener.AddTween(PacStudent.transform, PacStudent.transform.position, new Vector3(-8.3f, 3.1f, 0.0f), 1f);
            }

            // Move left:
            if (lastTime == 3f)
            {
                //Debug.Log("Moving left...");
                animator.SetFloat("Horizontal", -1.0f);
                animator.SetFloat("Vertical", 0.0f);
                tweener.AddTween(PacStudent.transform, PacStudent.transform.position, new Vector3(-10.22f, 3.1f, 0.0f), 1f);
            }

            // Move up:
            if (lastTime == 4f)
            {
                //Debug.Log("Moving up...");
                animator.SetFloat("Horizontal", 0.0f);
                animator.SetFloat("Vertical", 1.0f);
                tweener.AddTween(PacStudent.transform, PacStudent.transform.position, new Vector3(-10.22f, 4.34f, 0.0f), 1f);
                ResetTime();
            }
        }
    }
    private void ResetTime()
    {
        lastTime = -1f;
        timer = 0.0f;
    }
}
