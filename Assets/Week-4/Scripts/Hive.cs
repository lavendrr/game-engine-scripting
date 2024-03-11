// Ricky Moctezuma
// Game Engine Scripting SP 2024
// Columbia College Chicago

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hive : MonoBehaviour
{

    // Initialize rate and timer variables for honey and bee generation
    private float honeyRate = 500f;
    private float beeRate = 1200f;
    private float honeyTimer = 0f;
    private float beeTimer = 0f;

    // Instantiate starting values for nectar, honey, bees, and the max number of bees for this hive
    private int nectar = 0;
    private int honey = 0;
    private int totalBees = 2;
    private int maxBees = 10;

    // Get reference to the bee prefab to generate bees
    [SerializeField]
    private GameObject beePrefab;

    // Get references to the honey level sprites and make variables to store the sprites and the hive's image component
    [SerializeField]
    private Sprite sprite1, sprite2, sprite3, spriteEmpty;
    private Sprite[] spriteArray;
    private Image hiveSprite;

    // Make an array to store the UI text objects
    private TextMeshProUGUI[] textArray = new TextMeshProUGUI[3];

    void Start()
    {
        // Add the sprites into the array
        spriteArray = new Sprite[3] {sprite1, sprite2, sprite3};

        // Populate the text array with the hive's child UI objects
        textArray = GetComponentsInChildren<TextMeshProUGUI>();

        hiveSprite = GetComponent<Image>();

        // Initialize the starting amount of bees
        MakeBees(totalBees);

        // Update the UI text
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        // Increment the honey timer if the hive has nectar and if the timer variable is less than the rate value
        if (nectar > 0 && honeyTimer < honeyRate && honey < 9)
        {
            honeyTimer += 1;
        } 
        // Once the timer reaches the rate variable, increment the honey, decrement the nectar used, reset the timer, and update the UI.
        else if (honeyTimer >= honeyRate)
        {
            honey += 1;
            nectar -= 1;
            honeyTimer = 0f;

            UpdateText();

            // Update the sprite if the honey reaches the next increment of 3
            if (honey < 10 && honey % 3 == 0)
            {
                hiveSprite.sprite = spriteArray[(honey / 3) - 1];
            }
        }

        // Increment the bee timer if the hive has honey and if the timer variable is less than the rate value
        if (honey >= 9 && beeTimer < beeRate)
        {
            beeTimer += 1;
        } 
        // Once the timer reaches the rate value, and if the total bees for that hive has not exceeded the limit, instantiate a new bee
        else if (beeTimer >= beeRate && totalBees <= maxBees)
        {
            // Reset the hive's honey level, sprite, and bee timer
            honey = 0;
            hiveSprite.sprite = spriteEmpty;
            beeTimer = 0f;

            // Instantiate a new bee, add to the total bee count, and update the UI
            MakeBees(1);
            totalBees += 1;
            UpdateText();
        }
        
    }

    public void ReceiveNectar()
    {

        // Receive nectar from a bee and update the UI to match
        nectar += 1;
        UpdateText();
    }

    private void MakeBees(int beeAmt)
    {
        // Get the bee parent object's transform
        Transform beeParent = GameObject.Find("BeeParent").transform;

        // Instantiate bees up to the assigned beeAmount, parent them to the bee parent object, and call their Init function so they know which hive created them and to get the random position offset for that bee
        for (int i = 0; i < beeAmt; i++)
        {
            GameObject bee = Instantiate(beePrefab, beeParent);
            Vector3 offset = bee.GetComponent<Bee>().Init(gameObject);
            bee.transform.position = transform.position + offset;
        }
    }

    private void UpdateText()
    {
        // Updates UI text counters for nectar, honey, and bees
        textArray[0].text = "Nectar: " + nectar.ToString();
        textArray[1].text = "Honey: " + honey.ToString();
        textArray[2].text = "Bees: " + totalBees.ToString();
    }
}
