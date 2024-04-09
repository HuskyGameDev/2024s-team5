using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this will go on a prefab object and should probably be used through the spellbook
public class Spell : MonoBehaviour
{
    [SerializeField] EffectBase.Effects[] effects;
    [SerializeField] TargetingBase.Targets[] targets;

    [SerializeField] float damage;
    [SerializeField] int type;

    private void Awake()
    {
        this.GetComponent<EffectBase>().turnOnEffects(effects);
        this.GetComponent<TargetingBase>().turnOnTargets(targets);
    }

    //handles the actual firing of the spell not determing when to fire it
    public void fireSpell(Vector2 target)
    {
        //create the object going towards target (ise targeting base for this)

        //fire the object (slightly based on targets)
        Debug.Log("Fired spell");
    }
}
