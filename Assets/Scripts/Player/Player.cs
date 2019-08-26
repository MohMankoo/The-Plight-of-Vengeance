using UnityEngine;
using System.Collections;

public class Player : Entity {
    
    // Unity components
    private Rigidbody2D player;
    private Animator playerAnimator;
    public PlayerHealth playerHealthDisplay;  // Game HUD

    public PlayerGun gun;

    // Player stats
    public int maxHealth = 100;
    public int revengeScore = 0;
    public int projectileAttkPwrBonus = 1;

    private void Start() {
        // Initialize properties
        player = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        entityPopup = GetComponent<EntityPopupCreator>();

        // Health
        health = maxHealth;
        playerHealthDisplay.UpdateCurrentHealth(health);
        playerHealthDisplay.UpdateMaxHealth(maxHealth);
        playerAnimator.SetFloat("PlayerHealth", health);

        // Update speed from Entity.cs
        speed = 4f;

        // Set the default gun as Zyka
        gun.UpdateGun("Zyka", projectileAttkPwrBonus);
    }

    // Update is called once per frame
    void Update() {
        if (movementStopped)
            return;

        KillIfHealthDepleted();
        MovePlayer();
        
        // Change direction if mouse is within window
        if (Input.mousePosition.x >= 0 && Input.mousePosition.y >= 0
                && Input.mousePosition.x <= Screen.width && Input.mousePosition.y <= Screen.height) {
            ChangePlayerDirection();
        }

    }

    // Health

    private void KillIfHealthDepleted() {
        // Check if player should be dead
        if (IsDead()) {
            StopMovement(true);
            gun.Jarr(true);

            playerHealthDisplay.UpdateCurrentHealth(health);
            playerAnimator.SetFloat("PlayerHealth", health);
        }
    }

    public new void SetHealth(int health) {
        base.SetHealth(health);
        playerHealthDisplay.UpdateCurrentHealth(health);
        playerAnimator.SetFloat("PlayerHealth", health);
    }

    public void SetMaxHealth(int health) {
        maxHealth = health;
        playerHealthDisplay.UpdateMaxHealth(health);
    }

    public new void DepleteHealth(int hitPoints) {
        base.DepleteHealth(hitPoints);
        playerHealthDisplay.UpdateCurrentHealth(health);
        playerAnimator.SetFloat("PlayerHealth", health);
    }

    public void ReplenishFullHealth() {
        SetHealth(maxHealth);
        playerHealthDisplay.UpdateCurrentHealth(health);
        playerAnimator.SetFloat("PlayerHealth", health);
    }

    // Movement

    private void MovePlayer() {
        // Keyboard movement
        Vector2 userInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 movementMagnitude =
            userInput.normalized * Time.deltaTime * speed;

        // Animate player
        playerAnimator.SetFloat("HorizontalSpeed", Mathf.Abs(movementMagnitude.x));
        playerAnimator.SetFloat("VerticalSpeed", Mathf.Abs(movementMagnitude.y));

        // Don't allow movement beyond these bounds:
        if (player.position.y >= 7.02379 && userInput.y > 0
            || player.position.y <= -7.054649 && userInput.y < 0) {
            movementMagnitude.y = 0;
        }
        if (player.position.x >= 11.81223 && userInput.x > 0
                   || player.position.x <= -12.06596 && userInput.x < 0) {
            movementMagnitude.x = 0;
        }

        player.MovePosition(player.position + movementMagnitude);
    }

    private void ChangePlayerDirection() {
        // Convert mouse position to position within game world, then create vector from player to mouse
        Vector3 playerToMouseVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        // Turn the player on the z-axis (Vector3.forward)
        float turnAngle = Mathf.Atan2(playerToMouseVector.y, playerToMouseVector.x) * Mathf.Rad2Deg;
        turnAngle += 270;  // Position adjustment due to the way the sprite was made
        Quaternion turnRotation = Quaternion.AngleAxis(turnAngle, Vector3.forward);

        transform.rotation = Quaternion.Lerp(transform.rotation, turnRotation, turnSpeed * Time.deltaTime);
    }
    
    // Collisions

    private void OnTriggerEnter2D(Collider2D collider) {
        string colliderTag = collider.tag;

        // Take damage only if alive
        if (colliderTag.Equals("EnemyProjectile") && !IsDead()) {
            int damageDone = collider.GetComponent<Bullet>().attackPower;
            entityPopup.CreateDamageText(transform, damageDone);

            DepleteHealth(damageDone);
            Destroy(collider.gameObject);
        }
    }

    // Other

    // Update player's attack power bonus, then update gun with that value
    public void UpgradeAttackPwr(int attkPwr) {
        projectileAttkPwrBonus = attkPwr;
        gun.UpdateGun(projectileAttkPwrBonus);
    }

}
