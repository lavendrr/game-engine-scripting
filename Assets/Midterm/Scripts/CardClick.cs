using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClick : MonoBehaviour
{
    public void test(GameObject cardObj)
    {
        GameObject blade = GameObject.Find("Blade");
        blade.GetComponent<BladeManager>().blade_test(cardObj);
    }

}
