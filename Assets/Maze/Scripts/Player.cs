using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private int keys = 0;
    private int health = 10;
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
        // Increment keys and update UI to match
        keys++;
        keysText.text = "Keys: " + keys.ToString();
    }

    public void GiveCoin()
    {
        // Increment coins and update UI to match
        coins++;
        coinsText.text = "Coins: " + coins.ToString();
    }

    public void Damage(int amt)
    {
        // Subtract health by certain amount, update UI to match, and end the game if health reaches 0
        health -= amt;
        healthText.text = "Health: " + health.ToString();
        if (health <= 0)
        {
            EndGame(false);
        }
    }

    private void EndGame(bool won)
    {
        // Set text appropriately based on if player won or lost
        if (won)
        {
            endText.text = "You won!\nCoins: " + coins.ToString();
        }
        else
        {
            endText.text = "You lost!\nCoins: " + coins.ToString();
        }

        // Disable the original UI and enable the blur volume and game ending UI
        playerUI.SetActive(false);
        blurVolume.SetActive(true);
        endText.gameObject.SetActive(true);
        // Stop the player from being able to keep moving their character after the game has ended
        GetComponent<TwoDController>().enabled = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Checks if the player is inside a door's trigger box
        if (other.gameObject.name == "Door")
        {
            // Checks if the player is pressing K
            if (Input.GetKey("k"))
            {
                // If the player has a key to use (this check happens in the Door's OpenDoor() method), subtract a key and update the UI to match
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
        // If the player enters the win volume's trigger, end the game
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
