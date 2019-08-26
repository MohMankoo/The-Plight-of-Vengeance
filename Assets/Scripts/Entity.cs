using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

    protected int health;
    public float speed;
    public float turnSpeed;
    protected bool movementStopped = false;

    // Used to display information above characters
    protected EntityPopupCreator entityPopup;

    public void DepleteHealth(int hitPoints) {
        if (health >= hitPoints)
            SetHealth(health - hitPoints);
        else
            SetHealth(0);
    }

    public bool IsDead() {
        return health <= 0;
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
