using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {

    [SerializeField]
    // Stores the enemy's initial life
    private float initialLife = 4f;
    [SerializeField]
    // Speed the enemy will move
    private float speed = 2f;
    [SerializeField]
    // Time that our enemy will attack the player
    private float timeToAttack = 2f;
    [SerializeField]
    // Damage the enemy will do to our player
    private float damage = 1;

    // Store the actual life
    private float actualLife;
    // Stores the moving direction of our enemy
    private Vector2 movingDirection;
    // Stores the enemy's Rigidbody2D
    private Rigidbody2D rb;
    // If the player is right in our front, set this attribute to true
    private bool playerOnSight = false;

	private void Start () {
        // Sets the enemy actual life to initial life
        actualLife = initialLife;

        // The enemy starts with a 50% chance of walking to the left and 50% of walking to the right
        if (Random.Range(0, 100) > 50)
            movingDirection = Vector2.right;
        else
            movingDirection = Vector2.left;
        rb = GetComponent<Rigidbody2D>();
	}
	
    //This script is called everytime the enemy takes damage
	public void DealDamage(float damage) {
        // Decreases actual life in damage, if actualLife is less then or equals to zero, then destroy the enemy
        actualLife -= damage;
        if (actualLife <= 0f)
            Destroy(gameObject);
    }

    private void Update() {
        // If the game is not paused, verify if the enemy is facing the right direction
        if (!GameManager.Instance.IsGamePaused()) {
            if (movingDirection == Vector2.right && transform.rotation.y != 0f)
                transform.rotation = new Quaternion(0f, 0f, 0f, transform.rotation.w);
            else if (movingDirection == Vector2.left && transform.rotation.y != 180f)
                transform.rotation = new Quaternion(0f, 180f, 0f, transform.rotation.w);
        }
    }

    private void FixedUpdate() {
        // If the game isn't paused, verify if the player is right in front of our enemy
        // If it isn't, sets the enemy to the default behaviour, wich is moving from side to side of the screen
        if (!GameManager.Instance.IsGamePaused()) {
            if (!playerOnSight)
                transform.position += (Vector3)movingDirection * speed * Time.deltaTime;
        }
    }

    // This method is called when the player is in front of our enemy
    private void AttackPlayer() {
        if (playerOnSight) {
            // If the game is paused, wait it to resume to attack the player
            if (GameManager.Instance.IsGamePaused()) {
                AttackPlayer();
                return;
            }
            GameManager.Instance.DamagePlayer(damage);
            Invoke("AttackPlayer", timeToAttack);
        }
    }

    // When our enemy collides with an wall, it's movingDirection changes
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Wall")) {
            movingDirection = (movingDirection == Vector2.right) ? Vector2.left : Vector2.right;
        }
    }

    // If the player is right in front of our enemy, wait x seconds defined in the timeToAttack attribute to attack him
    // And playerOnSight attribute is set to true
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            playerOnSight = true;
            Invoke("AttackPlayer", timeToAttack);
        }
    }

    // When the player isn't in our front, set the playerOnSight attribute to false
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            playerOnSight = false;
        }
    }
}
