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

    //this is damage, healing, durability and things of that nature depending on what the spells effects are
    [SerializeField] public float strength = 0f;

    [SerializeField] public int numberOfCasts = 1;

    //the elemental attribute this spell will use
    [SerializeField] public EntityBase.Elements spellElement = EntityBase.Elements.Null;

    [SerializeField] float cooldownTime = 0f;
    [SerializeField] public float spellSpeed = 5;

    private EffectBase eb;
    private TargetingBase tb;

    private float currentTimer;

    private void Start()
    {
        eb = gameObject.AddComponent<EffectBase>();
        tb = gameObject.AddComponent<TargetingBase>();

        setValues();
    }

    public void setValues()
    {
        eb.turnOnEffects(effects);
        tb.turnOnTargets(targets);
    }

    //handles the actual firing of the spell not determing when to fire it
    public void fireSpell(GameObject caster, Vector2 mouseLocation)
    {
        if (currentTimer <= 0)
        {
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
