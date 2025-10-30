using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Apple : MonoBehaviour
{
    // Variables
    public GameObject applePrefab;
    private SpriteRenderer sr;
    public float speed = 3f;
    public float minSpeed = 3f;
    public float maxSpeed = 7f;
    public float minY = -4f;
    public float maxY = 4f;
    private Vector3 direction = Vector3.right;
    private static float screenLeft, screenRight;
    public float scaleIncreaseAmount = 0.2f; 
    public float initialGrowthDelay = 2.0f;
    public float growthRepeatRate = 1.0f;
    public float maxScale = 5.0f;
    public TextMeshProUGUI scoreText;
    public static int totalScore = 0; 
    public int appleValue = 5;
    public static int score = 0;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        Camera cam = Camera.main;
        if (cam == null) return; 

        float camDistance = Mathf.Abs(cam.transform.position.z - transform.position.z);
        Vector3 rightEdge = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, camDistance));
        Vector3 leftEdge = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, camDistance));

        screenRight = rightEdge.x;
        screenLeft = leftEdge.x;

        UpdateScoreDisplay();

        InvokeRepeating(nameof(Grow), initialGrowthDelay, growthRepeatRate);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        //Check right bound
        if (transform.position.x > screenRight)
        {
            direction = Vector3.left;
            if (sr != null) sr.flipX = true;
            RandomizeMovement();
        }
        //Check right bound
        else if (transform.position.x < screenLeft)
        {
            direction = Vector3.right;
            if (sr != null) sr.flipX = false;
            RandomizeMovement();
        }
    }
    
    //Ate apple 
    public void AteApple()
    {
        //Stop grow
        CancelInvoke(nameof(Grow));

        //Update score
        Apple.totalScore += appleValue;
        UpdateScoreDisplay();
        
        //Disable components before destroying
        if (sr != null) sr.enabled = false;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        
        //Destory current apple
        Destroy(gameObject, 0.1f); 

        //Transition to next scene
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        //Check if next scene is valid
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            Debug.Log("Scene: " + nextSceneIndex);
        }
        else
        {
            Debug.Log("End game");
        }
    }

    //Movement horizontally randomly
    void RandomizeMovement()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        float newY = Random.Range(minY, maxY);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    //Grow of the apple
    void Grow()
    {
        //If growth is less than maxScale
        if (transform.localScale.x < maxScale)
        {
            transform.localScale += new Vector3(scaleIncreaseAmount, scaleIncreaseAmount, scaleIncreaseAmount);
            
            //Update score
            appleValue = Mathf.Max(0, appleValue - 1);
            UpdateScoreDisplay();
        }
        else
        {
            CancelInvoke(nameof(Grow));
            Debug.Log("Apple reached max size and stopped growing.");
            FailDisappear();
        }
    }

    //Displace updated score
    void UpdateScoreDisplay()
    {
        if (Score.instance != null)
        {
            Score.instance.UpdateScoreDisplay(); 
        }
    }

    //Balloon disappears
    void FailDisappear()
    {
        CancelInvoke(nameof(Grow));
        
        //Disable components before destroying
        if (sr != null) sr.enabled = false;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        
        //Destroy current apple
        Destroy(gameObject, 0.1f); 
        
        //Restart level
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        Debug.Log("Scene Restarted");
    }
}