using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public Camera mainCamera;

    private Vector3 DEFAULT_CAMERA_POS = new Vector3(0, 0, -5);
    private float shakeIntenisty = 1;

    // Shake screen repeatedly (every 0.01s) for length
    public void TriggerShaking(float intensity, float length) {
        shakeIntenisty = intensity;

        InvokeRepeating("Shake", 0, 0.01f);
        Invoke("StopShaking", length);
    }

    // Helpers to shake the camera

    private void Shake() {
        // Common formula for calculating nice-looking screen shake
        float shakeOffsetX = (Random.value * shakeIntenisty * 2) - shakeIntenisty;
        float shakeOffsetY = (Random.value * shakeIntenisty * 2) - shakeIntenisty;

        Vector3 camPositionAfterShake = mainCamera.transform.position;
        camPositionAfterShake.x += shakeOffsetX;
        camPositionAfterShake.y += shakeOffsetY;

        mainCamera.transform.position = camPositionAfterShake;
    }

    private void StopShaking() {
        CancelInvoke("Shake");
        mainCamera.transform.localPosition = DEFAULT_CAMERA_POS;
    }

}
