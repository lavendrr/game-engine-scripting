using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckDraw : MonoBehaviour
{
    [SerializeField]
    private BladeManager blade;

    private int count = 0;

    public void Draw()
    {
        StartCoroutine(blade.Draw(count));
        count += 1;
    }
}
