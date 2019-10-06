using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {
    // Load the scene directly after the current one in the build order.
    public void LoadNextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Load any specified scene, be it the next one or not (for more flexibility).
    public void LoadScene(int scene) {
        SceneManager.LoadScene(scene);
    }

    /*void OnGUI() {
        // if any keyboard key is pressed AND we are on an introductory scene, switch to next scene.
        if (Input.anyKey && SceneManager.GetActiveScene().buildIndex < 3) {
            Event currEvent = Event.current;
            if (currEvent != null && currEvent.isKey)
                LoadNextScene();
        }       
    }*/
}
