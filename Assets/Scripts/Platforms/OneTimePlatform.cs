using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OneTimePlatform : MonoBehaviour
{
    public float InitDelay = 1;
    public float timeToDisappear = 2;
    public float CollisionOffTime = 1;

    SpriteRenderer sr;
    BoxCollider2D bc;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
    }
    
    public void Trigger()
    {
        Invoke("Disappear", InitDelay);
    }

    private void Disappear()
    {
        var idk = sr.DOFade(0, timeToDisappear);
        idk.SetEase(Ease.Linear);
        Invoke("DisableCollision", CollisionOffTime);
    }
    private void DisableCollision()
    {
        bc.enabled = false;
    }
}
