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
            hasNectar = false;
            flowerSprite.sprite = emptySprite;
            isTarget = false;
            if (random.Next(-1, 100) <= 20 && GameObject.FindGameObjectsWithTag("Flower").Length <= maxFlowers)
            {
                GameObject newFlower = Instantiate(flowerPrefab, GameObject.Find("FlowerParent").transform);
                //Vector3 offset = new Vector3((float)random.Next(-400, 400), (float)random.Next(-400, 400), 0f);
                Vector3 offset = new Vector3((float)(random.Next(1, 3)==1 ? random.Next(-300, -200) : random.Next(200, 300)), (float)(random.Next(1, 3)==1 ? random.Next(-300, -200) : random.Next(200, 300)), 0f);
                newFlower.transform.position = gameObject.transform.position + offset;
                Debug.Log("New flower made!");

                // NOTES: add minimum offset, use parent GameObjects to make sure bees are always on top of flowers
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
        isTarget = true;
    }

}
