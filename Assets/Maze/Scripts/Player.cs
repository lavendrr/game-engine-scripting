using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    private int keys = 0;
    private int health = 10;
    [SerializeField]
    private TextMeshProUGUI keysText, healthText;

    public int GetKeys()
    {
        return keys;
    }

    public void GiveKey()
    {
        keys++;
        keysText.text = "Keys: " + keys.ToString();
        Debug.Log("Got a key! Now I have: " + keys.ToString());
    }

    public void Damage(int amt)
    {
        health -= amt;
        healthText.text = "Health: " + health.ToString();
        Debug.Log("Took " + amt.ToString() + " damage");
    }
}
