using UnityEngine;
using System.Collections.Generic;

public class Enemy : Entity {

    // Keep track of all enemies craeted
    // int key = each Enemy will keep track of its order of creation
    //           given by an integer (formula: len(createdEnemies) + 1)
    // Enemy value = the Enemy associated with the key
    public static Dictionary<int, Enemy> currentEnemies = new Dictionary<int, Enemy>();

    // Unity component References
    protected Player player;
    protected Transform playerTransform;
    protected Animator enemyAnimator;
    
    public EnemyGun gun;

    // Attributes
    public int enemyIdentifier;  // The key of a specific instance in createdEnemies
    public int revengeScoreReward;

    // Repelling attributes
    public float repelAmount = 2f;
    
    public void InitializeEnemy(Enemy enemy, int health, int revengeScoreReward, float speed, float turnSpeed) {
        // Update reference information
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAnimator = GetComponent<Animator>();
        entityPopup = GetComponent<EntityPopupCreator>();

        // Set enemy information
        this.health = health;
        this.revengeScoreReward = revengeScoreReward;
        this.speed = speed;
        this.turnSpeed = turnSpeed;

        enemyIdentifier = currentEnemies.Count + 1;  // Create new key
        currentEnemies.Add(enemyIdentifier, enemy); // Add enemy with associated key (enemyIdentifier)

        Debug.Log("Enemy created: " + enemyIdentifier + "- Total: " + currentEnemies.Count);
    }

    public void OnEnemyUpdate() {
        if (movementStopped)
            return;

        KillIfHealthDepleted();

        // Return if player object is null
        if (!player)
            return;

        // Stop functionality if player is dead
        if (player.IsDead()) {
            StopMovement(true);
            gun.Jarr(true);
            enemyAnimator.SetBool("isPlayerDead", true);
        }

        // Repel from other enemies if too close
        RepelOtherEnemies();

        // Move towrads player if not dead
        MoveTowardsPlayer();
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

    public void KillIfHealthDepleted() {
        // If not already dead and should be, kill
        if (IsDead()) {
            // Stop enemy movement
            StopMovement(true);
            gun.Jarr(true);

            // Initiate death animation
            enemyAnimator.SetFloat("enemyHealth", health);

            // Craete revenge score label
            entityPopup.CreateRevengeScoreText(
                transform, revengeScoreReward);

            // Last-minute funtionality
            player.revengeScore += revengeScoreReward;
            currentEnemies.Remove(this.enemyIdentifier);  // Remove from list of alive enemies
            Destroy(gameObject, 2);
        }
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

            DepleteHealth(damageDone);
            Destroy(collider.gameObject);
        }
    }

}
