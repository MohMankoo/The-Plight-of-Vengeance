using UnityEngine;

public class LesserGuard : Enemy {

    private void Start() {
        // Update enemy information
        InitializeEnemy(this, 70, 50, 1f, 10f);
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
