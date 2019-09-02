using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    [Header("Player Properties")]
    public Player player;
    public GameObject playerHealthHUD;

    [Header("Display Properties")]
    public Text currentHealth;
    public Text maxHealth;

    public void SetVisible(bool on) {
        if (on)
            playerHealthHUD.SetActive(true);
        else
            playerHealthHUD.SetActive(false);
    }

    public void UpdateCurrentHealth(int newHealth) {
        currentHealth.text = newHealth.ToString();

        // Colour text to show feedback on low health
        if (newHealth / player.maxHealth <= 0.25f) {  // Lower than 25% of health
            currentHealth.color = new Color(210f, 39f, 39f);
        } else if (newHealth / player.maxHealth <= 0.5f) {  // Lower than 50% of health
            currentHealth.color = new Color(255f, 198f, 0f);
        }
    }

    public void UpdateMaxHealth(int newHealth) {
        maxHealth.text = newHealth.ToString();
    }

    public void ResetDisplayColours() {
        currentHealth.color = new Color(255f, 255f, 255f);
    }
}
