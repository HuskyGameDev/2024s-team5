using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class buildingSystem : MonoBehaviour
{

    /*********Script currently does not check for building material requirements 2/21/2024*********/
    /******Script currently allows you to place on top of things that it did not make 2/21/2024******/
    /*********Script currently does not give any indication that build mode is active 2/21/2024*********/

    /*
    * Build System Script
    * Clicking "B" will put the player into build mode
    *   If the player is in build mode this will make them exit build mode
    * Left Click to place while in build mode
    * Right Click to delete while in build mode
    * Tag "PlayerBuilt" is used to check if an object can be destroyed
    * 
    * 
    * 
    * This implementation is not perfect
    * Currently it does not include objects like the chest in the start map
    * this means that a player can build on top of these existing objects
    * **/






    //variables
    [SerializeField] private List<TowerTypes> towerList;
    [SerializeField] private Rigidbody2D player;
    private int index = 0;
    private bool buildMode = false;
    [SerializeField] private float buildRange = 8f;

    // Start is called before the first frame update
    void Start()
    {
  
    }

    //**
    //changes selected tower to build
    //**//
    public void setBuildIndex(int index)
    {
        if (index > towerList.Count - 1)
        {
            this.index = index;
        }
        else
        {
            Debug.Log("No Such Tower");
        } 
            
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            buildMode = !buildMode;
        }

        //super temporary for further **use setBuildIndex(int index) for future**
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            index = 0; 
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            index = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            index = 2;
        }

        
        


        


        //build the object if mouse 0 is clicked
        if (buildMode && (Input.GetMouseButtonDown(0)))
        {
            //grabs position of mouse and converts it to a position in the world
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(mousePos.x); 
            int y = Mathf.RoundToInt(mousePos.y);
            //make sure object is in game area
           
            //raycast to get game object
            RaycastHit2D buildHit = Physics2D.Raycast(mousePos, Vector2.zero);
            bool canBuild = false;

            float distanceFromPlayer = Vector2.Distance(player.position, new Vector2(x, y));
            canBuild = distanceFromPlayer <= buildRange;
            //if object can be built
            if (buildHit.collider == null && canBuild)
             {

                Debug.Log("No collider hit. Can build here.");
                //build it
                GameObject newTower = Instantiate(towerList[index].prefab.gameObject, new Vector2(x, y), Quaternion.identity);
                newTower.name = "Tower: " + (x) + "_" + (y );
                newTower.tag = "PlayerBuilt"; //current test tower already has tag but for future towers it will be assigned on instantiation
                    
             }
          
            
        }
        if (buildMode && (Input.GetMouseButtonDown (1)))
        {
            //grabs position of mouse
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(mousePos.x);
            int y = Mathf.RoundToInt(mousePos.y);
            bool canBuild = false;

            float distanceFromPlayer = Vector2.Distance(player.position, new Vector2(x, y));
            canBuild = distanceFromPlayer <= buildRange;
            //uses raycast to get actual game object
            RaycastHit2D findIt = Physics2D.Raycast(mousePos, Vector2.zero);
           if(findIt.collider != null && findIt.collider.gameObject.tag == "PlayerBuilt" && canBuild)
           {
                Destroy(findIt.collider.gameObject);
                       
           }
                   
                
            
        }
    }
}
