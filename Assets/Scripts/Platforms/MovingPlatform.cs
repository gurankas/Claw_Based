using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 destination;
    public float duration;

    // Start is called before the first frame update
    void Start()
    {
        Tweener moveTween = transform.DOMove(destination, duration);
        //We make this tween relative, so it is relative to it's start position
        moveTween.SetRelative(true);
        //We make it go back and forth infinitely
        moveTween.SetLoops(-1, LoopType.Yoyo);

        moveTween.SetEase(Ease.InOutExpo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
