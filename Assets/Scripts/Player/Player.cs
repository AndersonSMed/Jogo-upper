using System.Collections;
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
    // Stores the horizontal max speed of the player
    private float maxSpeed = 0f;
    [SerializeField]
    // Stores the fireball prefab
    private GameObject fireBall;
    [SerializeField]
    // Stores the position to spawn fireball
    private GameObject fireBallSpawn;
    [SerializeField]
    // Stores the fire cooldown
    private float cooldown = 2f;
    [SerializeField]
    // Stores the fireball audioclip
    private AudioClip fireBallClip;

    // Stores the Player's RigidBody2D component that will be used to apply forces to our player
    private Rigidbody2D rb;
    // Stores the animator component of the player
    private Animator anim;
    // Stores if the player is walking or jumping
    private bool walking, jumping;
    // Stores if the player is on air
    private bool onAir = false;
    // Or if the player is on ground
    private bool onGround = true;
    // If the player is walking, get the direction of the movement and stores here
    private float horizontalMovement;
    // Stores if the player is firing a ball
    private bool isFiring = false;

	// When the component start call this method
	private void Start () {
        // And get component Rigidbody2d from the GameObject and stores it
        rb = GetComponent<Rigidbody2D>();
        // Gets the animator component and stores it
        anim = GetComponent<Animator>();
	}

    // This function is called once per frame
    private void Update () {
        //Stop the animation if the game is paused
        anim.speed = (GameManager.Instance.IsGamePaused()) ? 0f : 1f;
        // Verify if the game is paused
        if (!GameManager.Instance.IsGamePaused()) {
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
                // Set player walking animation to true
                anim.SetBool("Walking", true);
                // Verifies if the player is walking left
                if (horizontalMovement < 0f) {
                    // Then check if the rotation in the Y axis is different from 180
                    if (transform.rotation.y != 180f)
                        // If it's different, rotate the player
                        transform.rotation = new Quaternion(0f, 180f, 0f, transform.rotation.w);
                } else {
                    // Do the same thing here, but in this case we check if the player is walking to the right
                    // And if the rotation is equals to 0
                    if (transform.rotation.y != 0f)
                        // If it's different, rotate the player
                        transform.rotation = new Quaternion(0f, 0f, 0f, transform.rotation.w);
                }
            } else {
                // Otherwise the player is not walking, so stores that to our variable
                walking = false;
                // Set player walking animation to false
                anim.SetBool("Walking", false);
            }
            // If the player's not pressing the jump button and he's on the ground, then the player isn't in the air
            if (onGround && !jumping) {
                onAir = false;
            }
            // If press the button E, then release the fireball
            if (Input.GetAxis("Fire1") > 0f && !isFiring) {
                // Starts the attacking animation
                anim.SetBool("Attacking", true);
                //Set isFiring to true
                isFiring = true;
                // Call the method FireBall when the animation of attacking is ended
                Invoke("FireBall", 0.5f);
            } else {
                // The player is not attacking
                anim.SetBool("Attacking", false);
            }
        }
    }

    private void FireBall() {
        // Instantiate a new fireball
        GameObject ball = Instantiate(fireBall);
        // Set it's position to our spawn
        ball.transform.position = new Vector2(fireBallSpawn.transform.position.x, fireBallSpawn.transform.position.y);
        // If the player is facing right, then call the FireRight method from the fireball script
        if (transform.rotation.y == 0f)
            ball.GetComponent<Fireball>().FireRight();
        // Otherwise, call the method FireLeft from the fireball script
        else
            ball.GetComponent<Fireball>().FireLeft();
        //Calls the SoundManager to play the Fireball AudioClip
        SoundManager.Instance.PlaySFX(fireBallClip);
        // Calls the method to reset cooldown in x seconds
        Invoke("CoolDownFire", cooldown);
    }

    // This method is called everytime the player use a fireball
    private void CoolDownFire() {
        isFiring = false;
    }

    // This function is called a fixed number of times per frames
    private void FixedUpdate() {
        // Verify if the game is paused
        if (!GameManager.Instance.IsGamePaused()) {
            // Here we verify if the player is walking and his velocity in the X axis is less than our max speed
            if (walking && rb.velocity.x < maxSpeed) {
                // If that is true, than add some force to our player, to create movement
                rb.AddForce(Vector2.right * horizontalMovement * walkingAcceleration);
            }
            else if (!walking) {
                // If the player isn't already stopped
                if (rb.velocity.x != 0)
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
