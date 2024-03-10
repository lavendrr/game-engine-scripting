using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    private bool isOpen = false;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite openSprite;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool OpenDoor(int keys)
    {
        if (isOpen == false)
        {
            if (keys > 0)
            {
                Debug.Log("Opened door");
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                isOpen = true;
                spriteRenderer.sprite = openSprite;
                return true;
            }
            else
            {
                Debug.Log("Failed to open door");
                return false;
            }
        }
        else
        {
            Debug.Log("Door is already open");
            return false;
        }
    }
}
