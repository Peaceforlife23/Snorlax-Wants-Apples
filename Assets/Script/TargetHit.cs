using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TargetHit : MonoBehaviour
{
    //Variables
    public float speed = 10f;
    private Rigidbody2D fire;

    void Start()
    {
        fire = GetComponent<Rigidbody2D>();
        fire.linearVelocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Apple apple = collision.GetComponent<Apple>();
        PoisonApple poisonApple = collision.GetComponent<PoisonApple>();

        //If hit apple
        if (apple != null)
        {
            apple.AteApple();
            Destroy(gameObject);
            Debug.Log("Apple ate");
        }
        //If hit poison apple
        else if (poisonApple != null) 
        {
            poisonApple.HitByProjectile();
            Destroy(gameObject); 
            Debug.Log("Poison apple ate");
        }
    }
}
