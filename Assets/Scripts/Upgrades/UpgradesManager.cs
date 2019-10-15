using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Controls Player upgrades: purchase, buying feedback, updates to upgrades screen.
public class UpgradesManager : MonoBehaviour {

    private Player player;

    // References to Upgrade images
    [Header("Upgrades Images")]
    public Image healthSprite;
    public Image speedSprite;
    public Image attackPwrSprite;
    public Image gunSprite;

    // The levels of upgrades bought thus far
    private int healthUpgradeLevel;
    private int speedUpgradeLevel;
    private int attackPowerUpgradeLevel;
    private int gunUpgradeLevel;

    [Header("HUD")]
    public Text revengeScoreLabel;  // Used to display the revenge score on HUD

    [Header("Misc")]
    public MouseCursor mouseCursorHandler;
    private EntityPopupCreator popupCreator;

    private string insufficientFunds = "Your revenge is lacking";
    private string atMaxPower = "Your skill is unmatchable";

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        popupCreator = GetComponent<EntityPopupCreator>();

        // Initialize upgrade levels
        healthUpgradeLevel = 0;
        speedUpgradeLevel = 0;
        attackPowerUpgradeLevel = 0;
        gunUpgradeLevel = 0;
    }

    private void Update() {
        revengeScoreLabel.text = player.revengeScore.ToString();
    }

    // Used by upgrade buttons in upgrades screen
    public void BuyUpgrade(GameObject upgradePanel) {

        // Develop the upgrade tiers as a list
        // Ex: Health Upgrade 1, Health Upgrade 2, ...
        List<Text> upgradeLevels = new List<Text>();
        foreach (Transform child in upgradePanel.transform) {
            if (child.tag.Equals("UpgradeLevel"))
                upgradeLevels.Add(child.GetComponent<Text>());
        }

        // Get a reference to the BUY button clicked, among other variables
        GameObject upgradeButton = upgradePanel.GetComponentInChildren<Button>().gameObject;
        string upgradePath = upgradeButton.tag;  // Health, speed, gun, power, etc.
        int playerRevengeBeforeUpgrade = player.revengeScore;

        if (upgradePath.Equals("HealthUpgrade")) {
            // Check if already maxed out
            if (healthUpgradeLevel < 3) 
            {
                int upgradeToAttempt = healthUpgradeLevel + 1;

                if (AddHealthUpgrade(upgradeToAttempt)) {
                    ShowBuyingFeedback(upgradeButton, upgradeLevels,
                        playerRevengeBeforeUpgrade, upgradeToAttempt);
                } else {
                    popupCreator.CreateShopFeedback(
                        upgradeButton.transform, insufficientFunds, false);
                }
            }  else { 
                popupCreator.CreateShopFeedback(
                    upgradeButton.transform, atMaxPower, false); 
            }
        } else if (upgradePath.Equals("SpeedUpgrade")) {
            if (speedUpgradeLevel < 3) {
                int upgradeToAttempt = speedUpgradeLevel + 1;

                if (AddSpeedUpgrade(upgradeToAttempt)) {
                    ShowBuyingFeedback(upgradeButton, upgradeLevels,
                        playerRevengeBeforeUpgrade, upgradeToAttempt);
                } else {
                    popupCreator.CreateShopFeedback(
                        upgradeButton.transform, insufficientFunds, false);
                }
            } else {
                popupCreator.CreateShopFeedback(
                    upgradeButton.transform, atMaxPower, false);
            }
        } else if (upgradePath.Equals("AttackPowerUpgrade")) {
            if (attackPowerUpgradeLevel < 3) {
                int upgradeToAttempt = attackPowerUpgradeLevel + 1;

                if (AddAttackPwrUpgrade(upgradeToAttempt)) {
                    ShowBuyingFeedback(upgradeButton, upgradeLevels,
                        playerRevengeBeforeUpgrade, upgradeToAttempt);
                } else {
                    popupCreator.CreateShopFeedback(
                        upgradeButton.transform, insufficientFunds, false);
                }
            } else {
                popupCreator.CreateShopFeedback(
                    upgradeButton.transform, atMaxPower, false);
            }
        } else {  // Gun upgrade
            if (gunUpgradeLevel < 2) {
                int upgradeToAttempt = gunUpgradeLevel + 1;

                if (AddGunUpgrade(upgradeToAttempt)) {
                    ShowBuyingFeedback(upgradeButton, upgradeLevels,
                        playerRevengeBeforeUpgrade, upgradeToAttempt);
                } else {
                    popupCreator.CreateShopFeedback(
                        upgradeButton.transform, insufficientFunds, false);
                }
            } else {
                popupCreator.CreateShopFeedback(
                    upgradeButton.transform, atMaxPower, false);
            }
        }
    }

    private void ShowBuyingFeedback (GameObject upgradeButton, List<Text> upgradeLevels,
                                    int playerRevengeBeforeUpgrade, int upgradeBought) {
        // Show visual success feedback
        int revengeScoreLost = playerRevengeBeforeUpgrade - player.revengeScore;
        popupCreator.CreateShopFeedback(
            upgradeButton.transform, "-" + revengeScoreLost, true);

        // Paint the recently bought upgrade GREEN
        int upgradeTextIndex = upgradeBought - 1;
        upgradeLevels[upgradeTextIndex].color = new Color32(41, 134, 0, 255);
    }

    public bool AddHealthUpgrade(int level) {
        if (level == 1  && player.revengeScore >= 250) {
            player.SetMaxHealth(200);

            player.revengeScore -= 250;
            healthUpgradeLevel++;

            // Update the sprite to the sprite of the next level available (if possible)
            healthSprite.sprite = Resources.Load<Sprite>("Upgrades/HealthUpgrade" + healthUpgradeLevel);
            healthSprite.color = new Color32(255, 255, 255, 255); // Make sprite visible upon first upgrade
            return true;

        } else if (level == 2 && player.revengeScore >= 750) {
            player.SetMaxHealth(500);

            player.revengeScore -= 750;
            healthUpgradeLevel++;

            healthSprite.sprite = Resources.Load<Sprite>("Upgrades/HealthUpgrade" + healthUpgradeLevel);
            return true;

        } else if (level == 3 && player.revengeScore >= 1500) { // Level 3
            player.SetMaxHealth(999);

            player.revengeScore -= 1500;
            healthUpgradeLevel++;

            healthSprite.sprite = Resources.Load<Sprite>("Upgrades/HealthUpgrade" + healthUpgradeLevel);
            return true;
        }

        return false;  // Failure to upgrade
    }

    public bool AddSpeedUpgrade(int level) {
        if (level == 1 && player.revengeScore >= 250) {
            player.SetSpeed(5.5f);
            player.SetTurnSpeed(20);

            player.revengeScore -= 250;
            speedUpgradeLevel++;

            // Update the sprite to the sprite of the next level available (if possible)
            speedSprite.sprite = Resources.Load<Sprite>("Upgrades/SpeedUpgrade" + speedUpgradeLevel);
            speedSprite.color = new Color32(255, 255, 255, 255); // Make sprite visible upon first upgrade
            return true;

        } else if (level == 2 && player.revengeScore >= 850) {
            player.SetCanDash(true);

            player.revengeScore -= 850;
            speedUpgradeLevel++;

            speedSprite.sprite = Resources.Load<Sprite>("Upgrades/SpeedUpgrade" + speedUpgradeLevel);
            return true;

        } else if (level == 3 && player.revengeScore >= 1250) { // Level 3
            player.SetSpeed(7f);
            player.SetTurnSpeed(30);

            player.revengeScore -= 1250;
            speedUpgradeLevel++;

            speedSprite.sprite = Resources.Load<Sprite>("Upgrades/SpeedUpgrade" + speedUpgradeLevel);
            return true;
        }

        return false; // Failure to upgrade
    }

    public bool AddAttackPwrUpgrade(int level) {
        if (level == 1 && player.revengeScore >= 300) {
            player.UpgradeAttackPwr(5);

            player.revengeScore -= 300;
            attackPowerUpgradeLevel++;

            attackPwrSprite.sprite = Resources.Load<Sprite>("Upgrades/DamageUpgrade" + attackPowerUpgradeLevel);
            attackPwrSprite.color = new Color32(255, 255, 255, 255); // Make sprite visible upon first upgrade

            // Update projectile bonus on the gun
            player.gun.UpdateGun(player.projectileAttkPwrBonus);
            return true;

        } else if (level == 2 && player.revengeScore >= 800) {
            player.UpgradeAttackPwr(12);

            player.revengeScore -= 800;
            attackPowerUpgradeLevel++;

            attackPwrSprite.sprite = Resources.Load<Sprite>("Upgrades/DamageUpgrade" + attackPowerUpgradeLevel);
            player.gun.UpdateGun(player.projectileAttkPwrBonus);
            return true;

        } else if (level == 3 && player.revengeScore >= 1500) { // Level 3
            player.UpgradeAttackPwr(18);

            player.revengeScore -= 1500;
            attackPowerUpgradeLevel++;

            attackPwrSprite.sprite = Resources.Load<Sprite>("Upgrades/DamageUpgrade" + attackPowerUpgradeLevel);
            player.gun.UpdateGun(player.projectileAttkPwrBonus);
            return true;
        }

        return false; // Failure to upgrade
    }
    
    public bool AddGunUpgrade(int level) {
        if (level == 1 && player.revengeScore >= 650) {  // X-Gun
            player.gun.UpdateGun("X-Gun", player.projectileAttkPwrBonus);

            player.revengeScore -= 650;
            gunUpgradeLevel++;

            // Change gun sprite and cursor
            gunSprite.sprite = Resources.Load<Sprite>("Upgrades/X-GunUpgrade");
            mouseCursorHandler.ChangeGameCursorType("X-Gun");
            return true;

        } else if (level == 2 && player.revengeScore >= 1750) {  // V-Gun
            player.gun.UpdateGun("V-Gun", player.projectileAttkPwrBonus);

            player.revengeScore -= 1750;
            gunUpgradeLevel++;

            // Change gun sprite and cursor
            gunSprite.sprite = Resources.Load<Sprite>("Upgrades/V-GunUpgrade");
            mouseCursorHandler.ChangeGameCursorType("V-Gun");
            return true;
        }

        return false; // Failure to upgrade
    }
}
