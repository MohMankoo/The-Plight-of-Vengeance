using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag.Equals("EnemyProjectile")
            || collider.tag.Equals("PlayerProjectile")) {
            Destroy(collider.gameObject);
        }
    }

}
