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
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                isOpen = true;
                spriteRenderer.sprite = openSprite;
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
