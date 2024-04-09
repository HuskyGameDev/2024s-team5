using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingBase : MonoBehaviour
{
    //can just hover over these to see which index they correspond to
    //these are the different targeting methods, e's mark things that are exclusive with something
    public enum Targets {PIERCING, AOE, WAVE, TIMER, eMULTI, eSINGLE, ePLAYER, eENEMY, eSPAWNPLAYER, eSPAWNMOUSE};

    //each index corresponds to the effects listed above
    private bool[] activeTargets = new bool[100];

    //will turn the desired effects on this object
    public void turnOnTargets(Targets[] desiredEffects)
    {
        foreach (Targets e in desiredEffects)
        {
            activeTargets[(int)e] = true;
        }
    }

    //define various targeting methods
}
