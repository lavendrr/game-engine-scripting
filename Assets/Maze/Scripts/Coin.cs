using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.GetComponent<Player>().GiveCoin();
        gameObject.SetActive(false);   
    }
}
