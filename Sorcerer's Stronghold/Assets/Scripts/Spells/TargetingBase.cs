using Stronghold.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TargetingBase : MonoBehaviour
{
    //can just hover over these to see which index they correspond to
    //these are the different targeting methods
    //e's mark things that are exclusive with something
    //m's mark the various shapes for multi projectile spells (can work with single projectiles as well it just might not do anything fancy), multishot can work with others too they'll just all move together
    public enum Targets { AOE_CIRCLE, m_RADIAL, m_WAVE, e_m_SEEK_ENEMIES, e_SELF, e_MOUSE};

    //similar to the above targets but you MUST have one of these as this determines the spawn location of the spell object
    public enum SpawnLocation { m_SPAWNPLAYER, m_SPAWNMOUSE, m_SPAWNENEMY };

    /*
     * This is how the projectile moves and MUST be selected, note this is note WHERE it moves but HOW and these are all exclusive.
     * 
     * NONE: the spell doesn't move on its own (if you want it fully stationary make sure to lock the rigidybody)
     * PROJECTILE: the spell behaves like a projectile and will move towards and then past its target (assuming it can get that far), note most spells have very little drag but you could increase that for this if you want it to slow to a stop
     * POSITION: the spell will do everything in its power to get to a specific place and then stop
     */
    public enum MoveType { NONE, PROJECTILE, POSITION };

    private Spell spell;

    //each index corresponds to the effects listed above
    private bool[] activeTargets = new bool[100];

    private GameObject spellObject;
    public Vector2 targetLocation;
    public bool goToTarget = false;

    //will turn the desired effects on this object
    public void turnOnTargets(Targets[] desiredEffects)
    {
        //clear all previous targets incase this gets called during runtime (most likely from editor changes during testing)s
        for(int i = 0; i < activeTargets.Length; i++)
        {
            activeTargets[i] = false;   
        }

        //turn on the desired effects
        foreach (Targets e in desiredEffects)
        {
            activeTargets[(int)e] = true;
        }
    }

    private void Start()
    {
        spell = GetComponent<Spell>();
    }


    public void deploySpell(GameObject caster, Vector2 mouseLocation, GameObject spellBullet)
    {
        for(int i = 0; i < spell.numberOfCasts; i++)
        {
            //instantiate the spell and send it out (if needed) currently only spawns from player
            spellObject = Instantiate(spellBullet, parent: caster.transform);

            //find the position to return (self, which enemies, which mouse positions, etc)
            targetLocation = findTargetLocation(caster, mouseLocation);
            Vector2 instantiateAt = findInstatiateLocation(caster, mouseLocation);

            spellObject.transform.position = instantiateAt;

            //apply force towards the target
            if(spell.moveType == MoveType.PROJECTILE)
                spellObject.GetComponent<Rigidbody2D>().AddForce((targetLocation - instantiateAt).normalized * spell.spellSpeed);

            //move the spell towards target and stop once reaching it
            if (spell.moveType == MoveType.POSITION)
            {
                spellObject.GetComponent<TargetingBase>().targetLocation = targetLocation;
                spellObject.GetComponent<TargetingBase>().goToTarget = true;
            }

            //destroy this gameobject after a variable number of seconds
            spellObject.GetComponent<TargetingBase>().destroyShot(spellObject, spell.decayTime);

            //wait for next click before continuing
        }
    }

    //find where the target is located in the world
    private Vector2 findTargetLocation(GameObject caster, Vector2 mouseLocation)
    {
        //returns the position of the mouse in world coordinates
        if(activeTargets[(int)Targets.e_MOUSE])
        {
            Vector3 temp = Input.mousePosition;

            Vector3 temp2 = Camera.main.ScreenToWorldPoint(temp);

            return temp2;
        }

        //returns the players world position
        if (activeTargets[(int)Targets.e_SELF])
        {  
            return caster.transform.position;
        }

        //cast on self by default
        return caster.transform.position;
    }

    //find where to instantiate the spell in the world
    private Vector2 findInstatiateLocation(GameObject caster, Vector2 mouseLocation)
    {
        if (spell.spawnLocation == SpawnLocation.m_SPAWNPLAYER)
        {
            return caster.transform.position;
        }

        else if (spell.spawnLocation == SpawnLocation.m_SPAWNMOUSE)
        {
            Vector3 temp = Input.mousePosition;

            Vector3 temp2 = Camera.main.ScreenToWorldPoint(temp);

            return temp2;
        }

        //TODO: not implemented yet
        else if (spell.spawnLocation == SpawnLocation.m_SPAWNENEMY)
        {
            Debug.LogError("Could not find a suitable location to spawn spell, please check TargetingBase script");
            return new Vector2(0, 0);
        }

        Debug.LogError("Could not find a suitable location to spawn spell, please check TargetingBase script");
        return new Vector2(0,0);
    }

    private void destroyShot(GameObject obj, float time)
    {
        Invoke("destroyThis", time);
    }

    private void destroyThis()
    {
        Destroy(this.gameObject);
    }

    //define various targeting methods

    private void FixedUpdate()
    {
        //move towards a target position and stop once there
        if(goToTarget)
        { 
            this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, targetLocation, Time.deltaTime*spell.spellSpeed);
        }
    }
}
