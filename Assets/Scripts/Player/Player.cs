using UnityEngine;
using System.Collections;

public class Player : Entity {
    
    // Player components
    private Rigidbody2D playerRB;
    private Animator playerAnimator;

    [Header("Health")]
    public PlayerHealth playerHealthDisplay;  // Game HUD
    public int maxHealth = 100;

    [Header("Progress")]
    public int revengeScore = 0;

    [Header("Player Gun")]
    public PlayerGun gun;
    public int projectileAttkPwrBonus = 1;

    [Header("Dashing")]
    public bool canDash;
    public float dashCooldown = 3f;
    public float dashIntensity = 500;
    private bool justDashed;

    [Header("Visual effects")]
    public CameraShake cameraShake;

    // Other
    private string blockedFeedback = "BLOCK";
    private bool allFunctionalityStopped;

    private void Start() {
        // Initialize properties
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        entityPopup = GetComponent<EntityPopupCreator>();

        // General functionality
        allFunctionalityStopped = false;

        // Dashing
        canDash = false;
        justDashed = false;

        // Health
        health = maxHealth;
        playerHealthDisplay.UpdateCurrentHealth(health);
        playerHealthDisplay.UpdateMaxHealth(maxHealth);
        playerAnimator.SetFloat("PlayerHealth", health);

        // Update speed attribute inherited from Entity.cs
        speed = 4f;

        // Set the default gun as Zyka
        gun.UpdateGun("Zyka", projectileAttkPwrBonus);
    }

    // Frame updates

    // Even though movement is not physics-related, put it in FixedUpdate()
    private void FixedUpdate() {
        if (movementStopped || allFunctionalityStopped) return;
        MovePlayer();

        // Change direction if mouse is within window
        if (Input.mousePosition.x >= 0 && Input.mousePosition.y >= 0
                && Input.mousePosition.x <= Screen.width && Input.mousePosition.y <= Screen.height) {
            ChangePlayerDirection();
        }
    }

    // Set movement boundaries
    private void LateUpdate() {
        Vector2 restrictedPosition = playerRB.position;
        restrictedPosition.x = Mathf.Clamp(playerRB.position.x, GameManager.MIN_X_BOUNDARY, GameManager.MAX_X_BOUNDARY);
        restrictedPosition.y = Mathf.Clamp(playerRB.position.y, GameManager.MIN_Y_BOUNDARY, GameManager.MAX_Y_BOUNDARY);
        playerRB.position = restrictedPosition;
    }

    private void Update() {
        if (!allFunctionalityStopped)
            StopFunctionalityOnDeath();
    }

    // Health

    private void StopFunctionalityOnDeath() {
        // Check if player should be dead
        if (IsDead() && !isInvincible) {
            StopMovement(true);
            gun.Jarr(true);
            allFunctionalityStopped = true;

            AudioManager.PlayVoice("player");
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
        if (!isInvincible) {
            base.DepleteHealth(hitPoints);
            playerHealthDisplay.UpdateCurrentHealth(health);
            playerAnimator.SetFloat("PlayerHealth", health);
        }
    }

    public void ReplenishFullHealth() {
        SetHealth(maxHealth);
        playerHealthDisplay.UpdateCurrentHealth(health);
        playerAnimator.SetFloat("PlayerHealth", health);
    }

    public IEnumerator MakeInvincible(float time) {
        isInvincible = true;
        yield return new WaitForSeconds(time);
        isInvincible = false;
    }

    // Movement

    private void MovePlayer() {
        // Keyboard movement
        Vector2 userInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 movementMagnitude = userInput.normalized * Time.deltaTime * speed;

        // Animate player
        playerAnimator.SetFloat("HorizontalSpeed", Mathf.Abs(movementMagnitude.x));
        playerAnimator.SetFloat("VerticalSpeed", Mathf.Abs(movementMagnitude.y));

        playerRB.MovePosition(playerRB.position + movementMagnitude);

        // Dash if able
        if (canDash && !justDashed && Input.GetKeyDown(KeyCode.Space)) {
            Vector2 dashDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            justDashed = true;

            StartCoroutine(MakeInvincible(0.9f));
            StartCoroutine(Dash(dashDir));
        }
    }

    IEnumerator Dash(Vector2 direction) {
        // Shake camera for effect and add sound
        cameraShake.TriggerShaking(0.2f, 0.1f);
        AudioManager.PlayEffect("dash");
        
        float dashLength = 0.8f;
        while (dashLength > 0) {
            playerRB.MovePosition(playerRB.position + (direction * dashIntensity));
            dashLength -= Time.deltaTime;
        }

        yield return new WaitForSeconds(dashCooldown);
        justDashed = false;
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
            if (!isInvincible) {
                int damageDone = collider.GetComponent<Bullet>().attackPower;

                entityPopup.CreateDamageText(transform, damageDone);
                AudioManager.PlayHitSound();
                DepleteHealth(damageDone);
            } else {
                entityPopup.CreateDamageText(transform, blockedFeedback);
            }

            Destroy(collider.gameObject);
        }
    }

    // Other

    // Update player's attack power bonus, then update gun with that value
    public void UpgradeAttackPwr(int attkPwr) {
        projectileAttkPwrBonus = attkPwr;
        gun.UpdateGun(projectileAttkPwrBonus);
    }

    public void SetCanDash(bool canDash) {
        this.canDash = canDash;
    }

    public void SetFullFunctionalityIndicator() {
        this.allFunctionalityStopped = false;
    }

}
