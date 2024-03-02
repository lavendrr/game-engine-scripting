using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using TMPro;


public class BladeManager : MonoBehaviour
{
    StateController sc;

    private int[] totalDeck = { 1, 1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 7, 7, 8, 8, 8, 8, 8, 8, 9, 9, 9, 9 };

    public Sprite[] spriteArray;

    [SerializeField]
    GameObject playerCardPrefab;

    [SerializeField]
    Transform hand1Obj, hand2Obj;

    [SerializeField]
    GameObject deck1Obj, deck2Obj;

    [SerializeField]
    public TextMeshProUGUI stack1Text, stack2Text;

    private int stack1 = 0;
    private int stack2 = 0;

    [SerializeField]
    private int[] hand1;
    [SerializeField]
    private int[] deck1;
    [SerializeField]
    private int[] hand2;
    [SerializeField]
    private int[] deck2;

    void Start()
    {
        spriteArray = Resources.LoadAll<Sprite>("Card Sprites");

        sc = gameObject.GetComponent<StateController>();
    }

    public void AssignCards()
    {
        Random rng = new Random();
        int[] randomDeck = totalDeck.OrderBy(x => rng.Next()).ToArray();

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

    public IEnumerator CreateCards(System.Action<bool> done)
    {
        // Instantiate 10 cards and assign their sprites based on their corresponding values from the hand array
        for (int i = 0; i < 10; i++)
        {
            GameObject card1 = Instantiate(playerCardPrefab, hand1Obj);
            card1.GetComponent<CardClick>().AssignBladeRef(this);
            Image img1 = card1.GetComponent<Image>();
            img1.sprite = spriteArray[hand1[i] - 1];
            yield return new WaitForSeconds(0.3f);

            // Set all of Player 2's cards to use the card back sprite since they are hidden
            GameObject card2 = Instantiate(playerCardPrefab, hand2Obj);
            Image img2 = card2.GetComponent<Image>();
            img2.sprite = spriteArray[9];
            yield return new WaitForSeconds(0.3f);
        }

        deck1Obj.SetActive(true);
        deck2Obj.SetActive(true);

        done(true);
    }

    public void PlayCard(GameObject card)
    {
        Debug.Log("Clicked!");
        int index = card.transform.GetSiblingIndex();
        stack1 += hand1[index];
        card.SetActive(false);
        stack1Text.text = stack1.ToString();
    }

    public IEnumerator Draw(int index)
    {
        stack1 += deck1[index];
        stack1Text.text = stack1.ToString();
        if (index >= deck1.Length - 1)
        {
            deck1Obj.SetActive(false);
        }

        stack2 += deck2[index];
        stack2Text.text = stack2.ToString();
        if (index >= deck2.Length - 1)
        {
            deck2Obj.SetActive(false);
        }


        if (stack1 > stack2)
        {
            sc.ChangeState(sc.PCActionState, 1.3f);
        }
        else if (stack2 > stack1)
        {
            sc.ChangeState(sc.PlayerActionState, 1.3f);
        }
        else if (stack1 == stack2)
        {
            sc.ChangeState(sc.DrawState, 1.5f);
            yield return new WaitForSeconds(1.5f);
            stack1 = 0;
            stack2 = 0;
            stack1Text.text = "";
            stack2Text.text = "";
        }
    }

}
