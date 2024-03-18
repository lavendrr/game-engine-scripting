using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random; // Using Random from System instead of Unity's Random
using TMPro;
using DG.Tweening; // Using DoTween for animations
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class BladeManager : MonoBehaviour
{
    StateController sc; // Reference to StateController

    // Array representing the total deck of cards
    private int[] totalDeck = { 1, 1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 7, 7, 8, 8, 8, 8, 8, 8, 9, 9, 9, 9 };

    private Sprite[] spriteArray; // Array to hold card sprites

    // Serialized fields for various game objects and UI elements
    [SerializeField] GameObject playerCardPrefab;
    [SerializeField] Transform hand1Obj, hand2Obj;
    [SerializeField] GameObject deck1Obj, deck2Obj;
    [SerializeField] TextMeshProUGUI stack1Text, stack2Text;

    // Variables to track stack values and card play orders
    private int stack1 = 0;
    private int stack2 = 0;
    private List<int> playOrder1 = new List<int>();
    private List<int>playOrder2 = new List<int>();
    private int recentBolt = 0;

    // Lists to hold cards in stacks
    private List<GameObject> stack1Cards = new List<GameObject>();
    private List<GameObject> stack2Cards = new List<GameObject>();

    // Arrays to represent player hands and decks
    private int[] hand1;
    private int[] deck1;
    private int[] hand2;
    private int[] deck2;

    private bool gameWon; // Flag to track game state

    void Start()
    {
        // Load card sprites from resources
        spriteArray = Resources.LoadAll<Sprite>("Card Sprites");

        // Get reference to StateController
        sc = gameObject.GetComponent<StateController>();
    }

    // Method to assign cards to players
    public void AssignCards()
    {
        // Shuffle the total deck
        Random rng = new Random();
        int[] randomDeck = totalDeck.OrderBy(x => rng.Next()).ToArray();

        // Assign cards to player hands and decks
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

    // Coroutine to create cards for players
    public IEnumerator CreateCards(System.Action<bool> done)
    {
        // Instantiate 10 cards and assign their sprites based on their corresponding values from the hand array for player 1, and hide all the sprites for player 2
        for (int i = 0; i < 10; i++)
        {
            GameObject card1 = Instantiate(playerCardPrefab, hand1Obj);
            card1.GetComponent<CardClick>().AssignRefs(this, sc);
            Image img1 = card1.GetComponent<Image>();
            img1.sprite = spriteArray[hand1[i] - 1];
            yield return new WaitForSeconds(0.3f);

            GameObject card2 = Instantiate(playerCardPrefab, hand2Obj);
            Image img2 = card2.GetComponent<Image>();
            img2.sprite = spriteArray[9]; // Card back sprite for player 2
            yield return new WaitForSeconds(0.3f);
        }

        // Activate deck objects
        deck1Obj.SetActive(true);
        deck2Obj.SetActive(true);

        done(true);
    }

    // Method to handle player card play
    public void PlayCard(GameObject card)
    {
        // Index of played card in player's hand
        int index = card.transform.GetSiblingIndex();

        // Check if player has only one card left
        bool oneCardLeft = hand1Obj.GetComponentsInChildren<Transform>().Length - 1 == 1;

        // Flag to check if opponent has played a Bolt card
        bool opponentBolted = false;

        // Check if opponent played Bolt card
        if (playOrder2[playOrder2.Count - 1] == -1)
        {
            opponentBolted = true;
            playOrder2.Remove(-1);
        }

        // Handle Mirror card play
        if (hand1[index] == 9)
        {
            if (oneCardLeft == true)
            {
                gameWon = false;
                sc.ChangeState(sc.GameEndState, 0f);
                return;
            }
            // Swap stack values and play orders
            (stack1, stack2) = (stack2, stack1);
            (playOrder1, playOrder2) = (playOrder2, playOrder1);
            stack1Text.text = stack1.ToString();
            stack2Text.text = stack2.ToString();

            // Animate mirror card usage
            AnimateDuplicateCard(stack1Cards, card.transform.position, stack1Text.transform, card.transform.position + new Vector3(0f, 100f, 0f), hand1[index], true);
            MirrorReparent();
        }
        // Handle Bolt card play
        else if (hand1[index] == 8)
        {
            if (oneCardLeft == true)
            {
                gameWon = false;
                sc.ChangeState(sc.GameEndState, 0f);
                return;
            }
            // Stores opponent's most recent card value in recentBolt, and removes it from their stack, then add a placeholder -1 value to the play order to indicate that a bolt was just played
            recentBolt = playOrder2[playOrder2.Count - 1];
            stack2 -= playOrder2[playOrder2.Count - 1];
            playOrder2.RemoveAt(playOrder2.Count - 1);
            stack2Text.text = stack2.ToString();
            playOrder1.Add(-1);

            // Animate bolt card usage
            AnimateDuplicateCard(stack1Cards, card.transform.position, stack1Text.transform, card.transform.position + new Vector3(0f, 100f, 0f), hand1[index], true);

            // Destroy the corresponding card object and remove it from their object play order
            Destroy(stack2Cards[stack2Cards.Count - 1]);
            stack2Cards.RemoveAt(stack2Cards.Count - 1);
        }
        // Handle usage of a 1 card to undo a bolt
        else if (hand1[index] == 1 && opponentBolted)
        {
            // Add the recentBolt value back to the stack and play order
            stack1 += recentBolt;
            playOrder1.Add(recentBolt);
            
            // Animate the 1 card being used but not added to the stack
            AnimateDuplicateCard(stack1Cards, card.transform.position, stack1Text.transform, card.transform.position + new Vector3(0f, 100f, 0f), hand1[index], true);

            // Set the initial offset if there are no cards in the stack, or offset the card according to the previous card in the stack
            if (stack1Cards.Count == 0)
            {
                AnimateDuplicateCard(stack1Cards, stack1Text.transform.position + new Vector3(300f, 0f, 0f), stack1Text.transform, stack1Text.transform.position + new Vector3(300f, 0f, 0f), recentBolt);
            }
            else
            {
                AnimateDuplicateCard(stack1Cards, stack1Cards[stack1Cards.Count - 1].transform.position + new Vector3(75f, 0f, 0f), stack1Text.transform, stack1Cards[stack1Cards.Count - 1].transform.position + new Vector3(75f, 0f, 0f), recentBolt);
            }
        }
        // Handle normal number card play
        else
        {
            // Add the card's value to the stack and play order
            stack1 += hand1[index];
            playOrder1.Add(hand1[index]);

            // Set the initial offset if there are no cards in the stack, or offset the card according to the previous card in the stack
            if (stack1Cards.Count == 0)
            {
                AnimateDuplicateCard(stack1Cards, card.transform.position, stack1Text.transform, stack1Text.transform.position + new Vector3(300f, 0f, 0f), hand1[index]);
            }
            else
            {
                AnimateDuplicateCard(stack1Cards, card.transform.position, stack1Text.transform, stack1Cards[stack1Cards.Count - 1].transform.position + new Vector3(75f, 0f, 0f), hand1[index]);
            }
        }

        // Deactivate played card and update text
        card.SetActive(false);
        stack1Text.text = stack1.ToString();

        // Determine game state based on stack values
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
            gameWon = false;
            sc.ChangeState(sc.GameEndState, 1.5f);
        }
    }

    // Method for PC to play a card
    public void PCPlayCard()
    {
        // Initialize values for PC algorithm
        int stackDifference = stack1 - stack2;
        int index = 0;
        int nearestDifference = 100;
        GameObject nearestObj = null;
        GameObject mirrorObj = null;
        GameObject boltObj = null;
        GameObject oneObj = null;
        bool tryMirror = false;
        bool tryBolt = false;
        bool tryUndoBolt = false;
        int numActiveNumberCards = 0;

        // Remove Bolt card from own play order if present
        if (playOrder2.Count != 0)
        {
            if (playOrder2[playOrder2.Count - 1] == -1)
            {
                playOrder2.RemoveAt(playOrder2.Count - 1);
            }
        }

        // Check if Bolt card should be played
        if (playOrder1[playOrder1.Count - 1] > 4)
        {
            tryBolt = true;
        }

        // Check if Mirror card should be played
        if (stackDifference > 2)
        {
            tryMirror = true;
        }

        // Check if a one should be used to undo a bolt card
        if (playOrder1[playOrder1.Count - 1] == -1)
        {
            tryUndoBolt = true;
            playOrder1.Remove(-1);
        }

        // Algorithm to find the card that surpasses the opponent's total by the smallest amount, or selects a card that equals the opponent's total if there is nothing to surpass the opponent
        foreach (int card in hand2)
        {
            // Find the index of the current card
            GameObject cardObj = hand2Obj.transform.GetChild(index).gameObject;

            // Checks that the card hasn't been played already
            if (cardObj.activeSelf)
            {
                // Don't include bolt and mirror cards in the value checks
                if (card != 8 && card != 9)
                {
                    numActiveNumberCards += 1;

                    if (card > stackDifference)
                    {
                        // If the card results in a lower stack difference, set that difference as the new nearest difference and store the card object
                        if (card - stackDifference < nearestDifference)
                        {
                            nearestDifference = card - stackDifference;
                            nearestObj = cardObj;
                        }
                    }
                    // If there isn't a winning card that has been found yet and the current card would cause a redraw, store the current card
                    else if (card == stackDifference && nearestObj == null)
                    {
                        nearestObj = cardObj;
                    }
                    // Store a one card if it is found
                    if (card == 1 && oneObj == null)
                    {
                        oneObj = cardObj;
                    }
                }
                // Store a bolt card if it is found
                else if (card == 8 && boltObj == null)
                {
                    boltObj = cardObj;
                }
                // Store a mirror card if it is found
                else if (card == 9 && mirrorObj == null)
                {
                    mirrorObj = cardObj;
                }
            }

            index++;
        }

        // Follow algorithm logic to play appropriate card based on the situation (see PlayCard() above for details on the implementation for the different types of cards, it's the same as here)
        if (nearestObj != null || boltObj != null || mirrorObj != null)
        {
            if (oneObj != null && tryUndoBolt == true) // Try to use a one card to undo a bolt
            {
                stack2 += recentBolt;
                playOrder2.Add(recentBolt);
                AnimateDuplicateCard(stack2Cards, oneObj.transform.position, stack2Text.transform, oneObj.transform.position + new Vector3(0f, -100f, 0f), 1, true);
                oneObj.SetActive(false);
                if (stack2Cards.Count == 0)
                {
                    AnimateDuplicateCard(stack2Cards, stack2Text.transform.position + new Vector3(-300f, 0f, 0f), stack2Text.transform, stack2Text.transform.position + new Vector3(-300f, 0f, 0f), recentBolt);
                }
                else
                {
                    AnimateDuplicateCard(stack2Cards, stack2Cards[stack2Cards.Count - 1].transform.position + new Vector3(-75f, 0f, 0f), stack2Text.transform, stack2Cards[stack2Cards.Count - 1].transform.position + new Vector3(-75f, 0f, 0f), recentBolt);
                }
            }
            else if (boltObj != null && tryBolt == true) // Bolt the opponent
            {
                recentBolt = playOrder1[playOrder1.Count - 1];
                stack1 -= playOrder1[playOrder1.Count - 1];
                playOrder1.RemoveAt(playOrder1.Count - 1);
                stack1Text.text = stack1.ToString();
                playOrder2.Add(-1);
                AnimateDuplicateCard(stack2Cards, boltObj.transform.position, stack2Text.transform, boltObj.transform.position + new Vector3(0f, -100f, 0f), 8, true);
                boltObj.SetActive(false);
                Destroy(stack1Cards[stack1Cards.Count - 1]);
                stack1Cards.RemoveAt(stack1Cards.Count - 1);
            }
            else if (mirrorObj != null && tryMirror == true) // Mirror the opponent
            {
                (stack1, stack2) = (stack2, stack1);
                (playOrder1, playOrder2) = (playOrder2, playOrder1);
                stack1Text.text = stack1.ToString();
                AnimateDuplicateCard(stack2Cards, mirrorObj.transform.position, stack2Text.transform, mirrorObj.transform.position + new Vector3(0f, -100f, 0f), 9, true);
                mirrorObj.SetActive(false);
                MirrorReparent();
            }
            else if ((numActiveNumberCards == 1 && hand2Obj.GetComponentsInChildren<Transform>().GetLength(0) - 1 > 1) || nearestObj == null) // If there is only one non-number card left, or if no non-losing number cards were found, play a bolt or mirror to avoid losing
            {
                if (boltObj != null) // Bolt the opponent
                {
                    recentBolt = playOrder1[playOrder1.Count - 1];
                    stack1 -= playOrder1[playOrder1.Count - 1];
                    playOrder1.RemoveAt(playOrder1.Count - 1);
                    stack1Text.text = stack1.ToString();
                    playOrder2.Add(-1);
                    AnimateDuplicateCard(stack2Cards, boltObj.transform.position, stack2Text.transform, boltObj.transform.position + new Vector3(0f, -100f, 0f), 8, true);
                    boltObj.SetActive(false);
                    Destroy(stack1Cards[stack1Cards.Count - 1]);
                    stack1Cards.RemoveAt(stack1Cards.Count - 1);
                }
                else if (mirrorObj != null) // Mirror the opponent
                {
                    (stack1, stack2) = (stack2, stack1);
                    (playOrder1, playOrder2) = (playOrder2, playOrder1);
                    stack1Text.text = stack1.ToString();
                    AnimateDuplicateCard(stack2Cards, mirrorObj.transform.position, stack2Text.transform, mirrorObj.transform.position + new Vector3(0f, -100f, 0f), 9, true);
                    mirrorObj.SetActive(false);
                    MirrorReparent();
                }
            }
            // Play a number card normally
            else
            {
                stack2 += hand2[nearestObj.transform.GetSiblingIndex()];
                playOrder2.Add(hand2[nearestObj.transform.GetSiblingIndex()]);

                if (stack2Cards.Count == 0)
                {
                    AnimateDuplicateCard(stack2Cards, nearestObj.transform.position, stack2Text.transform, stack2Text.transform.position + new Vector3(-300f, 0f, 0f), hand2[nearestObj.transform.GetSiblingIndex()]);
                }
                else
                {
                    AnimateDuplicateCard(stack2Cards, nearestObj.transform.position, stack2Text.transform, stack2Cards[stack2Cards.Count - 1].transform.position + new Vector3(-75f, 0f, 0f), hand2[nearestObj.transform.GetSiblingIndex()]);
                }

                nearestObj.SetActive(false);
            }

            stack1Text.text = stack1.ToString();
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
        // Player 1 wins if no suitable card to play can be found
        else
        {
            gameWon = true;
            sc.ChangeState(sc.GameEndState, 1.5f);
        }
    }

    // Method for drawing cards
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

        AnimateDuplicateCard(stack1Cards, deck1Obj.transform.position, stack1Text.transform, stack1Text.transform.position + new Vector3(300f, 0f, 0f), deck1[index]);
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

        AnimateDuplicateCard(stack2Cards, deck2Obj.transform.position, stack2Text.transform, stack2Text.transform.position + new Vector3(-300f, 0f, 0f), deck2[index]);
        stack2Text.text = stack2.ToString();

        // Disable the deck UI object if the deck is out of cards
        if (index >= deck2.Length - 1)
        {
            deck2Obj.SetActive(false);
        }

        playOrder1.Add(stack1);
        playOrder2.Add(stack2);

        // Trigger next game state based on results of the draw
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

    // Method for drawing a card when the deck is empty
    public void DrawNoDeck(GameObject playerCard)
    {
        int index = playerCard.transform.GetSiblingIndex();

        // Player drawing
        if (hand1[index] == 8 || hand1[index] == 9)
        {
            // Bolt and mirror cards have a value of 1 when initially drawn from the deck
            stack1 += 1;
        }
        else 
        {
            stack1 += hand1[index];
        }

        playerCard.SetActive(false);
        playOrder1.Add(stack1);
        stack1Text.text = stack1.ToString();

        // PC Drawing
        GameObject lowestCard = null;
        int lowestValue = 100;
        int PCindex = 0;

        // Find the lowest card in the PC's hand and play that to start
        foreach (int card in hand2)
        {
            int cardRealValue = card; // Assigns card into a new variable so it can be changed (cannot change foreach iteration variables)
            GameObject cardObj = hand2Obj.transform.GetChild(PCindex).gameObject;
            
            // Only looks at active cards
            if (cardObj.activeSelf)
            {
                // Convert blade and mirror cards to a value of 1
                if (cardRealValue == 8 || cardRealValue == 9)
                {
                    cardRealValue = 1;
                }

                if (cardRealValue < lowestValue)
                {
                    lowestValue = cardRealValue;
                    lowestCard = cardObj;
                }
            }

            PCindex++;
        }

        // Play the selected card, or player 1 wins if PC has no valid card left
        if (lowestCard != null)
        {
            stack2 += lowestValue;
            playOrder2.Add(stack2);
            lowestCard.SetActive(false);
            stack2Text.text = stack2.ToString();
        }
        else
        {
            gameWon = true;
            sc.ChangeState(sc.GameEndState, 1.5f);
        }

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

    // Method to reset game state upon drawing new cards
    public void Redraw()
    {
        stack1 = 0;
        stack2 = 0;
        stack1Text.text = "";
        stack2Text.text = "";
        playOrder1.Clear();
        playOrder2.Clear();
        stack1Cards.Clear();
        stack2Cards.Clear();
        // Clears the played card objects from the field
        foreach (Transform child in stack1Text.gameObject.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name != "Stack1Text")
            {
                Destroy(child.gameObject);
            }
        }
        foreach (Transform child in stack2Text.gameObject.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name != "Stack2Text")
            {
                Destroy(child.gameObject);
            }
        }
    }

    // Method to help create a new card and animate it
    public void AnimateDuplicateCard(List<GameObject> cardList, Vector3 startingPosition, Transform parentTransform, Vector3 endingPosition, int cardValue, bool destroy = false)
    {
        // Instantiate a new card at the starting position, disable its button component, and assign its sprite
        GameObject playedCard = Instantiate(playerCardPrefab, parentTransform);
        playedCard.transform.position = startingPosition;
        playedCard.GetComponent<Button>().enabled = false;
        playedCard.GetComponent<Image>().sprite = spriteArray[cardValue - 1];

        playedCard.transform.DOMove(endingPosition, 1f).OnComplete(() =>
        {
            // Destroy the card in case we don't want to add it to the stack (used for bolt/mirror/undoing bolt)
            if (destroy)
            {
                Destroy(playedCard);
            }
            else
            {
                cardList.Add(playedCard);
            }
        });
    }

    // Method to handle Mirror card reparenting and animating of stack swapping
    public void MirrorReparent()
    {
        // Reparents and animates each played card
        foreach (GameObject child in stack1Cards)
        {
            if (child != null){
                if (child.name != "Stack1Text")
                {
                    child.transform.SetParent(stack2Text.transform, true);
                    child.transform.DOMove(new Vector3(stack2Text.transform.position.x - child.transform.localPosition.x, stack2Text.transform.position.y, stack2Text.transform.position.z), 1f).OnComplete(() =>
                    {
                    });
                }
            }
        }
        foreach (GameObject child in stack2Cards)
        {
            if (child != null){
                if (child.name != "Stack2Text")
                {
                    child.transform.SetParent(stack1Text.transform, true);
                    child.transform.DOMove(new Vector3(stack1Text.transform.position.x - child.transform.localPosition.x, stack1Text.transform.position.y, stack1Text.transform.position.z), 1f).OnComplete(() =>
                    {
                    });
                }
            }
        }
        // Swap the values of the played card stacks
        (stack1Cards, stack2Cards) = (stack2Cards, stack1Cards);
    }

    public void CheckPlayerCardsLeft()
    {
        // Check if the player has 0 active children left (subtract 1 to discount the parent's transform) and trigger a loss if true
        if (hand1Obj.GetComponentsInChildren<Transform>().GetLength(0) - 1 == 0)
        {
            gameWon = false;
            sc.ChangeState(sc.GameEndState, 0f);
        }
    }

    public bool GetWinState()
    {
        return gameWon;
    }


    public void RestartGame()
    {
        SceneManager.LoadScene("Blade");
    }
}
