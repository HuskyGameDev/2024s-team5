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
    //m's mark the various shapes for multi projectile spells (can work with single projectiles as well it just might not do anything fancy)
    public enum Targets { e_NONE, PIERCING, AOE_CIRCLE, m_RADIAL, m_WAVE, SEEK_ENEMIES, e_SELF, e_m_ENEMY, e_m_MOUSE};

    //similar to the above targets but you MUST have one of these as this determines the spawn location of the spell object
    public enum SpawnLocation { e_SPAWNPLAYER, e_m_SPAWNMOUSE, e_m_SPAWNENEMY };

    private Spell spell;

    //each index corresponds to the effects listed above
    private bool[] activeTargets = new bool[100];

    //will turn the desired effects on this object
    public void turnOnTargets(Targets[] desiredEffects)
    {
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
            GameObject spellObject = Instantiate(spellBullet, parent: caster.transform);

            //find the position to return (self, which enemies, which mouse positions, etc)
            Vector2 targetLocation = findTargetLocation(caster, mouseLocation);

            spellObject.transform.position = findInstatiateLocation(caster, mouseLocation);

            //if not targeting self or none launch towards target
            if (!activeTargets[(int) Targets.e_SELF] && !activeTargets[(int)Targets.e_NONE])
                spellObject.GetComponent<Rigidbody2D>().AddForce(targetLocation.normalized * spell.spellSpeed);

            //destroy this gameobject after 1 seconds
            spellObject.GetComponent<TargetingBase>().destroyShot(spellObject, 5);

            //wait for next click before continuing
        }
    }

    private Vector2 findTargetLocation(GameObject caster, Vector2 mouseLocation)
    {
        if(activeTargets[(int)Targets.e_m_MOUSE])
        {
            Vector3 temp = Input.mousePosition;

            Vector3 temp2 = Camera.main.ScreenToWorldPoint(temp);

            return (temp2 - caster.transform.position);
        }

        if (activeTargets[(int)Targets.e_SELF])
        {  
            return caster.transform.position;
        }

        //cast on self by default
        return caster.transform.position;
    }

    private Vector2 findInstatiateLocation(GameObject caster, Vector2 mouseLocation)
    {
        if (spell.spawnLocation == SpawnLocation.e_SPAWNPLAYER)
        {
            return caster.transform.position;
        }

        else if (spell.spawnLocation == SpawnLocation.e_m_SPAWNMOUSE)
        {
            Vector3 temp = Input.mousePosition;

            Vector3 temp2 = Camera.main.ScreenToWorldPoint(temp);

            return temp2;
        }

        //TODO: not implemented yet
        else if (spell.spawnLocation == SpawnLocation.e_m_SPAWNENEMY)
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
}
