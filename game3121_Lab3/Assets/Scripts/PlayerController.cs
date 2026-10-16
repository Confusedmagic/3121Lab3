
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 30f;
    public GameObject deathEffect;
    public GameObject gameOverUI;
    public ScoreManager scoreManager;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private Vector2 targetPosition;
    private bool isDead = false;

    void Start()
    {
        Time.timeScale = 1f;

        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        targetPosition = rb.position;

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    void Update()
    {
        if (isDead)
        {
            if (Keyboard.current != null &&
                Keyboard.current.rKey.wasPressedThisFrame)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(
                    SceneManager.GetActiveScene().buildIndex
                );
            }

            return;
        }

        if (Mouse.current == null || mainCamera == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector3 screenPosition = new Vector3(
            mousePosition.x,
            mousePosition.y,
            Mathf.Abs(mainCamera.transform.position.z -
                      transform.position.z)
        );

        Vector3 worldPosition =
            mainCamera.ScreenToWorldPoint(screenPosition);

        targetPosition = worldPosition;
    }

    void FixedUpdate()
    {
        if (isDead)
            return;

        Vector2 newPosition = Vector2.MoveTowards(
            rb.position,
            targetPosition,
            moveSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPosition);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead)
            return;

        if (collision.gameObject.CompareTag("Wall") ||
            collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }




    void Die()
    {
        if (isDead)
            return;

        isDead = true;

        // Stop beeping from ALL GameAudio components
        GameAudio[] allAudio = FindObjectsByType<GameAudio>(
            FindObjectsSortMode.None
        );

        foreach (GameAudio audio in allAudio)
        {
            audio.StopBeeping();
        }

        // Stop score and save high score
        if (scoreManager != null)
        {
            scoreManager.EndRun();
        }

        // Play death particle effect
        if (deathEffect != null)
        {
            Instantiate(
                deathEffect,
                transform.position,
                Quaternion.identity
            );
        }

        StartCoroutine(GameOver());
    }





    IEnumerator GameOver()
    {
        // Stop player movement
        rb.simulated = false;

        // Hide the player
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // Show Game Over UI
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // Pause the game
        Time.timeScale = 0f;

        yield break;
    }


    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

}
