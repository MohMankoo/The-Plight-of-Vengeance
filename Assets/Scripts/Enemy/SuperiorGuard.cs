using UnityEngine;
using System.Collections;

public class SuperiorGuard : Enemy {

    private void Start() {
        // Update enemy information
        InitializeEnemy(this, 250, 300, 0.8f, 20f);
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
