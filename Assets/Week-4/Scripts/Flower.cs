// Ricky Moctezuma
// Game Engine Scripting SP 2024
// Columbia College Chicago

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Flower : MonoBehaviour
{
    // Initiate values for nectar generation
    private float nectarRate = 800f;
    private float nectarCounter = 0f;
    private bool hasNectar = false;
    
    // Get a reference to the flower prefab and instantiate values for flower generation
    [SerializeField]
    private GameObject flowerPrefab;
    private int maxFlowers = 15;
    private bool isTarget = false;

    // Get references to the nectar and non-nectar sprites and get the image component of the flower
    [SerializeField]
    private Sprite nectarSprite, emptySprite;
    private Image flowerSprite;

    private Random random;

    void Start()
    {
        random = new Random();
        // Add a random offset to the nectar generation rate
        nectarRate += random.Next(-250, 500);
        flowerSprite = GetComponent<Image>();
    }

    void Update()
    {
        // If the flower doesn't have nectar and the nectar counter is below the rate value, increment the nectar counter
        if (hasNectar == false && nectarCounter < nectarRate)
        {
            nectarCounter += 1f;
        } 
        // Once the nectar counter has reached the nectar rate value, set nectar status to true, reset the nectar counter, and update the sprite
        else if (hasNectar == false && nectarCounter >= nectarRate)
        {
            hasNectar = true;
            nectarCounter = 0f;
            flowerSprite.sprite = nectarSprite;
        }
    }

    public bool GetNectarStatus()
    {
        return hasNectar;
    }

    public bool GiveNectar()
    {
        if (hasNectar == true)
        {
            // Reset the flower's nectar status, sprite, and target status
            hasNectar = false;
            flowerSprite.sprite = emptySprite;
            isTarget = false;

            // 20% chance to generate a new flower when nectar is harvested, but will only generate if the total number of flowers is within the set flower limit
            if (random.Next(-1, 100) <= 20 && GameObject.FindGameObjectsWithTag("Flower").Length <= maxFlowers)
            {
                // Create a new flower and parent it to the flower parent object
                GameObject newFlower = Instantiate(flowerPrefab, GameObject.Find("FlowerParent").transform);

                // Generate a random offset within two ranges (negative/positive) from the current flower for the X and Y, with a minimum distance to avoid stacking on top of its parent
                float offsetX = (float)(random.Next(1, 3)==1 ? random.Next(-350, -200) : random.Next(200, 350));
                float offsetY = (float)(random.Next(1, 3)==1 ? random.Next(-350, -200) : random.Next(200, 350));
                Vector3 offset = new Vector3(offsetX, offsetY, 0f);

                // Add the offset to the current flower position and set the transform of the new flower to that
                newFlower.transform.position = gameObject.transform.position + offset;

            }
            return true;
        } else {
            return false;
        }
    }

    public bool GetTargetStatus()
    {
        return isTarget;
    }

    public void SetTarget()
    {
        // Mark self as targeted by a bee to avoid being targeted by other bees while nectar is being collected
        isTarget = true;
    }

}
