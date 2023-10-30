using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentTweener : MonoBehaviour
{
    private Tween pacStudentTween;

    // Start is called before the first frame update
    void Start()
    {
        pacStudentTween = GetComponent<Tween>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(pacStudentTween.Target.position, pacStudentTween.EndPos) > 0.1f)
        {
            float timeFraction = (Time.time - pacStudentTween.StartTime) / pacStudentTween.Duration;
            pacStudentTween.Target.position = Vector3.Lerp(pacStudentTween.StartPos, pacStudentTween.EndPos, timeFraction);
        } 
        else
        {
            pacStudentTween.Target.position = pacStudentTween.EndPos;
            pacStudentTween = null;
        }
        
    }

    public void AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        pacStudentTween = new Tween (targetObject, startPos, endPos, Time.time, duration);
    }
}
