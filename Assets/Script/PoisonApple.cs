using UnityEngine;

public class PoisonApple : MonoBehaviour
{
    //Variables
    public GameObject poisonApplePrefab;
    public int scorePenalty = 2; 
    
    public float minSpeed = 3f;
    public float maxSpeed = 7f;
    public float minY = -4f;
    public float maxY = 4f;

    private float speed;
    private SpriteRenderer sr; 
    private Vector3 direction = Vector3.right;
    private static float screenLeft, screenRight, screenTop, screenBottom;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        Camera cam = Camera.main;
        if (cam == null) return; 

        float camDistance = Mathf.Abs(cam.transform.position.z - transform.position.z);
        Vector3 rightEdge = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, camDistance));
        Vector3 leftEdge = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, camDistance));
        Vector3 topEdge = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, camDistance));
        Vector3 bottomEdge = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, camDistance));

        screenRight = rightEdge.x;
        screenLeft = leftEdge.x;
        screenTop = topEdge.y;
        screenBottom = bottomEdge.y;

        InitializeMovement();
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        bool bounced = false;
        
        //Reverse direction horizontally
        if (transform.position.x > screenRight && direction.x > 0)
        {
            direction.x *= -1;
            bounced = true;
        }
        else if (transform.position.x < screenLeft && direction.x < 0)
        {
            direction.x *= -1;
            bounced = true;
        }
        
        //Reverse direction vertically
        if (transform.position.y > screenTop && direction.y > 0)
        {
            direction.y *= -1; 
            bounced = true;
        }
        else if (transform.position.y < screenBottom && direction.y < 0)
        {
            direction.y *= -1;
            bounced = true;
        }
        
        if (bounced)
        {
             if (sr != null) sr.flipX = direction.x < 0;
             RandomizeMovement();
        }
    }

    //Initialize movement
    void InitializeMovement()
    {
        //Set a random speed
        speed = Random.Range(minSpeed, maxSpeed);

        //Random normalized direction vector
        direction = Random.insideUnitCircle.normalized;

        //Random starting edge (0=Left, 1=Right, 2=Top, 3=Bottom)
        int edge = Random.Range(0, 4);
        float x = 0, y = 0;

        //Set position based on the chosen edge and character direciton
        //Left 
        if (edge == 0)
        {
            x = screenLeft;
            y = Random.Range(screenBottom, screenTop);
            if (direction.x < 0) direction.x *= -1;
        }
        //Right
        else if (edge == 1)
        {
            x = screenRight;
            y = Random.Range(screenBottom, screenTop);
            if (direction.x > 0) direction.x *= -1;
        }
        //Top
        else if (edge == 2)
        {
            x = Random.Range(screenLeft, screenRight);
            y = screenTop;
            if (direction.y > 0) direction.y *= -1;
        }
        //Bottom
        else
        {
            x = Random.Range(screenLeft, screenRight);
            y = screenBottom;
            if (direction.y < 0) direction.y *= -1;
        }

        transform.position = new Vector3(x, y, transform.position.z);

        //Direciton flip
        if (sr != null) sr.flipX = direction.x < 0;
    }
    
    //Random speed
    void RandomizeMovement()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }
    
    //When hit by teeth
    public void HitByProjectile()
    {
        //Create new poison apple
        CreateNewPoisonApple(poisonApplePrefab);
        //Destroy previous poison apple
        Destroy(gameObject); 
        
        //Update score
        Apple.totalScore -= scorePenalty;
        if (Apple.totalScore < 0)
        {
            Apple.totalScore = 0;
        }
        Score.instance.UpdateScoreDisplay();
    }

    //Poison apple creation
    public static void CreateNewPoisonApple(GameObject prefab)
    {
        Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }
}