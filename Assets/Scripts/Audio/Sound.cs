using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound {
    public string name;
    public AudioClip clip;

    [HideInInspector]
    public AudioSource source;  // Set by AudioManager

    [Range(0, 1f)]
    public float volume = 1f;
    [Range(0, 3f)]
    public float pitch = 1f;
    public bool loop = false;
}
