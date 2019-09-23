using UnityEngine;

public class PlayerGun : Gun {

    private Animator gunAnimator;
    private GameObject player;

    [Header("Bullet")]
    public GameObject bullet;  // Bullet the gun uses

    [Header("Visual Effects")]
    public CameraShake cameraShake;

    // Start is called before the first frame update
    void Start() {
        // Initialize variables
        player = GameObject.FindGameObjectWithTag("Player");
        gunAnimator = GetComponent<Animator>();

        // Initialize variables
        cooldownTillNextShot = fixedCooldownTillNextShot;
    }

    // Update is called once per frame
    void Update() {
        if (jarred)
            return;

        // Shoot the gun only if cooldown has finished
        if (cooldownTillNextShot <= 0) {
            if (Input.GetMouseButtonDown(0)) 
            {
                Shoot(player, bullet);
                gunAnimator.SetTrigger("gunFired");
                //cameraShake.TriggerShaking(0.05f, 0.04f);

                cooldownTillNextShot = fixedCooldownTillNextShot;  // Reset cooldown
            }
        } else {
            cooldownTillNextShot -= Time.deltaTime;
        }
    }

    public void UpdateGun(string tag, int projectileAttkPwrBonus) {
        this.projectileAttkPwrBonus = projectileAttkPwrBonus;

        if (tag.Equals("Zyka")) {
            this.tag = "Zyka";
            gunAnimator.SetInteger("gunType", 1);

            attackPower = 15;
            projectileSizeFactor = 1f;
            projectileSpeed = 30;
            fixedCooldownTillNextShot = 0.2f;

        } else if (tag.Equals("X-Gun")) {
            this.tag = "X-Gun";
            gunAnimator.SetInteger("gunType", 2);

            attackPower = 15;
            projectileSizeFactor = 1f;
            projectileSpeed = 35;
            fixedCooldownTillNextShot = 0.1f;

        } else if (tag.Equals("V-Gun")) {
            this.tag = "V-Gun";
            gunAnimator.SetInteger("gunType", 3);

            attackPower = 15;
            projectileSizeFactor = 1f;
            projectileSpeed = 40;
            fixedCooldownTillNextShot = 0.05f;
        }
    }

    // Overloaded method that updates ONLY attack power
    public void UpdateGun(int projectileAttkPwrBonus) {
        this.projectileAttkPwrBonus = projectileAttkPwrBonus;
    }

}
