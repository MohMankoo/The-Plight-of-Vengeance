using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private Vector3 bulletDirection;
    private GameObject shooter;

    // Bullet behaviour
    [Header("Projectile settings")]
    public int attackPower;
    public float projectileSizeFactor;
    public float projectileSpeed;

    // shooter = Enemy || Player
    public void Instantiate(int attackPower, float projectileSpeed, float projectileSizeFactor,
                            GameObject shooter) {

        // Randomize attack power to +/- of the provided attackPower
        int attackPowerOffset = Random.Range(-5, 5);
        this.attackPower = attackPower + attackPowerOffset;

        // Minimum value of 1 for attacks
        if (this.attackPower <= 0)
            this.attackPower = 1;

        // FORMULA for determining speed of bullet: entitySpeed + projectileSpeed
        // Ensure projectileSpeed is a constant rate faster than the entity's speed
        this.projectileSpeed = shooter.GetComponent<Entity>().speed + projectileSpeed;

        this.projectileSizeFactor = projectileSizeFactor;
        this.shooter = shooter;

        // Determine if the shooter is the player to decide bullet behaviour
        bool isPlayerShooter = false;
        if (this.shooter.tag.Equals("Player"))
            isPlayerShooter = true;

        // Decide path of projectile
        if (isPlayerShooter) {
            // Set the location to go towards as the mouse position
            bulletDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - 
                               transform.position).normalized;
            tag = "PlayerProjectile";

        } else {  // Enemy fired
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            bulletDirection = (player.position - transform.position).normalized;
            tag = "EnemyProjectile";
        }

        // Change scale of bullet
        transform.localScale *= this.projectileSizeFactor;

        // Move the bullet
        GetComponent<Rigidbody2D>().velocity = bulletDirection * this.projectileSpeed;
    }

    // Update is called once per frame
    void Update() {
        if (this.shooter == null)
            Destroy(gameObject);
    }

}
