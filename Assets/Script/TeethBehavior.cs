using UnityEngine;
using UnityEngine.InputSystem;

public class TeethBehavior : MonoBehaviour
{
    // Variables
    public Transform shootingPoint;
    public GameObject firePrefab;
    public float fireSpeed = 10f;

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;
        float horizontalInput = 0f;
        
        //Flip direction of shooting point based on direciton of the character
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            horizontalInput += 1f;
        }
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            horizontalInput -= 1f;
        }
        if (Mathf.Abs(horizontalInput) > 0)
        {
            Quaternion rightRotation = Quaternion.Euler(0, 0, 0);
            Quaternion leftRotation = Quaternion.Euler(0, 0, 180);

            if (horizontalInput > 0)
            {
                shootingPoint.rotation = rightRotation;
            }
            else
            {
                shootingPoint.rotation = leftRotation;
            }
        }
        
        //Create the firing teeth
        if (Keyboard.current.spaceKey.wasPressedThisFrame){
            Instantiate(firePrefab, shootingPoint.position, shootingPoint.rotation);
        }
    }

    //If hit apple, destroy apple
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Teeth hit");
        Destroy(gameObject);
    }
}