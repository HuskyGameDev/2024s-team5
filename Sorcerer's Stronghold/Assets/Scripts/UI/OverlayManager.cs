using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayManager : MonoBehaviour
{
    [SerializeField] GameObject statOverlay;
    [SerializeField] GameObject buildOverlay;

    // Update is called once per frame
    void Update()
    {
        //if build menu is off turn it on when B is pressed, otherwise turn it off when B is pressed
        if(Input.GetKeyDown(KeyCode.B) && buildOverlay.activeSelf)
            buildOverlay.SetActive(false);
        else if(Input.GetKeyDown(KeyCode.B))
            buildOverlay.SetActive(true);
    }
}
