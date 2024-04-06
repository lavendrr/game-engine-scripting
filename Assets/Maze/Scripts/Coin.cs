using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnEnable()
    {
        Player.GameRestart += ResetCoin;
    }

    void OnDestroy()
    {
        Player.GameRestart -= ResetCoin;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // When a collision happens, make sure it's with the player
        if (other.gameObject.name == "Player2D")
        {
            // Give the player a coin and disable this coin
            other.gameObject.GetComponent<Player>().GiveCoin();
            gameObject.SetActive(false);
        }
    }

    private void ResetCoin()
    {
        gameObject.SetActive(true);
    }
}
