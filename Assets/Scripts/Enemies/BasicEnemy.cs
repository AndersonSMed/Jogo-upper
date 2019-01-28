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
    [SerializeField]
    // Stores the sword SFX
    private AudioClip swordSfxClip;
    [SerializeField]
    // Stores the diying SFX
    private AudioClip dyingSfx;

    // Store the actual life
    private float actualLife;
    // Stores the animator component of the player
    private Animator anim;
    // Stores the moving direction of our enemy
    private Vector2 movingDirection;
    // If the player is right in our front, set this attribute to true
    private bool playerOnSight = false;
    // Stores if the enemy is attacking
    private bool attacking = false;
    // Stores the indivdual audio source
    private AudioSource audioSource;

	private void Start () {
        // Sets the enemy actual life to initial life
        actualLife = initialLife;
        // Gets the animator component and stores it
        anim = GetComponent<Animator>();
        // Gets the audio source component and stores it
        audioSource = GetComponent<AudioSource>();
        // The enemy starts with a 50% chance of walking to the left and 50% of walking to the right
        if (Random.Range(0, 100) > 50)
            movingDirection = Vector2.right;
        else
            movingDirection = Vector2.left;
        // Get audio source volume from sound manager
        audioSource.volume = SoundManager.Instance.GetSFXVolume();
	}
	
    //This script is called everytime the enemy takes damage
	public void DealDamage(float damage) {
        // Decreases actual life in damage, if actualLife is less then or equals to zero, then destroy the enemy
        actualLife -= damage;
        if (actualLife <= 0f) {
            SoundManager.Instance.PlayEnemySFX(dyingSfx);
            Destroy(gameObject);
        }
    }

    private void Update() {
        // Get audio source volume from sound manager
        audioSource.volume = SoundManager.Instance.GetSFXVolume();
        //Stop the animation if the game is paused
        anim.speed = (GameManager.Instance.IsGamePaused()) ? 0f : 1f;
        // If the game is not paused, verify if the enemy is facing the right direction
        if (!GameManager.Instance.IsGamePaused()) {
            if (movingDirection == Vector2.right && transform.rotation.y != 0f)
                transform.rotation = new Quaternion(0f, 0f, 0f, transform.rotation.w);
            else if (movingDirection == Vector2.left && transform.rotation.y != 180f)
                transform.rotation = new Quaternion(0f, 180f, 0f, transform.rotation.w);
            if (playerOnSight && !attacking) {
                attacking = true;
                Invoke("AttackPlayer", timeToAttack);
            }
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
                Invoke("AttackPlayer", timeToAttack);
            } else {
                SoundManager.Instance.PlayEnemySFX(swordSfxClip);
                GameManager.Instance.DamagePlayer(damage);
                Invoke("AttackPlayer", timeToAttack);
            }
        } else {
            attacking = false;
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
            anim.SetBool("Attacking", true);
        }
    }

    // When the player isn't in our front, set the playerOnSight attribute to false
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            playerOnSight = false;
            anim.SetBool("Attacking", false);
        }
    }
}
