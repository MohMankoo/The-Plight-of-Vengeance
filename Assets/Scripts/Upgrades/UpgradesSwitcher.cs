using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Controls which upgrade tier to display on the upgrades screen
public class UpgradesSwitcher : MonoBehaviour {

    // Information-displaying panels
    public GameObject upgradeInfoPanel;
    private List<GameObject> upgradeInfoOptions;  // Individual panels for each upgrade

    // Selection fields
    // The following are stored as tags
    private string lastUpgradeInfoOptionSelected;
    private string selectedUpgradeOption;

    // Start is called before the first frame update
    void Start() {

        if (upgradeInfoPanel) {
            // Get all upgrade information panels
            upgradeInfoOptions = new List<GameObject>();
            foreach (Transform child in upgradeInfoPanel.transform) {
                upgradeInfoOptions.Add(child.gameObject);

                // Disable each child by default first
                child.gameObject.SetActive(false);
            }

            // Default the first upgrade option as selected
            upgradeInfoOptions[0].SetActive(true);
            selectedUpgradeOption = upgradeInfoOptions[0].tag;
            lastUpgradeInfoOptionSelected = upgradeInfoOptions[0].tag;
        }
    }

    // Manually toggle which button should be selected
    // Used upon player revival when the Upgrades screen is re-entered
    public void ToggleUpgradeButton(GameObject upgradeButton) {
        upgradeButton.GetComponent<Toggle>().isOn = true;
        SwitchToUpgradeGivenBy(upgradeButton);
    }

    // Fired every time an upgrade toggle is clicked on the left scrollview for any button toggled/de-toggled
    // Enabled button background (and associated upgrade info on right) if toggled and disables if de-toggled
    public void SwitchToUpgradeGivenBy(GameObject upgradeButton) {
        // Check whether the given button was toggled or de-toggled
        Toggle upgradeButtonToggle = upgradeButton.GetComponent<Toggle>();

        if (upgradeButtonToggle.isOn) {
            // Highlight image of button to indicate button selected
            upgradeButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            // Update the selected upgrade by tag name
            string newUpgradeOption = upgradeButton.tag;
            lastUpgradeInfoOptionSelected = selectedUpgradeOption;
            selectedUpgradeOption = newUpgradeOption;

            // Change upgrade information panel
            ChangeUpgradeDisplay();
        } else {
            // Upgrade de-selected - remove button background indicating button de-selection
            upgradeButton.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        }
    }

    // Activate the upgrade panel matching the selected option
    // and disable the previous option that was selected
    private void ChangeUpgradeDisplay() {
        // If the last selected and currently selected are the same, skip changing
        if (selectedUpgradeOption.Equals(lastUpgradeInfoOptionSelected))
            return;

        GameObject optionToEnable = null;
        GameObject optionToDisable = null;

        foreach (GameObject upgradeInfo in upgradeInfoOptions) {
            if (upgradeInfo.tag.Equals(selectedUpgradeOption)) {
                optionToEnable = upgradeInfo;
            }
            if (upgradeInfo.tag.Equals(lastUpgradeInfoOptionSelected)) {
                optionToDisable = upgradeInfo;
            }
        }

        optionToDisable.SetActive(false);
        optionToEnable.SetActive(true);
    }

}
