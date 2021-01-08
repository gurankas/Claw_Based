using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DisappearingPlatform : MonoBehaviour
{
    public float StableDuration = 3;
    public float timeToDisappear = 2;
    public float collisionToggleTime = .5f;

    SpriteRenderer sr;
    BoxCollider2D bc;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        Invoke("Disappear", StableDuration);
    }

    private void Reappear()
    {
        var idk = sr.DOFade(1, timeToDisappear);
        idk.SetEase(Ease.Linear);

        Invoke("EnableCollision", timeToDisappear - collisionToggleTime);
        Invoke("Disappear", StableDuration + timeToDisappear);
    }

    private void Disappear()
    {
        var idk = sr.DOFade(0, timeToDisappear);
        idk.SetEase(Ease.Linear);
        Invoke("DisableCollision", collisionToggleTime);
        Invoke("Reappear", StableDuration);
    }

    private void EnableCollision()
    {
        bc.enabled = true;
    }

    private void DisableCollision()
    {
        bc.enabled = false;
    }
}
