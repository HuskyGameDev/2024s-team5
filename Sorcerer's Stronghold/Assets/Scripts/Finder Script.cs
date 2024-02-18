using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * This script is given a tag, collects all game objects with that tag, and returns the
 * closest game object to the object this script is attached to.
 * 
 * When we want to implement favored targets, this can find objects such as resource towers by 
 * giving them the "Resource Tower" tag.
 */
public class FinderScript : MonoBehaviour
{
    public string targetTag;
    public GameObject FindClosestOfTag(string tag)
    {
        targetTag = tag;
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        float distanceToClosestTarget = Mathf.Infinity;
        GameObject closestTarget = null; 

        foreach (GameObject target in targets)
        {
            float distanceToTarget = (target.transform.position - this.transform.position).sqrMagnitude;
            if(distanceToTarget < distanceToClosestTarget)
            {
                distanceToClosestTarget = distanceToTarget;
                closestTarget = target;
            }
        }

        return closestTarget;

    }
}
