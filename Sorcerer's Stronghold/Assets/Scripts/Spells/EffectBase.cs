using Stronghold.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using static UnityEditor.Rendering.FilterWindow;

public class EffectBase : MonoBehaviour
{
    //can just hover over these to see which index they correspond to
    //these are the different effects a spell can have, e's mark things that are exclusive with something
    public enum Effects {eDAMAGE, eHEAL, CREATEWALL, CREATETOWER, e_s_PUSH, e_s_PULL, s_PIERCING};

    //each index corresponds to the effects listed above
    private bool[] activeEffects = new bool[100];

    private Spell spell;

    //keeps track of how many collisions this spell has had for things like piercing
    private int hitCounter = 0;

    //will turn the desired effects on this object
    public void turnOnEffects(Effects[] desiredEffects)
    {
        //clear all previous targets incase this gets called during runtime (most likely from editor changes during testing)s
        for (int i = 0; i < activeEffects.Length; i++)
        {
            activeEffects[i] = false;
        }

        //turn on the desired effects
        foreach (Effects e in desiredEffects)
        {
            activeEffects[(int) e] = true;
        }
    }

    private void Start()
    {
        spell = GetComponent<Spell>();
    }

    //define various effect methods

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //make sure we don't collide with player or other spells
        if (collision.gameObject.layer != LayerMask.NameToLayer("Spell") && collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            hitCounter++;

            //if the hit object is an enemy, damage it
            try
            {
                //if colliding with enemy...
                if(collision.gameObject.tag == "Enemy")
                {
                    //damage enemy
                    if (activeEffects[(int)Effects.eDAMAGE])
                    {
                        EntityBase entity = collision.gameObject.GetComponent<EntityBase>();
                        entity.alterHealth(-spell.strength, spell.spellElement);
                    }

                    //push or pull enemy
                    if (activeEffects[(int)Effects.e_s_PUSH])
                    {
                        Rigidbody2D entity = collision.gameObject.GetComponent<Rigidbody2D>();
                        entity.AddForce(this.GetComponent<Rigidbody2D>().velocity.normalized * spell.secondaryStrength);
                    }
                    else if (activeEffects[(int)Effects.e_s_PULL])
                    {
                        Rigidbody2D entity = collision.gameObject.GetComponent<Rigidbody2D>();
                        entity.AddForce(-this.GetComponent<Rigidbody2D>().velocity.normalized * spell.secondaryStrength);
                    }
                }

                //if colliding with player built structure heal it
                if (activeEffects[(int)Effects.eHEAL] && collision.gameObject.tag == "PlayerBuilt")
                {
                    EntityBase entity = collision.gameObject.GetComponent<EntityBase>();
                    entity.alterHealth(spell.strength, spell.spellElement);
                }

            }
            catch (NullReferenceException e)
            {
                Debug.LogWarning("The object you hit doesn't have the entityBase component");
            }

            //then destroy the spell object
            if (!activeEffects[(int) Effects.s_PIERCING] || hitCounter >= spell.secondaryStrength)
                Destroy(this.gameObject);
        }

    }
}
