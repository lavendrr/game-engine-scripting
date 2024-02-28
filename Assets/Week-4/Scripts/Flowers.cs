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

    void Start()
    {
        random = new Random();
        nectarRate += random.Next(-250, 500);
        flowerSprite = GetComponent<Image>();
        Debug.Log(nectarRate);
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
            Debug.Log("Nectar ready!");
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
            Debug.Log("Nectar taken!");
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
