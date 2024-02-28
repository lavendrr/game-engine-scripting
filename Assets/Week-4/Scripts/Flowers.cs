using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Flowers : MonoBehaviour
{

    private float nectarRate = 800f;
    private bool hasNectar = false;
    private float nectarCounter = 0f;
    [SerializeField]
    private Color fullColor, emptyColor;
    [SerializeField]
    private Sprite nectarSprite, emptySprite;
    private Image flowerSprite;
    private Random random;
    private bool isTarget = false;
    [SerializeField]
    private GameObject flowerPrefab;
    private int maxFlowers = 15;

    void Start()
    {
        random = new Random();
        nectarRate += random.Next(-250, 500);
        flowerSprite = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasNectar == false && nectarCounter < nectarRate)
        {
            nectarCounter += 1f;
            //Debug.Log("Incremented nectar!");
        } else if (hasNectar == false && nectarCounter >= nectarRate)
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
