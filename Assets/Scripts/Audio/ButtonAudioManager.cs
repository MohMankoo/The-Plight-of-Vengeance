using UnityEngine;

public class ButtonAudioManager : MonoBehaviour {
    public void PlayEffect (string effectName) {
        AudioManager.PlayEffect(effectName);
    }
}
