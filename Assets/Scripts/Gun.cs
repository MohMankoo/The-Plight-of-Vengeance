using UnityEngine;

public class Gun : MonoBehaviour {

    [Header("Attack Power")]
    public int attackPower;
    public int projectileAttkPwrBonus;

    [Header("Projectile Properties")]
    public float projectileSizeFactor;
    public float projectileSpeed;

    // Jarr the gun to stop functionality
    protected bool jarred = false;

    [Header("Cooldown")]
    public float fixedCooldownTillNextShot;
    public float cooldownTillNextShot;

    public void Jarr(bool jarred) {
        this.jarred = jarred;

        // Reset the cooldown once no longer jarred status
        if (!jarred)
            cooldownTillNextShot = fixedCooldownTillNextShot;
    }

    public void Shoot(GameObject wielder, GameObject bullet) {
        if (jarred) return;

        // Shoot the bullet and save an instance of it
        Bullet shotTaken = Instantiate(bullet, transform.position, wielder.transform.rotation)
                                .GetComponent<Bullet>();

        // Calculate attack power
        int totalAttackPwr = attackPower + projectileAttkPwrBonus;

        // If bullet successfuly shot, initialize it
        if (shotTaken)
            shotTaken.Instantiate(totalAttackPwr, projectileSpeed, projectileSizeFactor, wielder);
    }

}
