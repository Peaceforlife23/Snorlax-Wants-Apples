using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    //Variables
    public static Score instance;
    public TextMeshProUGUI scoreText; 

    private int currentScore 
    {
        get { return Apple.score; }
        set { Apple.score = value; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    //Displace score
    public void UpdateScoreDisplay()
    {
        int currentTotalScore = Apple.totalScore;
        
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentTotalScore.ToString();
        }
    }
}