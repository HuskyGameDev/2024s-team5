using Stronghold.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    //store a list of spell objects
    [SerializeField] SpellBase spell;

    private float currentTimer;

    private void Start()
    {
        currentTimer = spell.spellCooldown;
    }

    //use a spell by calling cast on the object
    private void Update()
    {
        //fire the spell
        if (Input.GetMouseButtonDown(0) && currentTimer <= 0)
        {
            spell.castSpell(this.gameObject);
            currentTimer = spell.spellCooldown;
        }

        //display cooldowns in a UI element
    }

    //keep track of spell cooldowns here
    private void FixedUpdate()
    {
        if (currentTimer > 0)
        {
            currentTimer -= Time.fixedDeltaTime;
        }
    }
}
