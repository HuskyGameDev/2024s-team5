using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    //a list of spells the player can use referenced by name
    private Dictionary<string, Spell> knownSpells = new Dictionary<string, Spell>();

    [SerializeField] string[] keys;
    [SerializeField] Spell[] spells;
    [SerializeField] string currentSpell;

    //lets you initialize a list of spells from the editor because unity doesn't like dictionaries
    //note: this doesn't let you change the dictionary during runtime
    private void Start()
    {
        for(int i = 0; i < keys.Length; i++)
        {
            knownSpells.Add(keys[i], spells[i]);
        }
    }

    //adds a spell to what the player can use
    public void addSpell(string name, Spell newSpell)
    {
        knownSpells.Add(name, newSpell);
    }

    //removes a spell the player has (if they don't have it this does nothing)
    public void removeSpell(string name)
    {
        knownSpells.Remove(name);
    }

    //casts a spell by a given name
    public void castSpell(string name)
    {
        Spell currentSpell;

        //if the spell exists fire it, otherwise print to debug log
        if(knownSpells.TryGetValue(name, out currentSpell))
        {
            currentSpell.fireSpell(this.gameObject, Input.mousePosition);
        }
        else
        {
            Debug.Log("The spell you tried to cast doesn't exist");
        }    
    }

    private void Update()
    {
        //when the player clicks the mouse cast whatever currentspell is set to
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            castSpell(currentSpell);
        }
    }
}
