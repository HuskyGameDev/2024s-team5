using Stronghold.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.FilterWindow;

public class EffectBase : MonoBehaviour
{
    //can just hover over these to see which index they correspond to
    //these are the different effects a spell can have, e's mark things that are exclusive with something
    public enum Effects {eDAMAGE, eHEAL, CREATEWALL, CREATETOWER, ePUSH, ePULL};

    //each index corresponds to the effects listed above
    private bool[] activeEffects = new bool[100];

    private Spell spell;

    //will turn the desired effects on this object
    public void turnOnEffects(Effects[] desiredEffects)
    {
        foreach(Effects e in desiredEffects)
        {
            activeEffects[(int) e] = true;
        }
    }

    private void Awake()
    {
        spell = GetComponent<Spell>();
    }

    //define various effect methods

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the hit object is an enemy, damage it
        try
        {
            if (activeEffects[(int)Effects.eDAMAGE] && collision.gameObject.tag == "Enemy")
            {
                EntityBase entity = collision.gameObject.GetComponent<EntityBase>();
                entity.alterHealth(-spell.strength, spell.spellElement);
            }
            else if (activeEffects[(int)Effects.eHEAL] && collision.gameObject.tag == "PlayerBuilt")
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
        Debug.Log("destroyed");
        Destroy(this.gameObject);
    }
}
