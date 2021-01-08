using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int points;
    public AudioClip CoinEffect;
    AudioSource audioSource;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BasePlayer>() != null)
        {
           // AudioSource audioSource = GetComponent<AudioSource>();

            AudioSource.PlayClipAtPoint(CoinEffect,transform.position);

            BasePlayer.instance.AddScore(points);
            Destroy(gameObject);
        }
    }
}
