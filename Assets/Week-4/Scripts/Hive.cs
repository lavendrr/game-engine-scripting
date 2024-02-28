using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hive : MonoBehaviour
{

    private float honeyRate = 500f;
    private float beeRate = 1200f;
    private int totalBees = 2;
    private int beeLimit = 10;

    [SerializeField]
    private GameObject beePrefab;
    private int nectar = 0;
    private int honey = 0;
    private float honeyTimer = 0f;
    private float beeTimer = 0f;
    

    [SerializeField]
    private Sprite sprite1, sprite2, sprite3, spriteEmpty;
    private Sprite[] spriteArray;
    private Image hiveSprite;


    // Start is called before the first frame update
    void Start()
    {
        spriteArray = new Sprite[3] {sprite1, sprite2, sprite3};

        hiveSprite = GetComponent<Image>();

        MakeBees(totalBees);
    }

    // Update is called once per frame
    void Update()
    {
        // Increment the honey timer if the hive has nectar and if the timer variable is less than the rate. 
        //Once the timer reaches the rate variable, increment the honey, decrement the nectar used, and reset the timer
        if (nectar > 0 && honeyTimer < honeyRate)
        {
            honeyTimer += 1;
        } else if (honeyTimer >= honeyRate)
        {
            honey += 1;
            nectar -= 1;
            honeyTimer = 0f;

            if (honey < 10 && honey % 3 == 0)
            {
                hiveSprite.sprite = spriteArray[(honey / 3) - 1];
            }
        }

        if (honey >= 9 && beeTimer < beeRate)
        {
            beeTimer += 1;
        } else if (honey > 0 && beeTimer >= beeRate && totalBees <= beeLimit)
        {
            honey = 0;
            hiveSprite.sprite = spriteEmpty;
            totalBees += 1;
            beeTimer = 0f;
            MakeBees(1);
            Debug.Log("New bee made!");
        }
        
    }

    public void ReceiveNectar()
    {
        nectar += 1;
    }

    private void MakeBees(int beeAmt)
    {
        // Find all the flowers and store them in an array to pass to each Bee in their Init function
        GameObject[] flowerArray = GameObject.FindGameObjectsWithTag("Flower");

        Transform canvas = GameObject.Find("Canvas").transform;

        // Instantiate bees up to the assigned beeAmount, parent them to the hive, and call their Init function so they know which hive created them
        for (int i = 0; i < beeAmt; i++)
        {
            GameObject bee = Instantiate(beePrefab, canvas);
            bee.transform.position = transform.position;
            bee.GetComponent<Bee>().Init(gameObject, flowerArray);
        }
    }
}
