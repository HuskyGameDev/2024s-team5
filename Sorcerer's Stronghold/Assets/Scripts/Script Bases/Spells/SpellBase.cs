using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stronghold.Base
{
    public class SpellBase : MonoBehaviour
    {
        //this determines the type of effect the spell will have once triggered
        public enum SpellEffects
        {
            Null,
            LingeringDamage,
            InstantDamage,
            Heal
        }

        //this is how the spell will target enemies/the player
        public enum SpellTargets
        {
            Null,
            Self,
            AOE,
            Single,
            Multiple,
            Chain
        }

        //this is what causes a spell to "activate" and trigger its effect
        public enum SpellTriggers
        {
            Null,
            Collision,
            Timer,
            Proximity
        }

        //the elemental attribute this spell will use when attacking enemies
        [SerializeField] protected EntityBase.Elements spellElement = EntityBase.Elements.Null;

        [SerializeField] protected float baseDamage = 0f;

        //the public structure to build spells with
        public SpellEffects effect = SpellEffects.Null;
        public SpellTargets target = SpellTargets.Null;
        public SpellTriggers trigger = SpellTriggers.Null;

        //how long before the spell can be cast again
        public float spellCooldown = 0f;

        //casts the spell using the above specifications
        public void castSpell()
        {
            //spell target will determine how the player "fires" the spell

            //spell trigger will determine how the spell object activates its effect (probably instatiate an object with the trigger/effect base)

            //spell effect will actually do the damage or whatever
        }
    }
}
