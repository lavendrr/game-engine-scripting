// Ricky Moctezuma
// Game Engine Scripting SP 2024
// Columbia College Chicago

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = System.Random;

public class Bee : MonoBehaviour
{
    // Instantiate variables to store the parent hive and the flower array
    private GameObject hive;
    private GameObject[] flowerArray;

    // Make a variable to track whether the bee is currently retrieving nectar from a flower
    private bool inProgress;

    // Get references to the sprites for when the bee is carrying nectar vs. not, and get the bee's image component
    [SerializeField]
    private Sprite beeNectar, beeEmpty;
    private Image beeSprite;
    
    private Random random;
    private Vector3 offset;

    public Vector3 Init(GameObject parentHive)
    {
        // Store the parent hive and assign global variables
        hive = parentHive;
        inProgress = false;
        beeSprite = GetComponent<Image>();
        random = new Random();

        // Generate a random offset within two ranges so the bee is visible on the edges of the hive (the bees will all be below the hive so as not to cover the UI and honey level indicators)
        offset = new Vector3((float)(random.Next(1, 3)==1 ? random.Next(-150, -100) : random.Next(50, 150)), (float)(random.Next(1, 3)==1 ? random.Next(-150, -50) : random.Next(50, 150)), 0f);
        return offset;
    }

    void Update()
    {
        // Only check flowers if the bee is not already retrieving nectar from a flower
        if (inProgress == false)
        {
            // Update the flower array in case new flowers have been made
            flowerArray = GameObject.FindGameObjectsWithTag("Flower");
            CheckAnyFlower();
        }
    }

    public void CheckAnyFlower()
    {
        inProgress = true;

        // Pick a random flower from the array
        GameObject targetFlower = flowerArray[random.Next(0, flowerArray.Length)];

        // Get the flower's script component and store it
        Flowers flowerScript = targetFlower.GetComponent<Flowers>();

        // If the flower has nectar and is not already the target of another bee, begin nectar retrieval
        if (flowerScript.GetNectarStatus() == true && flowerScript.GetTargetStatus() == false)
        {
            // Set the flower as a target so other bees don't go after it
            flowerScript.SetTarget();

            // Move towards the flower
            transform.DOMove(targetFlower.transform.position, 2f).OnComplete(() =>
            {
                // After reaching the flower, take the nectar from the flower and update the bee sprite to have nectar
                flowerScript.GiveNectar();
                beeSprite.sprite = beeNectar;

                // Move towards the hive
                transform.DOMove(hive.transform.position + offset, 2f).OnComplete(() =>
                {
                    // Give the nectar to the hive, reset the bee sprite to not have nectar, and set the progress variable to false so that a new flower can be checked
                    hive.GetComponent<Hive>().ReceiveNectar();
                    beeSprite.sprite = beeEmpty;
                    inProgress = false;
                });
            });
        }
        // If the flower that was chosen doesn't have nectar or is already targeted by another bee, set progress to false to check another flower on the next frame
        else 
        {
            inProgress = false;
        }
    }
}
