using UnityEngine;
using System.Collections.Generic;

public class Enemy : Entity {

    // Keep track of all enemies craeted
    // int key = each Enemy will keep track of its order of creation
    //           given by an integer (formula: len(createdEnemies) + 1)
    // Enemy value = the Enemy associated with the key
    [HideInInspector]
    public static Dictionary<int, Enemy> currentEnemies = new Dictionary<int, Enemy>();

    // Player References
    protected Player player;
    protected Transform playerTransform;

    // Enemy component references
    protected Rigidbody2D enemyRB;
    protected Animator enemyAnimator;
    
    [Header("Enemy Attributes")]
    public EnemyGun gun;
    public int enemyIdentifier;  // The key of a specific instance in createdEnemies
    public int revengeScoreReward;

    [Header("Repelling Attributes")]
    public static float repelAmount = 2f;
    
    public void InitializeEnemy(Enemy enemy, int health, int revengeScoreReward, float speed, float turnSpeed) {
        // Update reference information
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAnimator = GetComponent<Animator>();
        enemyRB = GetComponent<Rigidbody2D>();
        entityPopup = GetComponent<EntityPopupCreator>();

        // Set enemy information
        this.health = health;
        this.revengeScoreReward = revengeScoreReward;
        this.speed = speed;
        this.turnSpeed = turnSpeed;

        // Set animator information
        enemyAnimator.SetBool("isPlayerDead", false);
        enemyAnimator.SetFloat("enemyHealth", this.health);

        // Update list of enemies
        enemyIdentifier = currentEnemies.Count + 1;  // Create new key
        currentEnemies.Add(enemyIdentifier, enemy); // Add enemy with associated key (enemyIdentifier)
        // Debug.Log("Enemy created: #" + enemyIdentifier + " | Total: " + currentEnemies.Count);
    }

    public void OnEnemyUpdate() {
        if (movementStopped)
            return;
        if (!player)  // If Player is null
            return;

        StopFunctionalityOnDeath();

        // Stop functionality if player is dead
        if (player.IsDead()) {
            StopMovement(true);
            gun.Jarr(true);
            enemyAnimator.SetBool("isPlayerDead", true);
        }

        // Repel from other enemies if too close
        // RepelOtherEnemies();

        // Move towrads player
        MoveTowardsPlayer();
    }

    // Set movement boundaries
    private void LateUpdate() {
        Vector2 restrictedPosition = enemyRB.position;
        restrictedPosition.x = Mathf.Clamp(enemyRB.position.x, GameManager.MIN_X_BOUNDARY, GameManager.MAX_X_BOUNDARY);
        restrictedPosition.y = Mathf.Clamp(enemyRB.position.y, GameManager.MIN_Y_BOUNDARY, GameManager.MAX_Y_BOUNDARY);
        enemyRB.position = restrictedPosition;
    }

    public void MoveTowardsPlayer() {
        if (movementStopped || IsDead())
            return;

        // Move towards player
        transform.position = 
            Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);

        // Face the player
        Vector2 vectorToPlayer = playerTransform.position - transform.position;
        float turnAngle = Mathf.Atan2(vectorToPlayer.y, vectorToPlayer.x) * Mathf.Rad2Deg;
        turnAngle += 270;  // Position adjustment due to the way the sprite was made

        Quaternion turnRotation = Quaternion.AngleAxis(turnAngle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, turnRotation, turnSpeed * Time.deltaTime);
    }

    public void StopFunctionalityOnDeath() {
        // If not already dead and should be, kill
        if (IsDead()) {
            // Stop enemy movement
            StopMovement(true);
            gun.Jarr(true);

            // Initiate death animation and sound feedback
            enemyAnimator.SetFloat("enemyHealth", health);
            AudioManager.PlayVoice("enemy");

            // Craete revenge score label and sound feedback
            entityPopup.CreateRevengeScoreText(
                transform, revengeScoreReward);
            AudioManager.PlayEffect("expGain");

            // Last-minute funtionality
            player.revengeScore += revengeScoreReward;  // Award Player
            bool removedSuccessfuly = currentEnemies.Remove(this.enemyIdentifier);  // Remove from list of alive enemies
            // Debug.Log("Enemy was removed: " + removedSuccessfuly);

            Destroy(gameObject, 2);
        }
    }

    private void OnDestroy() {
        bool removedSuccessfuly = currentEnemies.Remove(this.enemyIdentifier);  // Remove from list of alive enemies
        // Debug.Log("Enemy was removed: " + removedSuccessfuly);
    }

    public void RepelOtherEnemies() {
        if (currentEnemies is null) return;

        foreach(Enemy other in currentEnemies.Values) {
            // Ignore if found self
            if (this.Equals(other))
                continue;

            // Repel if the distance is too close
            Vector2 selfPosition = gameObject.GetComponent<Rigidbody2D>().position;
            Vector2 otherPosition = other.gameObject.GetComponent<Rigidbody2D>().position;

            if (Vector2.Distance(selfPosition, otherPosition) <= GameManager.minDistanceBetweenEnemies) {
                Vector2 repelForce = (selfPosition - otherPosition).normalized * repelAmount * Time.deltaTime;

                // Quick hack of making it non-kinematic to give it mass and add force
                gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
                gameObject.GetComponent<Rigidbody2D>().mass = 1;
                gameObject.GetComponent<Rigidbody2D>().AddForce(repelForce, ForceMode2D.Impulse);
                gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            }
        }
    }

    public void OnEnemyCollision(Collider2D collider) {
        string colliderTag = collider.tag;

        if (colliderTag.Equals("Player")) {
            player.DepleteHealth(player.maxHealth);  // Instantly kill player
        } else if (colliderTag.Equals("PlayerProjectile")) {
            // Damage self and destroy projectile
            int damageDone = collider.GetComponent<Bullet>().attackPower;
            entityPopup.CreateDamageText(transform, damageDone);
            AudioManager.PlayHitSound();

            DepleteHealth(damageDone);
            Destroy(collider.gameObject);
        }
    }

}
