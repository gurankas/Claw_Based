using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotBlocker : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //ignore collision with player
        Physics2D.IgnoreLayerCollision(2,8);
    }
}
