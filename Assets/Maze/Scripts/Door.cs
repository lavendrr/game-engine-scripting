using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isOpen = false;
    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Returns true if the door successfully opens, returns false if the player doesn't have a key or if the door is already open
    public bool OpenDoor(int keys)
    {
        // Makes sure the door isn't already open
        if (isOpen == false)
        {
            // Makes sure the player has a key to use on the door
            if (keys > 0)
            {
                // Disables the door's collider to let the player through, marks itself as open, and triggers the door's open animation
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                isOpen = true;
                animator.SetTrigger("OpenDoor");
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
