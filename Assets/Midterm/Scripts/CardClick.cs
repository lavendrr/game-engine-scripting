using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClick : MonoBehaviour
{
    private BladeManager blade;
    private StateController sc;

    public void AssignRefs(BladeManager bladeScript, StateController stateController)
    {
        blade = bladeScript;
        sc = stateController;
    }
    
    public void PlayCard()
    {
        if (sc.GetCurrentState() == sc.DrawState)
        {
            blade.DrawNoDeck(gameObject);
        }
        else
        {
            blade.PlayCard(gameObject);
        }
    }

}
