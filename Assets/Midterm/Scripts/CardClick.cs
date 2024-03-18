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
        // Call the draw without a deck function if this is called during the draw state, which only happens if the deck is empty
        if (sc.GetCurrentState() == sc.DrawState)
        {
            blade.DrawNoDeck(gameObject);
        }
        // Play the selected card
        else
        {
            blade.PlayCard(gameObject);
        }
    }

}
