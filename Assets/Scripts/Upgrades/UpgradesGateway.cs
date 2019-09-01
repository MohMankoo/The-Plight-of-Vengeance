using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls entering the game from the upgrades screen and vice versa
public class UpgradesGateway : MonoBehaviour {
    
    // Player and Enemies
    private Player player;
    public PlayerHealth playerHealthDisplay;
    public GameObject spawner;

    // Cursor 
    public MouseCursor mouseCursorHandler;

    // For showing "Revived message" upon player death
    public Canvas gameHUD;
    public GameObject revivedDisplay;

    // Upgrades screen information
    public Canvas upgradesCanvas;
    private Animator upgradesCanvasAninmator;

    public UpgradesSwitcher upgradesSwitcher;
    public GameObject upgradeButtonSelectedOnRevival;

    private bool gameOver;

    void Start() {
        // Set Upgrades menu animator
        upgradesCanvasAninmator = upgradesCanvas.GetComponent<Animator>();

        // Set player data
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.StopMovement(true);
        player.gun.Jarr(true);

        // Set spawner data
        spawner.SetActive(false);

        gameOver = false;
    }

    private void Update() {
        // Once player dies, end game and transition to Upgrades screen
        if (player.IsDead() && !gameOver) {
            gameOver = true;
            EnterUpgrades();
        }
    }

    public void ExitUpgrades() {
        player.ReplenishFullHealth();
        player.transform.position = GameManager.defaultPlayerPos;
        player.transform.rotation = GameManager.defaultPlayerRotation;

        player.StopMovement(false);
        player.gun.Jarr(false);

        playerHealthDisplay.ResetDisplayColours();
        playerHealthDisplay.SetVisible(true);

        // Change mouse cursor
        mouseCursorHandler.EnableGameCursor();

        // When entering game, reset currentEnemies list
        gameOver = false;
        Enemy.currentEnemies.Clear();

        // Set animator information and transition
        upgradesCanvasAninmator.enabled = true;
        upgradesCanvasAninmator.SetTrigger("exitUpgrades");
        StartCoroutine(TriggerUpgradesTransition(false));
    }

    public void EnterUpgrades() {
        player.StopMovement(true);
        player.gun.Jarr(true);

        playerHealthDisplay.SetVisible(false);

        // Show REVIVED message
        GameObject revivedDisplayMsg =
                    Instantiate(revivedDisplay, gameHUD.transform.position, Quaternion.identity);
        revivedDisplayMsg.transform.SetParent(gameHUD.transform);

        Destroy(revivedDisplayMsg, 1.2f);

        // Set animator information
        upgradesCanvasAninmator.enabled = true;
        StartCoroutine(TriggerUpgradesTransition(true));

        // Change mouse cursor
        mouseCursorHandler.EnableMenuCursor();
    }

    // Transition from or to the upgrades screen
    // If enteringUpgrades, turn on Upgrades canvas and turn off spawner, else do the opposite
    IEnumerator TriggerUpgradesTransition(bool enteringUpgrades) {
        // Spawner
        yield return new WaitForSeconds(0.2f);
        if (enteringUpgrades)
            spawner.SetActive(false);
        else
            spawner.SetActive(true);

        // Upgrades canvas
        yield return new WaitForSeconds(0.85f);
        upgradesCanvas.gameObject.SetActive(enteringUpgrades);

        // Select health upgrade button/toggle by default on enter/exit
        upgradesSwitcher.ToggleUpgradeButton(upgradeButtonSelectedOnRevival);

        // Trigger animation for entering Upgrades AFTER Upgrades panel has been re-activated
        // to prevent buggy behaviour
        if (enteringUpgrades) {
            upgradesCanvasAninmator.SetTrigger("enterUpgrades");

            // After x seconds, disable animator as it prevents upgrade images from changing color
            yield return new WaitForSeconds(1f);
            upgradesCanvasAninmator.enabled = false;
        }
    }

}
