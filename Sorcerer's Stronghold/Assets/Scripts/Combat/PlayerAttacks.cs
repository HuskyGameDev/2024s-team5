using Stronghold.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    //store a list of spell objects
    [SerializeField] SpellBase spell;

    //use a spell by calling cast on the object
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            spell.castSpell();
        }
    }

    //display cooldowns in a UI element
}
