using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClick : MonoBehaviour
{
    private BladeManager blade;

    public void AssignBladeRef(BladeManager bladeScript)
    {
        blade = bladeScript;
    }
    
    public void PlayCard()
    {
        blade.PlayCard(gameObject);
    }

}
