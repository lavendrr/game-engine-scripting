using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField]
    private int damageAmount = 1;

    private void OnCollisionEnter2D(Collision2D other)
    {
        // When a collision happens, make sure it's with the player, and damage them
        if (other.gameObject.name == "Player2D")
        {
            other.gameObject.GetComponent<Player>().Damage(damageAmount);
        }
    }
}
