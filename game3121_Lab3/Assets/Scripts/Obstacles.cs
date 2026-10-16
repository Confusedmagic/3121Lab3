
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float lifeTime = 12f;

    public GameObject collisionEffect;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;

        rb.collisionDetectionMode =
            CollisionDetectionMode2D.Continuous;

        Vector2 direction = Random.insideUnitCircle.normalized;

        if (direction == Vector2.zero)
            direction = Vector2.right;

        rb.linearVelocity = direction * moveSpeed;

        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (gameObject.GetInstanceID() <
                collision.gameObject.GetInstanceID())
            {
                if (collisionEffect != null)
                {
                    Instantiate(
                        collisionEffect,
                        collision.GetContact(0).point,
                        Quaternion.identity
                    );
                }
            }
        }
    }
}
