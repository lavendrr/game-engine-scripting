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
        // Draw the next card in the decks, and increment the count for the next draw
        blade.Draw(count);
        count += 1;
    }
}
