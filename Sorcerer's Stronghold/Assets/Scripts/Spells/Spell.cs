using Stronghold.Base;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

//this will go on a prefab object and should probably be used through the spellbook
public class Spell : MonoBehaviour
{
    [SerializeField] EffectBase.Effects[] effects;
    [SerializeField] TargetingBase.Targets[] targets;
    [SerializeField] public TargetingBase.SpawnLocation spawnLocation;
    [SerializeField] public TargetingBase.MoveType moveType;

    //this is damage, healing, durability and things of that nature depending on what the spells effects are
    [SerializeField] public float strength = 0f;

    //this affects the strength of secondary effects (they'll be tagged with an s in EffectBase)
    [SerializeField] public float secondaryStrength = 0f;

    //how many "projectiles" this spell 'fires" (not necessarily projectiles or firing I just don't know how to word it better)
    [SerializeField] public int numberOfCasts = 1;

    //the elemental attribute this spell will use
    [SerializeField] public EntityBase.Elements spellElement = EntityBase.Elements.Null;

    //how long until you can cast this spell again
    [SerializeField] float cooldownTime = 0f;

    //how quickly this spell moves
    [SerializeField] public float spellSpeed = 5;

    //how long until this spell is destroyed (incase it isn't destroyed by something else)
    [SerializeField] public float decayTime = 1;

    private EffectBase eb;
    private TargetingBase tb;
    private float currentTimer;

    private void Start()
    {
        eb = GetComponent<EffectBase>();
        tb = GetComponent<TargetingBase>();

        updateValues();
    }

    //clears all values and then sets the appropriate values on targeting and effect bases
    public void updateValues()
    {
        eb.turnOnEffects(effects);
        tb.turnOnTargets(targets);
    }

    //handles the actual firing of the spell not determing when to fire it
    public void fireSpell(GameObject caster, Vector2 mouseLocation)
    {
        if (currentTimer <= 0)
        {
            //this is for testing during runtime in the editor, should be safe to comment it out for release
            updateValues();

            //create a copy of this object going towards target
            tb.deploySpell(caster, mouseLocation, this.gameObject);

            currentTimer = cooldownTime;
        }
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
