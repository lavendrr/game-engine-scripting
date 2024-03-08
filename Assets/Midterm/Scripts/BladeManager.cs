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
    private List<int> playOrder1, playOrder2 = new List<int>();

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
            //img2.sprite = spriteArray[hand2[i] - 1]; // Use for testing what card the opponent plays
            yield return new WaitForSeconds(0.3f);
        }

        deck1Obj.SetActive(true);
        deck2Obj.SetActive(true);

        done(true);
    }

    public void PlayCard(GameObject card)
    {
        int index = card.transform.GetSiblingIndex();

        if (hand1[index] == 9)
        {
            // Use C# tuple functionality to swap the values of the two stacks and play orders
            (stack1, stack2) = (stack2, stack1);
            (playOrder1, playOrder2) = (playOrder2, playOrder1);
            // Make sure to update opponent's stack text since you changed it
            stack2Text.text = stack2.ToString();
        }
        else if (hand1[index] == 8)
        {
            Debug.Log("Bolting this: " + playOrder2[playOrder2.Count - 1].ToString());
            stack2 -= playOrder2[playOrder2.Count - 1];
            playOrder2.RemoveAt(playOrder2.Count - 1);
            stack2Text.text = stack2.ToString();
        }
        else
        {
            stack1 += hand1[index];
            playOrder1.Add(hand1[index]);
            Debug.Log(playOrder1.ToString());
        }

        card.SetActive(false);
        stack1Text.text = stack1.ToString();

        if (stack1 > stack2)
        {
            sc.ChangeState(sc.PCActionState, 1.5f);
        }
        else if (stack1 == stack2)
        {
            sc.ChangeState(sc.DrawState, 1.5f);
        }
        else if (stack1 < stack2)
        {
            sc.ChangeState(sc.GameEndState, 1.5f);
            Debug.Log("Player 1 lost");
        }
    }

    public void PCPlayCard()
    {
        int stackDifference = stack1 - stack2;
        int index = 0;
        int nearestDifference = 100;
        GameObject nearestObj = null;
        GameObject mirrorObj = null;
        bool tryMirror = false;

        if (stackDifference > 2)
        {
            tryMirror = true;
        }

        // Algorithm to find the card that surpasses the opponent's total by the smallest amount, or selects a card that equals the opponent's total if there is nothing to surpass the opponent
        foreach (int card in hand2)
        {
            GameObject cardObj = hand2Obj.transform.GetChild(index).gameObject;

            if (cardObj.activeSelf)
            {
                // Don't include bolt and mirror cards in the value checks
                if (card != 8 && card != 9)
                {
                    if (card > stackDifference)
                    {
                        if (card - stackDifference < nearestDifference)
                        {
                            nearestDifference = card - stackDifference;
                            nearestObj = cardObj;
                        }
                    }
                    else if (card == stackDifference && nearestObj == null)
                    {
                        nearestObj = cardObj;
                    }
                }
                else if (tryMirror == true && card == 9 && mirrorObj == null)
                {
                    mirrorObj = cardObj;
                }
            }
            
            index++;
        }


        if (nearestObj != null)
        {
            if (mirrorObj != null)
            {
                (stack1, stack2) = (stack2, stack1);
                (playOrder1, playOrder2) = (playOrder2, playOrder1);
                stack1Text.text = stack1.ToString();
                mirrorObj.SetActive(false);
            }
            else
            {
                stack2 += hand2[nearestObj.transform.GetSiblingIndex()];
                playOrder2.Add(hand2[nearestObj.transform.GetSiblingIndex()]);
                nearestObj.SetActive(false);
            }

            stack2Text.text = stack2.ToString();

            if (stack2 == stack1)
            {
                sc.ChangeState(sc.DrawState, 1.5f);
            }
            else if (stack2 > stack1)
            {
                sc.ChangeState(sc.PlayerActionState, 1.5f);
            }
        }
        else
        {
            sc.ChangeState(sc.GameEndState, 1.5f);
            Debug.Log("Player 2 lost");
        }
    }

    public void Draw(int index)
    {
        // Player drawing
        if (deck1[index] == 8 || deck1[index] == 9)
        {
            // Bolt and mirror cards have a value of 1 when initially drawn from the deck
            stack1 += 1;
        }
        else 
        {
            stack1 += deck1[index];
        }

        stack1Text.text = stack1.ToString();

        // Disable the deck UI object if the deck is out of cards
        if (index >= deck1.Length - 1)
        {
            deck1Obj.SetActive(false);
        }

        // PC drawing
        if (deck2[index] == 8 || deck2[index] == 9)
        {
            stack2 += 1;
        }
        else
        {
            stack2 += deck2[index];
        }

        stack2Text.text = stack2.ToString();

        if (index >= deck2.Length - 1)
        {
            deck2Obj.SetActive(false);
        }

        // Clear the playOrder lists and add the new starting stack value to them
        playOrder1.Clear();
        playOrder2.Clear();
        playOrder1.Add(stack1);
        playOrder2.Add(stack2);


        if (stack1 > stack2)
        {
            sc.ChangeState(sc.PCActionState, 1.5f);
        }
        else if (stack2 > stack1)
        {
            sc.ChangeState(sc.PlayerActionState, 1.5f);
        }
        else if (stack1 == stack2)
        {
            sc.ChangeState(sc.DrawState, 1.5f);
        }
    }

    public void Redraw()
    {
        stack1 = 0;
        stack2 = 0;
        stack1Text.text = "";
        stack2Text.text = "";
    }

    public void CheckPlayerCardsLeft()
    {
        // Check if the player has 0 active children left (subtract 1 to discount the parent's transform) and trigger a loss if true
        if (hand1Obj.GetComponentsInChildren<Transform>().GetLength(0) - 1 == 0)
        {
            sc.ChangeState(sc.GameEndState, 0f);
            Debug.Log("Player 1 lost");
        }
    }

}
