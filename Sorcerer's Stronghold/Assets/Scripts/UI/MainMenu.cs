using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject optionsMenu;

    private enum MENU_TYPE {NONE, MAIN, OPTIONS};
    private bool inGame = false;

    private void Start()
    {
        //freeze time on start
        Time.timeScale = 0f;
    }

    private void Update()
    {
        //if the player is in a game pressing escape will open and close the main menu
        if (inGame && Input.GetKeyDown(KeyCode.Escape))
        {
            if (mainMenu.activeSelf == false)
            {
                setActiveMenu(MENU_TYPE.MAIN);
                Time.timeScale = 0f;
            }
            else
            {
                setActiveMenu(MENU_TYPE.NONE);
                Time.timeScale = 1f;
            }
        }

    }

    //exits the game
    public void exitGame()
    {
        Application.Quit();
    }

    //starts a new game
    public void startGame()
    {
        setActiveMenu(MENU_TYPE.NONE);
        inGame = true;

        //unfreeze time
        Time.timeScale = 1f;
    }
    
    //opens option menu
    public void openOptionsMenu()
    {
        setActiveMenu(MENU_TYPE.OPTIONS);
    }

    //closes the options menu
    public void closeOptionsMenu()
    {
        setActiveMenu(MENU_TYPE.MAIN);
    }

    //sets the active menu to the specified one, turning all others off
    private void setActiveMenu(MENU_TYPE menuName)
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);

        switch(menuName)
        {
            case MENU_TYPE.NONE: 
                break;

            case MENU_TYPE.MAIN:
                mainMenu.SetActive(true);
                break;

            case MENU_TYPE.OPTIONS:
                optionsMenu.SetActive(true);
                break;
        }
    }
}
