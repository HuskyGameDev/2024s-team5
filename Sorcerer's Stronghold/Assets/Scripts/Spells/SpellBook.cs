using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
// using UnityEditor.SearchService;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    //a list of spells the player can use referenced by name
    private Dictionary<string, Spell> knownSpells = new Dictionary<string, Spell>();

    //a gameobject that has spell-objects as its children
    private GameObject spellBookObject = null;

    //use this only for changing values in editor for testing, otherwise call the castSpell function
    [SerializeField] private string currentSpell;

    //lets you initialize a list of spells from the editor because unity doesn't like dictionaries
    //note: this doesn't let you change the dictionary during runtime
    private void Start()
    {
        spellBookObject = GameObject.FindGameObjectWithTag("SpellBook");

        if (spellBookObject == null)
            Debug.LogError("SpellBook object not present, you need to add that to cast spells");

        updateSpellBook();
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

    //this will add any spells on the spellbook gameobject to the list of known spells, it will also clear the known spells prior to doing this (incase any were removed)
    public void updateSpellBook()
    {
        knownSpells.Clear();

        foreach(Spell s in spellBookObject.GetComponentsInChildren<Spell>())
        {
            addSpell(s.name, s);
        }
    }

    private void Update()
    {
        //when the player clicks the mouse cast whatever currentspell is set to
        if (Input.GetKeyDown(KeyCode.Mouse0) && !this.GetComponent<buildingSystem>().buildModeActive())
        {
            castSpell(currentSpell);
        }
    }
}
