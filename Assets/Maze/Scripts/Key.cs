using UnityEngine;

public class Key : MonoBehaviour
{
    void OnEnable()
    {
        Player.GameRestart += ResetKey;
    }

    void OnDestroy()
    {
        Player.GameRestart -= ResetKey;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // When a collision happens, make sure it's with the player
        if (other.gameObject.name == "Player2D")
        {
            // Give the player a key and disable this key
            other.gameObject.GetComponent<Player>().GiveKey();
            gameObject.SetActive(false);
        }
    }

    private void ResetKey()
    {
        gameObject.SetActive(true);
    }
}
