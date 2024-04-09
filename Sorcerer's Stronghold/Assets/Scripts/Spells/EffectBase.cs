using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBase : MonoBehaviour
{
    //can just hover over these to see which index they correspond to
    //these are the different effects a spell can have, e's mark things that are exclusive with something
    public enum Effects {eDAMAGE, eHEAL, CREATEWALL, CREATETOWER, ePUSH, ePULL};

    //each index corresponds to the effects listed above
    private bool[] activeEffects = new bool[100];

    //will turn the desired effects on this object
    public void turnOnEffects(Effects[] desiredEffects)
    {
        foreach(Effects e in desiredEffects)
        {
            activeEffects[(int) e] = true;
        }
    }

    //define various effect methods
}
