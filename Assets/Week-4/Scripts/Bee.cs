using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = System.Random;

public class Bee : MonoBehaviour
{

    private GameObject hive;
    private GameObject[] flowers;
    private bool inProgress;
    private Random random;

    public void Init(GameObject parentHive, GameObject[] flowerArray)
    {
        hive = parentHive;
        flowers = flowerArray;
        inProgress = false;
        random = new Random();
    }

    // Update is called once per frame
    void Update()
    {
        if (inProgress == false)
        {
            CheckAnyFlower();
        }
    }

    public void CheckAnyFlower()
    {
        inProgress = true;

        GameObject targetFlower = flowers[random.Next(0, flowers.Length)];

        Flowers flowerScript = targetFlower.GetComponent<Flowers>();

        if (flowerScript.GetNectarStatus() == true && flowerScript.GetTargetStatus() == false)
        {
            flowerScript.SetTarget();

            transform.DOMove(targetFlower.transform.position, 2f).OnComplete(() =>
            {
                flowerScript.GiveNectar();

                transform.DOMove(hive.transform.position, 1f).OnComplete(() =>
                {
                    hive.GetComponent<Hive>().ReceiveNectar();
                    inProgress = false;
                });
            });
        } else 
        {
            inProgress = false;
        }
    }
}
