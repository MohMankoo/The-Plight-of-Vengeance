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
        float healthPercentage = (float)newHealth / (float)player.maxHealth;
        if (healthPercentage <= 0.3f) {  // Lower than 25% of health
            Debug.Log("Math: " + newHealth + "/" + player.maxHealth + " = " + healthPercentage);
            currentHealth.color = new Color32(210, 39, 39, 255);
        } else if (healthPercentage <= 0.6f) {  // Lower than 50% of health
            currentHealth.color = new Color32(255, 198, 0, 255);
        }
    }

    public void UpdateMaxHealth(int newHealth) {
        maxHealth.text = newHealth.ToString();
    }

    public void ResetDisplayColours() {
        currentHealth.color = new Color(255f, 255f, 255f);
    }
}
