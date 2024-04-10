using Stronghold.Base;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

//this will go on a prefab object and should probably be used through the spellbook
public class Spell : MonoBehaviour
{
    [SerializeField]  EffectBase.Effects[] effects;
    [SerializeField] TargetingBase.Targets[] targets;
    [SerializeField] public TargetingBase.SpawnLocation spawnLocation;
    [SerializeField] private GameObject spellBullet;

    //this is damage, healing, durability and things of that nature depending on what the spells effects are
    [SerializeField] public float strength = 0f;

    [SerializeField] public int numberOfCasts = 1;

    //the elemental attribute this spell will use
    [SerializeField] public EntityBase.Elements spellElement = EntityBase.Elements.Null;

    [SerializeField] float cooldownTime = 0f;

    private EffectBase eb;
    private TargetingBase tb;

    private void Awake()
    {
        eb = this.GetComponent<EffectBase>();
        tb = this.GetComponent<TargetingBase>();

        eb.turnOnEffects(effects);
        tb.turnOnTargets(targets);
    }

    //handles the actual firing of the spell not determing when to fire it
    public void fireSpell(GameObject caster, Vector2 mouseLocation)
    {
        //create a copy of this object going towards target
        tb.deploySpell(caster, mouseLocation, spellBullet);
    }
}
