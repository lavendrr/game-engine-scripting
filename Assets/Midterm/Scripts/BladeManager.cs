using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;


public class BladeManager : MonoBehaviour
{
    private int[] totalDeck = { 1, 1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 7, 7, 8, 8, 8, 8, 8, 8, 9, 9, 9, 9 };

    public Sprite[] spriteArray;

    [SerializeField]
    GameObject playerCardPrefab;
    // [SerializeField]
    // Sprite cardback, card1, card2, card3, card4, card5, card6, card7, cardbolt, cardmirror;

    [SerializeField]
    Transform hand1Obj, hand2Obj;

    [SerializeField]
    private int[] hand1;
    [SerializeField]
    private int[] deck1;
    [SerializeField]
    private int[] hand2;
    [SerializeField]
    private int[] deck2;

    // Start is called before the first frame update
    void Start()
    {

        spriteArray = Resources.LoadAll<Sprite>("Card Sprites");

        AssignCards();

        // Instantiate 10 cards and assign their sprites based on their corresponding values from the hand array
        for (int i = 0; i < 10; i++)
        {
            GameObject card1 = Instantiate(playerCardPrefab, hand1Obj);
            Image img1 = card1.GetComponent<Image>();
            img1.sprite = spriteArray[hand1[i] - 1];

            GameObject card2 = Instantiate(playerCardPrefab, hand2Obj);
            Image img2 = card2.GetComponent<Image>();
            img2.sprite = spriteArray[9];
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AssignCards()
    {
        Random rng = new Random();
        int[] randomDeck = totalDeck.OrderBy(x => rng.Next()).ToArray();

        // foreach (int item in randomDeck)
        // {
        //     Debug.Log(item);
        // }

        deck1 = new int[6];
        deck2 = new int[6];
        hand1 = new int[10];
        hand2 = new int[10];

        Array.Copy(randomDeck, 0, hand1, 0, 10);
        Array.Copy(randomDeck, 10, deck1, 0, 6);

        Array.Copy(randomDeck, 16, hand2, 0, 10);
        Array.Copy(randomDeck, 26, deck2, 0, 6);

        Array.Sort(hand1);
        Array.Sort(hand2);

    }
}
