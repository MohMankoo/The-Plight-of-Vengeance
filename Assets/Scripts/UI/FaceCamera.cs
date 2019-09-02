﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

    void Update() {
        Camera mainCamera = Camera.main;
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                         mainCamera.transform.rotation * Vector3.up);
    }
}
