using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Camera script
public class PlayerFollow : MonoBehaviour {

    private Player player;
    public Vector3 offsetFromPlayer;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update() {
        // Follow player only if they are alive
        if (!player.IsDead())
            transform.position = player.transform.position - offsetFromPlayer;
    }
}
