using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private TextMeshProUGUI[] textArray = new TextMeshProUGUI[3];


    // Start is called before the first frame update
    void Start()
    {
        spriteArray = new Sprite[3] {sprite1, sprite2, sprite3};

        textArray = GetComponentsInChildren<TextMeshProUGUI>();

        hiveSprite = GetComponent<Image>();

        MakeBees(totalBees);

        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        // Increment the honey timer if the hive has nectar and if the timer variable is less than the rate. 
        // Once the timer reaches the rate variable, increment the honey, decrement the nectar used, and reset the timer
        if (nectar > 0 && honeyTimer < honeyRate && honey < 9)
        {
            honeyTimer += 1;
        } else if (honeyTimer >= honeyRate)
        {
            honey += 1;
            nectar -= 1;
            honeyTimer = 0f;

            UpdateText();

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
            UpdateText();
        }
        
    }

    public void ReceiveNectar()
    {
        nectar += 1;
        UpdateText();
    }

    private void MakeBees(int beeAmt)
    {
        Transform canvas = GameObject.Find("BeeParent").transform;

        // Instantiate bees up to the assigned beeAmount, parent them to the hive, and call their Init function so they know which hive created them
        for (int i = 0; i < beeAmt; i++)
        {
            GameObject bee = Instantiate(beePrefab, canvas);
            Vector3 offset = bee.GetComponent<Bee>().Init(gameObject);
            bee.transform.position = transform.position + offset;
        }
    }

    private void UpdateText()
    {
        textArray[0].text = "Nectar: " + nectar.ToString();
        textArray[1].text = "Honey: " + honey.ToString();
        textArray[2].text = "Bees: " + totalBees.ToString();
    }
}
