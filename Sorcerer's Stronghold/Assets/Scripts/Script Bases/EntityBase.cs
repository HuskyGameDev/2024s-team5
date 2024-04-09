using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stronghold.Base{
    public abstract class EntityBase : MonoBehaviour{
        //These are the elements added in the game
        public enum Elements{
            Null,
            Normal,
            Fire,
            Water,
            Life,
            Death,
            Earth
        }

        /*
        These are elemental vulnerabilities and resistances. To use them, you take the element
        of an entity and use it as an index for the lists. The resulting element will be either
        its resistance or vulnerability. Elements.Null replaces null as nothing, since enums 
        cannot be null. Elements.Null should never be used as an actual element.  
        */
        public Elements[] resist = {Elements.Null, Elements.Null, Elements.Null, Elements.Fire, Elements.Null, Elements.Life, Elements.Fire};
        public Elements[] vulnerable = {Elements.Null, Elements.Null, Elements.Water, Elements.Life, Elements.Death, Elements.Null};

        /*
        Most of this is fairly self explanitory. SerializeField marks a private
        variable to be editable from the inspector in Unity, without exposing
        the variables to other scripts. 
        */
        [Header("Entity Stats")]
        [SerializeField] protected float maximumHealth = 10f;
        [SerializeField] protected float health = 10f;
        [SerializeField] protected float speed = 1f;
        [SerializeField] protected float damage = 2f;
        public Elements element = Elements.Normal;
        [SerializeField] protected float resistanceFactor = 0.5f;
        [SerializeField] protected float vulnerableFactor = 1.5f;

        protected bool alive = true;

        /*
        This will be a shared method across all entities. Anything that has a health
        bar can be damaged, or healed, by calling alterHealth. Damage should have
        negative values, healing should have positive values. 
        */
        public void alterHealth(float d, Elements e){
            if (resist[(int) element] == e){
                d *= resistanceFactor;
            }else if (vulnerable[(int) element] == e){
                d *= vulnerableFactor;
            }
            health += d;
            if (health > maximumHealth){
                health = maximumHealth;
            }else if (health <= 0){
                onDeath();
            }
        }

        /*
        This method alters an entity's health by a percentage of its maximum health.
        This can be useful if we want to add things of the "heal by 25%" nature. 
        */
        public void alterHealthPercentage(float p){
            health += maximumHealth * p;
            if (health > maximumHealth){
                health = maximumHealth;
            }else if (health <= 0){
                onDeath();
            }
        }

        public float getHealth(){
            return health;
        }

        public float getMaxHealth()
        {
            return maximumHealth;
        }

        public bool isAlive(){
            return alive;
        }

        /*
        This method allows for different death behaviors. Individual scripts can
        override this method to have unique behavior. Instead of just dieing, an
        enemy may explode, or enter a second phase, etc. This method should always
        call die() at some point.
        */
        protected abstract void onDeath();

        /*
        This method should always just destroy the game object. 
        */
        protected void die(){
            Destroy(gameObject);
        }
    }
}
