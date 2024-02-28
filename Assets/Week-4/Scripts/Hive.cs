using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : MonoBehaviour
{

    private float honeyRate = 15f;
    private int beeAmount = 2;
    [SerializeField]
    private GameObject beePrefab;
    private int nectar = 0;
    private int honey = 0;
    private float honeyTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Find all the flowers and store them in an array to pass to each Bee in their Init function
        GameObject[] flowerArray = GameObject.FindGameObjectsWithTag("Flower");

        Transform canvas = GameObject.Find("Canvas").transform;

        // Instantiate bees up to the assigned beeAmount, parent them to the hive, and call their Init function so they know which hive created them
        for (int i = 0; i < beeAmount; i++)
        {
            GameObject bee = Instantiate(beePrefab, canvas);
            bee.transform.position = transform.position;
            bee.GetComponent<Bee>().Init(gameObject, flowerArray);
        }
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
        }
        
    }

    public void ReceiveNectar()
    {
        nectar += 1;
    }
}
