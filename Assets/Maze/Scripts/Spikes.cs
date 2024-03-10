using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField]
    private int damageAmount = 2;
    private void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<Player>().Damage(damageAmount);
        //gameObject.SetActive(false);
    }
}
