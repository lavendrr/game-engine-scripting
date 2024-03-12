using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.GetComponent<Player>().GiveKey();
        gameObject.SetActive(false);   
    }
}
