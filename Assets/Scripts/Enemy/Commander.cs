using UnityEngine;

public class Commander : Enemy {

    private void Start() {
        // Update enemy information
        InitializeEnemy(this, 999, 2000, 1.35f, 20f);
        gun.UpdateEnemyGun(gameObject);  // tag = type of enemy

        // Setup animator information
        enemyAnimator.SetFloat("enemyHealth", health);
        enemyAnimator.SetBool("isPlayerDead", false);
    }

    private void Update() {
        OnEnemyUpdate();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (!IsDead())
            OnEnemyCollision(collider);
    }
}
