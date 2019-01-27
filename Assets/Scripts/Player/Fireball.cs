using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    [SerializeField]
    // Sets the speed of the fireball
    private float speed = 2f;

    // Stores the Rigidbody2D component
    private Rigidbody2D rb;
    // Stores the SpriteRenderer component that will be used to flip the sprite
    private SpriteRenderer sr;


    // When the component is created, call this method
    private void Awake() {
        // Get the components and store in our attributes
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
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
    }
}
