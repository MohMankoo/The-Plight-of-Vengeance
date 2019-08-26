using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneSwitcher : MonoBehaviour {

    void OnGUI() {
        // if any keyboard key is pressed AND we are on an introductory scene, switch to next scene
        if (Input.anyKey && SceneManager.GetActiveScene().buildIndex < 3) {
            Event currEvent = Event.current;
            if (currEvent != null)
                if (currEvent.isKey)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
            
    }

    // Load any specified scene, be it the next one or not
    // For more flexibility
    public void LoadScene(int scene) {
        SceneManager.LoadScene(scene);
    }

}
