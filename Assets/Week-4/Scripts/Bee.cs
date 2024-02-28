using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = System.Random;

public class Bee : MonoBehaviour
{

    private GameObject hive;
    private GameObject[] flowerArray;
    private bool inProgress;
    private Image beeSprite;
    [SerializeField]
    private Sprite beeNectar, beeEmpty;
    private Random random;
    private Vector3 offset;

    public Vector3 Init(GameObject parentHive)
    {
        hive = parentHive;
        inProgress = false;
        beeSprite = GetComponent<Image>();
        random = new Random();
        offset = new Vector3((float)random.Next(-100, 100), (float)random.Next(-100, 100), 0f);
        return offset;
    }

    void Update()
    {
        if (inProgress == false)
        {
            flowerArray = GameObject.FindGameObjectsWithTag("Flower");
            CheckAnyFlower();
        }
    }

    public void CheckAnyFlower()
    {
        inProgress = true;

        GameObject targetFlower = flowerArray[random.Next(0, flowerArray.Length)];

        Flowers flowerScript = targetFlower.GetComponent<Flowers>();

        if (flowerScript.GetNectarStatus() == true && flowerScript.GetTargetStatus() == false)
        {
            flowerScript.SetTarget();

            transform.DOMove(targetFlower.transform.position, 2f).OnComplete(() =>
            {
                flowerScript.GiveNectar();
                beeSprite.sprite = beeNectar;

                transform.DOMove(hive.transform.position + offset, 2f).OnComplete(() =>
                {
                    hive.GetComponent<Hive>().ReceiveNectar();
                    beeSprite.sprite = beeEmpty;
                    inProgress = false;
                });
            });
        } else 
        {
            inProgress = false;
        }
    }
}
