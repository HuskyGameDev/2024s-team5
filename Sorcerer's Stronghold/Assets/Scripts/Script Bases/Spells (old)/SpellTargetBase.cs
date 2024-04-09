using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Stronghold.Base
{ 
    public class SpellTargetBase : MonoBehaviour
    {
        //SpellBase.SpellTargets target, int numberOfTargets, GameObject caster
        public static void castOnTarget(SpellBase spell, GameObject caster)
        {
            switch (spell.target)
            {
                case SpellBase.SpellTargets.Multiple:
                    createSpellObject(spell.numberOfTargets, spell.target, caster, spell);
                    break;

                //all other types have just 1 target location they need
                default: 
                    createSpellObject(1, spell.target, caster, spell);
                    break;
            }
        }

        private static Vector2 getLocation(SpellBase.SpellTargets target, GameObject caster)
        {
            if (target == SpellBase.SpellTargets.Self)
                return caster.transform.position;

            //convert mouse position into coordinates
            Vector3 temp = Input.mousePosition;

            Vector3 temp2 = Camera.main.ScreenToWorldPoint(temp);

            return (temp2 - caster.transform.position);
        }

        private static void createSpellObject(int number, SpellBase.SpellTargets target, GameObject caster, SpellBase spell)
        {
            //TODO: need some way to pause this for multi selection
            for (int i = 0; i < number; i++) 
            {
                Vector2 temp = getLocation(target, caster);

                //instantiate objects
                GameObject spellObject =  Instantiate(spell.obj, parent: caster.transform);

                spellObject.GetComponent<Rigidbody2D>().AddForce(temp.normalized*5);

                //attach trigger and effect
                SpellTriggerBase trigBase = spellObject.AddComponent<SpellTriggerBase>();
                trigBase.setValues(spell.baseDamage, spell.spellElement, spell.effect);

                //create visual and aduio effects

                //send it the the target direction
            }
        }
    }
}
