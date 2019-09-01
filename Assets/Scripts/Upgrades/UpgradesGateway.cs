using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls entering the game from the upgrades screen and vice versa
public class UpgradesGateway : MonoBehaviour {

    private Player player;
    public PlayerHealth playerHealthDisplay;
    public GameObject spawner;

    // Player defaults
    public Vector3 DEFAULT_PLAYER_POS = new Vector3(0, -6.5f, 0);
    public Quaternion DEFAULT_PLAYER_ROTATION = Quaternion.identity;

    // For showing "Revived message" upon player death
    public Canvas gameHUD;
    public GameObject revivedDisplay;

    // Upgrades screen information
    public Canvas upgradesCanvas;
    private Animator upgradesCanvasAninmator;

    public UpgradesWindowManager upgradesWindowManager;
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
        player.transform.position = DEFAULT_PLAYER_POS;
        player.transform.rotation = DEFAULT_PLAYER_ROTATION;

        Debug.Log(player.transform.position + " and " + DEFAULT_PLAYER_POS);

        player.StopMovement(false);
        player.gun.Jarr(false);

        playerHealthDisplay.ResetDisplayColours();
        playerHealthDisplay.SetVisible(true);

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
        upgradesWindowManager.ToggleUpgradeButton(upgradeButtonSelectedOnRevival);

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
