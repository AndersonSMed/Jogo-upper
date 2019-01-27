﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // SerializeField is used to create encapsulation of the attributes
    // But also let the attributes to be modified from the Unity's inspector window
    [SerializeField]
    // Stores the walking acceleration of the player
    private float walkingAcceleration = 0f;
    [SerializeField]
    // Stores the jump force of the player
    private float jumpForce = 0f;
    [SerializeField]
    // Stored the horizontal max speed of the player
    private float maxSpeed = 0f;

    // Stores the Player's RigidBody2D component that will be used to apply forces to our player
    private Rigidbody2D rb;
    // Stores if the player is walking or jumping
    private bool walking, jumping;
    // Stores if the player is on air
    private bool onAir = false;
    // Or if the player is on ground
    private bool onGround = true;
    // If the player is walking, get the direction of the movement and stores here
    private float horizontalMovement;

	// When the component start call this method
	private void Start () {
        // And get component Rigidbody2d from the GameObject and stores it
        rb = GetComponent<Rigidbody2D>();
	}

    // This function is called once per frame
    private void Update () {
        // Inputs are used to create multiple forms to control the game
        // Get if the button for jump was pressed and store that information
        // The input values range between [0,1], for this case
        if (Input.GetAxis("Jump") != 0f) {
            jumping = true;
        } else {
            jumping = false;
        }
        // Here the input range between [-1,1], -1 means that the player is pressing left and 1 means right
        // 0 means that the player is not walking
        // Stores that info in our variable horizontalMovement
        horizontalMovement = Input.GetAxis("Horizontal");
        if (horizontalMovement != 0f) {
            // Stores if the player is walking
            walking = true;
        } else {
            // Otherwise the player is not walking, so stores that to our variable
            walking = false;
        }
        // If the player's not pressing the jump button and he's on the ground, then the player isn't in the air
        if(onGround && !jumping) {
            onAir = false;
        }
    }

    // This function is called a fixed number of times per frames
    private void FixedUpdate() {
        // Here we verify if the player is walking and his velocity in the X axis is less than our max speed
        if (walking && rb.velocity.x < maxSpeed) {
            // If that is true, than add some force to our player, to create movement
            rb.AddForce(Vector2.right * horizontalMovement * walkingAcceleration);
        }else if (!walking) {
            // If the player is not walking, then reset his velocity on the axis X
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        // If the player wants to jump, and his isn't on air
        if (jumping && !onAir) {
            // Apply some Impulse in the axis Y
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            // And now his is on air
            onAir = true;
        }
    }

    // This method is called everytime our player collides with something
    private void OnCollisionEnter2D(Collision2D collision) {
        // If the collision's gameobject tag is equals to "Floor", then the player is on ground
        if (collision.gameObject.CompareTag("Floor")) {
            onGround = true;
        }
    }

    // This method is called everytime our player is colliding with something and then exits it
    private void OnCollisionExit2D(Collision2D collision) {
        // If the collision's gameobject tag is equals to "Floor", then the player is not on ground
        if (collision.gameObject.CompareTag("Floor")) {
            onGround = false;
        }
    }
}