using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stronghold.Base
{
    public class SpellTriggerBase : MonoBehaviour
    {
        private float damage = 0;
        private EntityBase.Elements element = EntityBase.Elements.Null;
        SpellBase.SpellEffects effect;

        public void setValues(float damageAmount, EntityBase.Elements elementType, SpellBase.SpellEffects spellEffect)
        {
            damage = damageAmount;
            element = elementType;
            effect = spellEffect;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //if the hit object is an enemy, damage it
            try
            {
                if (effect == SpellBase.SpellEffects.InstantDamage)
                {
                    EntityBase entity = collision.gameObject.GetComponent<EntityBase>();
                    entity.alterHealth(-damage, element);
                }
                else if (effect == SpellBase.SpellEffects.Heal)
                {
                    EntityBase entity = collision.gameObject.GetComponent<EntityBase>();
                    entity.alterHealth(damage, element);
                }
            }
            catch (NullReferenceException e)
            {
                Debug.LogWarning("The object you hit doesn't have the entityBase component");
            }

            //then destroy the spell object
            Destroy(this.gameObject);
        }
    }
}
