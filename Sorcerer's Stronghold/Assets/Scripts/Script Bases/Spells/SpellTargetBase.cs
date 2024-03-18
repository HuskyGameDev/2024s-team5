using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Stronghold.Base
{ 
    public class SpellTargetBase : MonoBehaviour
    {
        //SpellBase.SpellTargets target, int numberOfTargets, GameObject caster
        public static void castOnTarget(SpellBase spell)
        {
            switch (spell.target)
            {
                case SpellBase.SpellTargets.Multiple:
                    createSpellObject(spell.numberOfTargets, spell.target, spell.gameObject, spell.obj);
                    break;

                //all other types have just 1 target location they need
                default: 
                    createSpellObject(1, spell.target, spell.gameObject, spell.obj);
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

            return new Vector2(temp2.x, temp2.y);
        }

        private static void createSpellObject(int number, SpellBase.SpellTargets target, GameObject caster, GameObject obj)
        {
            //TODO: need some way to pause this for multi selection
            for (int i = 0; i < number; i++) 
            {
                Vector2 temp = getLocation(target, caster);

                //instantiate objects
                Instantiate(obj, new Vector3(temp.x, temp.y, 0), Quaternion.identity);


                //attach trigger and effect

                //create visual and aduio effects

                //send it the the target direction
            }
        }
    }
}
