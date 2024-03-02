using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClick : MonoBehaviour
{
    private BladeManager blade;

    public void assignBladeRef(BladeManager bladeScript)
    {
        blade = bladeScript;
    }
    
    public void test()
    {
        blade.blade_test(gameObject);
    }


}
