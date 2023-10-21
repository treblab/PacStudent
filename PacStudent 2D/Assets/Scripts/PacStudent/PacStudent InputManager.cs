using System.Collections;
using System.Collections.Generic;
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
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if ((int)timer > lastTime) // every second
        {
            lastTime = (int)timer; // update time

            // Move right:


            // Move down:


            // Move left:


            // Move up:

        }
    }
}
