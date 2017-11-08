using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class Player : MovingObject
{
    public int damagePerHit = 1;
    public int healthPerMove = 1;
    public int healthPerFood = 100;
    public int healthPerSoda = 200;

    Text healthText;

    Animator animator;
    int health;

    bool lastMovedHorizontally;     // Used to move the player diagonlly.

    protected override void Start ()
    {
        // Make camera follow above player.
        Vector3 camStartPos = new Vector3(
            transform.position.x, transform.position.y, transform.position.z - 10f);
        Camera.main.transform.position = camStartPos;
        Camera.main.transform.parent = transform;

        // Load the player's health from the GameManager and update the health text UI.
        health = GameManager.instance.playerHealth;
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        healthText.text = "Health: " + health;

        animator = GetComponent<Animator>();

        base.Start();
	}

    // Handle input from the user each frame.
    void Update ()
    {
        // Only accept input if its the player's turn.
        if (!GameManager.instance.playersTurn)
            return;

        // Get user's input.
        int horizontal = (int)Input.GetAxis("Horizontal");
        int vertical = (int)Input.GetAxis("Vertical");

        // If the input is a diagonal vector, alternate between moving verically and horizontally.
        if (horizontal != 0 && vertical != 0)
        {
            if (lastMovedHorizontally)
                horizontal = 0;
            else
                vertical = 0;
        }

        // Attempt to move if ther was input.
        if (horizontal != 0 || vertical != 0)
        {
            // Update lastMovedHorizontally.
            if (horizontal != 0)
                lastMovedHorizontally = true;
            else
                lastMovedHorizontally = false;
            
            AttemptMove<Wall>(horizontal, vertical);
        }
	}

    // Handle collisions with trigger in the scene.
    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Exit":
                ReachedExit();
                break;
            case "Food":
                health += healthPerFood;
                healthText.text = "+" + healthPerFood + " Health: " + health;
                other.gameObject.SetActive(false);
                break;
            case "Soda":
                health += healthPerSoda;
                healthText.text = "+" + healthPerSoda + " Health: " + health;
                other.gameObject.SetActive(false);
                break;
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        health -= healthPerMove;
        healthText.text = "Health: " + health;

        base.AttemptMove<T>(xDir, yDir);

        CheckIfGameOver();

        GameManager.instance.playersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(damagePerHit);
        animator.SetTrigger("playerChop");
    }

    public void LoseHealth(int loss)
    {
        animator.SetTrigger("playerHit");
        health -= loss;
        healthText.text = "-" + loss + " Health: " + health;
        CheckIfGameOver();
    }

    void ReachedExit()
    {
        GameManager.instance.playerHealth = health;
        GameManager.instance.EndLevel();
        enabled = false;
    }

    void CheckIfGameOver()
    {
        if (health <= 0)
            GameManager.instance.GameOver();
    }
}
