// Original script from https://stuartspixelgames.com/2018/06/24/simple-2d-top-down-movement-unity-c/
// Modified by me to include sprite walk animations

using UnityEngine;
public class TwoDController : MonoBehaviour
{
    Rigidbody2D body;
    float horizontal;
    float vertical;
    float moveLimiter = 0.7f; // Used to limit speed when moving diagonally (70% by default)
    public float runSpeed = 20.0f;

    public Sprite[] walkAnim;
    private bool isWalking = false;
    private int currentSpriteIndex = 0;
    private SpriteRenderer spriteRenderer;


    void Start ()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Load sprites
        walkAnim = new Sprite[4];
        walkAnim[0] = Resources.Load<Sprite>("walkA");
        walkAnim[1] = Resources.Load<Sprite>("walkB");
        walkAnim[2] = Resources.Load<Sprite>("walkA");
        walkAnim[3] = Resources.Load<Sprite>("walkC");

        // Call updateAnimState every 0.25 seconds
        InvokeRepeating("updateAnimState", 0.25f, 0.25f);
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left, 1 is right
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down, 1 is up
        isWalking = horizontal != 0 || vertical != 0;

        // Update the sprite direction based on the direction the player is facing
        if (horizontal > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontal < 0)
        {
            spriteRenderer.flipX = false;
        }

    }

    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // Limit movement speed diagonally using the moveLimiter as set above
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        } 

        // Add the velocity to the RigidBody
        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }

    void updateAnimState()
    {
        if (isWalking)
        {
            // Cycle through the sprites
            currentSpriteIndex = (currentSpriteIndex + 1) % 4;
        }
        else
        {
            // Set the sprite to standing idle
            currentSpriteIndex = 0;
        }
        
        spriteRenderer.sprite = walkAnim[currentSpriteIndex];
    }
}