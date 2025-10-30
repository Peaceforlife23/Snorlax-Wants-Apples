using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Snorlax : MonoBehaviour
{
    //Variables
    public float speed = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private AudioSource audioSource;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        moveInput = Vector2.zero;
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        //Plays sound effect when space bar is pressed
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            if (audioSource != null)
            {
                Debug.Log("Munch Munch");
                audioSource.Play();
            }
        }

        //Keys correspond to movement
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
            moveInput.y += 1; 
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
            moveInput.y -= 1; 
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            moveInput.x -= 1; 
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            moveInput.x += 1; 

        //Prevent faster diagonal movement
        moveInput = moveInput.normalized;

        //Flip the Snorlax when moving left/right for a visual effect
        if (moveInput.x > 0.01f) sr.flipX = true;
        else if (moveInput.x < -0.01f) sr.flipX = false;
    }

    void FixedUpdate()
    {
        Vector2 newPos = rb.position + moveInput * speed * Time.fixedDeltaTime;

        Camera cam = Camera.main;
        if (cam != null)
        {
            float vertExtent = cam.orthographicSize;
            float horzExtent = vertExtent * cam.aspect;       
            
            float leftBound = -horzExtent;
            float rightBound = horzExtent;
            float bottomBound = -vertExtent;
            float topBound = vertExtent;

            newPos.x = Mathf.Clamp(newPos.x, leftBound, rightBound);
            newPos.y = Mathf.Clamp(newPos.y, bottomBound, topBound);
        }

        rb.MovePosition(newPos);
    }
}
