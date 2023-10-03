using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEditor;
using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    [SerializeField] private GameObject PacStudent;
    private PacStudentTweener tweener;

    float timer;
    float lastTime;
    bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<PacStudentTweener>();
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if ((int)timer > lastTime) // execute movement every second
        {
            lastTime = (int)timer; // update time

            // Move right:
            if (!isMoving)
            {
                isMoving = true;
                tweener.AddTween(PacStudent.transform, PacStudent.transform.position, new Vector3(-8.3f, 4.34f, 0.0f), 0.25f);
                isMoving = false;
            }

            // Move down:
            if (!isMoving)
            {
                isMoving = true;
                tweener.AddTween(PacStudent.transform, PacStudent.transform.position, new Vector3(-8.3f, 3.1f, 0.0f), 0.25f);
                isMoving = false;
            }

            // Move left:
            if (!isMoving)
            {
                isMoving = true;
                tweener.AddTween(PacStudent.transform, PacStudent.transform.position, new Vector3(-10.22f, 3.1f, 0.0f), 0.25f);
                isMoving = false;
            }

            // Move up:
            if (!isMoving)
            {
                isMoving = true;
                tweener.AddTween(PacStudent.transform, PacStudent.transform.position, new Vector3(-10.22f, 4.34f, 0.0f), 0.25f);
                isMoving = false;
            }
        }
    }
}
