using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        //if other overlapping is the player
        //if player if overlapping with a climbable area and has a vertical input (considering dead zones)
        if (other.GetComponent<BasePlayer>() && triggered != true )
        {
            //and it is out of the dead zone
            if (other.GetComponent<BasePlayer>().verticalInput > 0.1)
            {
                other.GetComponent<BasePlayer>().ClimbState.Invoke();
                triggered = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<BasePlayer>())
        {
            if (other.GetComponent<BasePlayer>().bClimbingMode == true)
            {
                other.GetComponent<BasePlayer>().bClimbingMode = false;
                other.GetComponent<BasePlayer>().anim.SetBool("bClimbingMode", false);
               // other.GetComponent<BasePlayer>().
                triggered = false;
                other.GetComponent<BasePlayer>().rb.gravityScale = 1;
            }
        }
    }
}
