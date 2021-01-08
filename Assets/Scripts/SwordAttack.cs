using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwordAttack : MonoBehaviour
{
    [SerializeField]
    private float attackDuration = .3f;

    [SerializeField]
    private float attackSpeed = .05f;

    SpriteRenderer sr;
    BoxCollider2D bc;
    public bool leftDir;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        Invoke("Disappear", 0);
    }

    private void Disappear()
    {
        var idk = sr.DOFade(0, attackDuration);
        idk.SetEase(Ease.InOutExpo);
        Invoke("DisableCollision", attackDuration);
    }
    private void DisableCollision()
    {
        bc.enabled = false;
    }
    private void FixedUpdate()
    {
        transform.position += new Vector3(attackSpeed * (leftDir ? -1 : 1), 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Enemy>() != null)
        {
            other.GetComponent<Enemy>().dead = true;
            other.GetComponent<Enemy>().anim.SetTrigger("Dead");
            //disable box collider
            other.GetComponent<Enemy>().bc.enabled = false;
            BasePlayer.instance.AddScore(other.GetComponent<Enemy>().points);
        }
    }

    
}
