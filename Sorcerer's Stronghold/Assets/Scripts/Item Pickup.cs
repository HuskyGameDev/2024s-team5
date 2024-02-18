using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/*
 * This script is designed to send out a unity event signal
 * when the picked up item has a collision with the player. This
 * script is designed to be copied and used for seperate items, 
 * Example:
 * Create a copy of this called woodPickup that invokes an
 * event called pickupWood that can then be used by other scripts to
 * update ui count, inventory counts, ect with just 1 event
 * 
 * The video I am referencing for this use case: https://www.youtube.com/watch?v=ax1DiSutEy8
 */
public class ItemPickup : MonoBehaviour
{
    public UnityEvent pickupItem; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            pickupItem.Invoke();
        }
    }
}
