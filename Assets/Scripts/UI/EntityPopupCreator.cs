using UnityEngine;
using UnityEngine.UI;

public class EntityPopupCreator : MonoBehaviour {

    [Header("General")]
    public Text entityText;
    public static float destroyTime = 0.6f;

    public void CreateDamageText(Transform entity, int damage) {
        Text damageText = CreateTemporaryText(entity, "EntityCanvas");
        damageText.text = damage.ToString();
        
        Destroy(damageText, destroyTime);
    }

    public void CreateRevengeScoreText(Transform entity, int revengeScore) {
        Text revengeScoreText = CreateTemporaryText(entity, "EntityCanvas");

        // Change text formatting
        revengeScoreText.text = "+ " + revengeScore;
        revengeScoreText.color = new Color(255f, 198f, 0f);  // Bright yellow

        // Offset text so it does not overlap damage text
        Vector3 revengeScoreOffset = new Vector3(0.6f, 0.9f, 0);  // Move up y-axis
        revengeScoreText.transform.position += revengeScoreOffset;

        Destroy(revengeScoreText, destroyTime);
    }

    // For Upgrade button in Upgrades shop
    // Used by UpgradesManager
    public void CreateNegativeRevengeText(Transform upgradeButton, string feedbackText) {
        Text revengeCostText = CreateTemporaryText(upgradeButton, "");  // Canvas tag not needed

        // Change text formatting
        revengeCostText.fontSize = 120;
        revengeCostText.text = feedbackText;
        revengeCostText.color = new Color32(255, 255, 255, 255);

        Destroy(revengeCostText, destroyTime);
    }

    // canvasTag: determines which canvas to spawn text on
    private Text CreateTemporaryText(Transform entity, string canvasTag) {

        Transform entityCanvas;
        Text createdText;

        // Determine which canvas to spawn text on
        // If canvas belongs to an enemy/player, find it as child
        if (canvasTag.Equals("EntityCanvas")) {
            entityCanvas = entity.Find(canvasTag);
            createdText = Instantiate(entityText, entityCanvas.position, Quaternion.identity);

        } else {  // If finding canvas inside the UpgradeButtons
            entityCanvas = entity.GetComponentInChildren<Canvas>().transform;
            createdText = Instantiate(entityText, entityCanvas.position, Quaternion.identity);
        }

        createdText.transform.SetParent(entityCanvas);  // Add text to canvas
        return createdText;
    }

}
