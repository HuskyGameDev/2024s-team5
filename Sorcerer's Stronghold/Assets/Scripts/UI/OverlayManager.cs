using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] GameObject statOverlay;
    [SerializeField] GameObject buildOverlay;

    [Header("Player Attributes")]
    [SerializeField] GameObject player;
    [SerializeField] Slider playerHealthBar;

    [Header("Tower Attributes")]
    [SerializeField] GameObject crystal;
    [SerializeField] Slider crystalHealthBar;

    //grab the components that store health
    private CenterCrystal cCrys;

    private void Start()
    {
        cCrys = crystal.GetComponent<CenterCrystal>();
    }

    // Update is called once per frame
    void Update()
    {
        //if build menu is off turn it on when B is pressed, otherwise turn it off when B is pressed
        if(Input.GetKeyDown(KeyCode.B) && buildOverlay.activeSelf)
            buildOverlay.SetActive(false);
        else if(Input.GetKeyDown(KeyCode.B))
            buildOverlay.SetActive(true);

        updateCrystalHealthBar();
    }

    private void updateCrystalHealthBar()
    {
        crystalHealthBar.value = cCrys.getHealth() / cCrys.getMaxHealth();
    }

    private void updatePlayerHealthBar()
    {

    }
}
