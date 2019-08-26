using UnityEngine;

public class Guard : Enemy {

    private void Start() {
        // Update enemy information
        InitializeEnemy(this, 175, 150, 1.2f, 15f);
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
