using UnityEngine;

public class EnemyGun : Gun {

    // Gun users and targets
    private Player playerTarget;  // Target of EnemyGun
    private GameObject enemyUser;
    private bool enemyUserSet = false;

    // Gun Components
    private Animator enemyGunAnimator;
    public GameObject bullet;  // Bullet the gun uses
    
    // Start is called before the first frame update
    void Start() {
        // Initialize variables
        playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemyGunAnimator = GetComponent<Animator>();
        cooldownTillNextShot = fixedCooldownTillNextShot;
    }

    // Update is called once per frame
    void Update() {
        if (jarred)
            return;

        // Only decide to shoot if the enemy wielder is defined and player is alive
        if (enemyUserSet && !playerTarget.IsDead()) {
            // Shoot the gun only if cooldown has finished
            if (cooldownTillNextShot <= 0) {
                Shoot(enemyUser, bullet);
                enemyGunAnimator.SetTrigger("gunFired");

                cooldownTillNextShot = fixedCooldownTillNextShot;  // Reset cooldown

            } else {
                cooldownTillNextShot -= Time.deltaTime;
            }

        }
    }

    public void UpdateEnemyGun(GameObject enemyUser) {
        this.enemyUser = enemyUser;
        string tag = enemyUser.tag;

        projectileAttkPwrBonus = 1;

        if (tag.Equals("LesserGuard")) {
            tag = "LesserGuard";
            attackPower = 100;
            projectileSizeFactor = 0.7f;
            projectileSpeed = 2f;
            fixedCooldownTillNextShot = 2.5f;

        } else if (tag.Equals("Guard")) {
            tag = "Guard";
            attackPower = 12;
            projectileSizeFactor = 0.7f;
            projectileSpeed = 2f;
            fixedCooldownTillNextShot = 0.2f;

        } else if (tag.Equals("SuperiorGuard")) {
            tag = "SuperiorGuard";
            attackPower = 250;
            projectileSizeFactor = 2f;
            projectileSpeed = 8;
            fixedCooldownTillNextShot = 2.5f;

        } else {  // Commander
            tag = "Commander";
            attackPower = 999;
            projectileSizeFactor = 3f;
            projectileSpeed = 15;
            fixedCooldownTillNextShot = 3f;
        }

        // Make changes take effect
        cooldownTillNextShot = fixedCooldownTillNextShot;
        enemyUserSet = true;
    }

}
