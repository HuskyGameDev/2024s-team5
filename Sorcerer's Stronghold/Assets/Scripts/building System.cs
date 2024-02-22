using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingSystem : MonoBehaviour
{

    /*********Script currently does not check for building material requirements 2/21/2024*********/
    /******Script currently allows you to place on top of things that it did not make 2/21/2024******/
    /*********Script currently only places 1 type of tower "TestTower" see line 47 2/21/2024*********/
    /******Script currently does not have a buildrange, if you can click it you can place it 2/21/2024******/
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
    * To compensate for (0,0) being in the middle of the map
    *   int x = Mathf.RoundToInt(mousePos.x) + width / 2;
    *   int y = Mathf.RoundToInt(mousePos.y) + height / 2;
    *   NOTE: You will want to subtract width /2 and height / 2 from x and y respectively to convert
    *   back to unity coordinate system. i.e new Vector2(x - width  / 2, y - height / 2)
    *  
    * 
    * 
    * The implementation makes a 2d array the size of the gameworld to keep track of 
    * where the player can build
    * The array "buildArray" will contain a true at [x][y] if there is a building already there
    * and a false if there is no building there
    * 
    * This implementation is not perfect
    * Currently it does not include objects like the chest in the start map
    * this means that a player can build on top of these existing objects
    * **/






    //variables
    public GameObject building; //this is the object that will be built, assigned in unity editor
    [SerializeField] int width = 20;
    [SerializeField] int height = 20;

    /*
     * buildArray key
     * false: this is a free space, the player can build here
     * true: The player has already built something here, they cannot place a new building**/
    bool[,] buildArray; //keeps track of where things can be built
    bool buildMode = false;

    // Start is called before the first frame update
    void Start()
    {
        //this makes the 2d array the same size as the game area on start
        buildArray = new bool[width, height];

    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            buildMode = !buildMode;
        }
        //build the object if mouse 0 is clicked
        if (buildMode && (Input.GetMouseButtonDown(0)))
        {
            //grabs position of mouse and converts it to a position in the world
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(mousePos.x) + width / 2; //(0,0) is at center of map this corrects for that
            int y = Mathf.RoundToInt(mousePos.y) + height / 2;
            //make sure object is in game area
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                //if object can be built
                if (!buildArray[x, y])
                {
                    //build it
                    GameObject newTower = Instantiate(building, new Vector2(x - width  / 2, y - height / 2), Quaternion.identity);
                    newTower.name = "Tower: " + (x - width / 2) + "_" + (y - height / 2);
                    newTower.tag = "PlayerBuilt"; //current test tower already has tag but for future towers it will be assigned on instantiation
                    buildArray[x, y] = true;
                    
                }
          
            }
        }
        if (buildMode && (Input.GetMouseButtonDown (1)))
        {
            //grabs position of mouse and converts it to a position in the world
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(mousePos.x) + width / 2; //(0,0) is at center of map this corrects for that
            int y = Mathf.RoundToInt(mousePos.y) + height / 2;
            //make sure object is in game area
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                //check that object is there
                if (buildArray[x, y])
                {
                    //uses raycast to get actual game object
                    RaycastHit2D findIt = Physics2D.Raycast(mousePos, Vector2.zero);
                    if(findIt.collider != null && findIt.collider.gameObject.tag == "PlayerBuilt")
                    {
                        Destroy(findIt.collider.gameObject);
                        buildArray[x, y] = false;
                    }
                   
                }
            }
        }
    }
}
