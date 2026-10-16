
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text finalScoreText;

    [Header("Score Settings")]
    public float pointsPerSecond = 1f;

    private float survivalTime = 0f;
    private int score = 0;
    private int highScore = 0;
    private bool gameOver = false;

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateUI();
    }

    void Update()
    {
        if (gameOver)
            return;

        // Increase score while player is alive
        survivalTime += Time.deltaTime;

        score = Mathf.FloorToInt(
            survivalTime * pointsPerSecond
        );

        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (highScoreText != null)
            highScoreText.text = "High Score: " + highScore;
    }

    public void EndRun()
    {
        if (gameOver)
            return;

        gameOver = true;

        // Save the highest score
        if (score > highScore)
        {
            highScore = score;

            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        UpdateUI();

        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + score;
    }
}
