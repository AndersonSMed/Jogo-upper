using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    [SerializeField]
    // Sets the speed of the fireball
    private float speed = 2f;
    [SerializeField]
    // Sets the damage the fireball will cause
    private float damage = 2f;

    // Stores the Rigidbody2D component
    private Rigidbody2D rb;
    // Stores the SpriteRenderer component that will be used to flip the sprite
    private SpriteRenderer sr;
    // Stores the bullet's velocity if the game is paused
    private Vector2 lastSpeed;

    // When the component is created, call this method
    private void Awake() {
        // Get the components and store in our attributes
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Update() {
        // Check if the game is paused
        if (GameManager.Instance.IsGamePaused()) {
            // If it paused and the bullet is moving
            if (rb.velocity != Vector2.zero) {
                // Stores the last velocity
                lastSpeed = rb.velocity;
                // And reset it to zero
                rb.velocity = Vector2.zero;
            }
            // If the game was resumed
        } else {
            // And the actual velocity is 0
            if(rb.velocity == Vector2.zero) {
                // Then load the backup velocity
                rb.velocity = lastSpeed;
            }
        }
    }

    // Method called when we need to fire to the right
    public void FireRight() {
        // Here we add a impulse force to our object pointing to the right
        rb.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
    }

    // Method called when we need to fire to the left
    public void FireLeft() {
        // We first need to flip the object's sprite
        sr.flipX = true;
        // Then we add a impulse force to our object pointing to the left
        rb.AddForce(Vector2.left * speed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // If any collision with the scenario happens, destroy this object
        if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Floor")) {
            Destroy(gameObject);
        }
        // Verifies if the collision ocurred with one enemy
        if (collision.gameObject.CompareTag("Enemy")) {
            // Deal damage to that enemy
            collision.gameObject.GetComponent<BasicEnemy>().DealDamage(damage);
            Destroy(gameObject);
        }
    }
}
