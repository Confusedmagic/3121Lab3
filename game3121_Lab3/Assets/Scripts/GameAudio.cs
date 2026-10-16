
using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public enum ObjectType
    {
        Player,
        Obstacle
    }

    [Header("Object Type")]
    public ObjectType objectType;

    [Header("Sound Effects")]
    public AudioClip wallHitSound;
    public AudioClip obstacleHitSound;
    public AudioClip deathSound;
    public AudioClip backgroundMusic;
    public AudioClip beepSound;

    [Header("Beeping Settings")]
    public float beepDistance = 6f;
    public float dangerDistance = 1f;
    public float slowBeepSpeed = 1f;
    public float fastBeepSpeed = 0.15f;

    private AudioSource musicSource;
    private AudioSource beepSource;
    private float beepTimer;
    private bool isGameOver = false;
    void Start()
    {
        if (objectType == ObjectType.Player)
        {
            // Background music
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            musicSource.spatialBlend = 0f;
            musicSource.volume = 0.5f;

            if (backgroundMusic != null)
            {
                musicSource.Play();
            }

            // Beeping sound
            beepSource = gameObject.AddComponent<AudioSource>();
            beepSource.playOnAwake = false;
            beepSource.spatialBlend = 0f;
        }
    }

    void Update()
    {
        if (objectType != ObjectType.Player)
            return;

        if (beepSound == null)
            return;

        if (isGameOver)
            return;

        // Find the closest obstacle
        GameObject[] obstacles =
            GameObject.FindGameObjectsWithTag("Obstacle");

        float closestDistance = Mathf.Infinity;

        foreach (GameObject obstacle in obstacles)
        {
            float distance = Vector2.Distance(
                transform.position,
                obstacle.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
            }
        }

        // Stop beeping when obstacles are far away
        if (closestDistance > beepDistance)
        {
            beepTimer = 0f;
            return;
        }

        float danger = Mathf.InverseLerp(
            beepDistance,
            dangerDistance,
            closestDistance
        );

        beepTimer -= Time.deltaTime;

        if (beepTimer <= 0f)
        {
            // Higher pitch when closer
            beepSource.pitch = Mathf.Lerp(1f, 2f, danger);
            beepSource.PlayOneShot(beepSound);

            // Faster beeping when closer
            beepTimer = Mathf.Lerp(
                slowBeepSpeed,
                fastBeepSpeed,
                danger
            );
        }

        

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (objectType == ObjectType.Player)
        {
            // Player hits wall
            if (collision.gameObject.CompareTag("Wall"))
            {
                PlaySound(wallHitSound);
            }

            // Player hits obstacle
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                PlaySound(deathSound);
            }
        }

        if (objectType == ObjectType.Obstacle)
        {
            // Two obstacles collide
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                // Prevent playing the sound twice
                if (gameObject.GetInstanceID() <
                    collision.gameObject.GetInstanceID())
                {
                    PlaySound(obstacleHitSound);
                }
            }
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip == null)
            return;

        Vector3 position = transform.position;

        if (Camera.main != null)
        {
            position = Camera.main.transform.position;
        }

        AudioSource.PlayClipAtPoint(clip, position);
    }


    public void StopBeeping()
    {
        isGameOver = true;

        beepTimer = 0f;

        if (beepSource != null)
        {
            beepSource.Stop();
            beepSource.enabled = false;
        }
    }



}
