using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tweener : MonoBehaviour
{
    //private Tween activeTween;
    private List<Tween> activeTweens;

    void Start()
    {
        activeTweens = new List<Tween>();
    }

    void Update()
    {
        //if (activeTween != null)
        Tween activeTween;
        for (int i = activeTweens.Count - 1; i >= 0; i--) //Tween activeTween in activeTweens.Reverse<Tween>())
        {
            activeTween = activeTweens[i];

            if (Vector3.Distance(activeTween.Target.position, activeTween.EndPos) > 0.1f)
            {
                float timeFraction = (Time.time - activeTween.StartTime) / activeTween.Duration;
                //timeFraction = Mathf.Pow(timeFraction, 3);
                activeTween.Target.position = Vector3.Lerp(activeTween.StartPos,
                                                          activeTween.EndPos,
                                                           timeFraction);
            }
            else
            {
                activeTween.Target.position = activeTween.EndPos;
                //activeTween = null;
                activeTweens.RemoveAt(i);
            }
        }
    }

    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        if (!TweenExists(targetObject))
        {
            activeTweens.Add(new Tween(targetObject, startPos, endPos, Time.time, duration));
            return true;
        }
        return false;
    }


    public bool TweenExists(Transform target)
    {
        foreach (Tween activeTween in activeTweens)
        {
            if (activeTween.Target.transform == target)
                return true;
        }
        return false;
    }

    public bool IsTweening(Transform target = null)
    {
        if (target == null)
        {
            // If no target is provided, check if there are any active tweens at all
            return activeTweens.Count > 0;
        }
        else
        {
            // If a target is provided, check if that specific target is tweening
            return TweenExists(target);
        }
    }

    public void removeTween(Transform target)
    {
        for (int i = activeTweens.Count - 1; i >= 0; i--)
        {
            if (activeTweens[i].Target == target)
            {
                activeTweens.RemoveAt(i);
                break; // Assuming one tween per target. If multiple tweens can exist for the same target, remove this line.
            }
        }
    }
}
