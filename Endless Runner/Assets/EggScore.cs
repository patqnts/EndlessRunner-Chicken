using UnityEngine;
using UnityEngine.UI;

public class EggScore : MonoBehaviour
{
    [SerializeField]
    public Text eggScoreText; // Text component to display the current egg score
    [SerializeField]
    public Text highScoreText; // Text component to display the high score

    public int eggScore = 0; // Current egg score
    private int highScore = 0; // High score

    private void Start()
    {
        // Load the high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreText();
    }

    public void CollectEgg()
    {
        eggScore++;
        UpdateEggScoreText();

        // Check if the current score is higher than the high score
        if (eggScore > highScore)
        {
            highScore = eggScore;
            UpdateHighScoreText();

            // Save the new high score to PlayerPrefs
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

    private void UpdateEggScoreText()
    {
        eggScoreText.text = "Egg Score: " + eggScore;
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = "High Score: " + highScore;
    }
}
