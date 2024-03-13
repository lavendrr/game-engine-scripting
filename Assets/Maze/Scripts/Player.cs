using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int keys = 0;
    [SerializeField]
    private int health = 10;
    [SerializeField]
    private int coins = 0;
    [SerializeField]
    private TextMeshProUGUI keysText, healthText, coinsText, endText;
    [SerializeField]
    private GameObject blurVolume, playerUI;

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

    public void GiveCoin()
    {
        coins++;
        coinsText.text = "Coins: " + coins.ToString();
        Debug.Log("Got a coin! Now I have: " + coins.ToString());
    }

    public void Damage(int amt)
    {
        health -= amt;
        healthText.text = "Health: " + health.ToString();
        Debug.Log("Took " + amt.ToString() + " damage");
        if (health <= 0)
        {
            EndGame(false);
        }
    }

    private void EndGame(bool won)
    {
        if (won)
        {
            endText.text = "You won!\nCoins: " + coins.ToString();
        }
        else
        {
            endText.text = "You lost!\nCoins: " + coins.ToString();
        }

        playerUI.SetActive(false);
        blurVolume.SetActive(true);
        endText.gameObject.SetActive(true);
        GetComponent<TwoDController>().enabled = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Door")
        {
            if (Input.GetKey("k"))
            {
                if (other.gameObject.GetComponent<Door>().OpenDoor(keys))
                {
                    keys--;
                    keysText.text = "Keys: " + keys.ToString();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "WinVolume")
        {
            EndGame(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Maze");
    }
}
