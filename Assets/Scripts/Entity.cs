using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

    // Used to display information above characters
    protected EntityPopupCreator entityPopup;

    [Header("Health")]
    protected int health;
    protected bool isInvincible = false;

    [Header("Movement")]
    public float speed;
    public float turnSpeed;
    protected bool movementStopped = false;

    public void DepleteHealth(int hitPoints) {
        if (isInvincible) return;

        if (health >= hitPoints)
            SetHealth(health - hitPoints);
        else
            SetHealth(0);
    }

    public bool IsDead() {
        return health <= 0;
    }

    public void Kill() {
        if (!isInvincible)
            DepleteHealth(health);
    }

    public void StopMovement(bool movementStopped) {
        this.movementStopped = movementStopped;
    }

    // Gettrs/Setters

    public void SetHealth(int health) {
        this.health = health;
    }

    public int GetHealth() {
        return health;
    }

    public void SetSpeed(float speed) {
        this.speed = speed;
    }

    public void SetTurnSpeed(float turnSpeed) {
        this.turnSpeed = turnSpeed;
    }

}
