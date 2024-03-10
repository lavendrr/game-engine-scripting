using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int keys = 0;
    [SerializeField]
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

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Door")
        {
            if (Input.GetKey("k")) // Note: this is calling a bunch of times which doesn't currently seem to be affecting performance, but might be worth fixing later
            {
                if (other.gameObject.GetComponent<Door>().OpenDoor(keys))
                {
                    keys--;
                    keysText.text = "Keys: " + keys.ToString();
                }
            }
        }
    }
}
