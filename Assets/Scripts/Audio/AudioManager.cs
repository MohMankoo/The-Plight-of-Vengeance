using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    private static string currentlyPlayingOST;

    // Sound arrays set in Inspector
    public Sound[] effects;
    public Sound[] playerGuns;
    public Sound[] enemyGuns;
    public Sound[] voices;
    public Sound[] hits;
    public Sound[] OST;

    // Sound Dictionary mappings
    private static Dictionary<string, Sound> effectsDic;
    private static Dictionary<string, Sound> playerGunsDic;
    private static Dictionary<string, Sound> enemyGunsDic;
    private static Dictionary<string, Sound> voicesDic;
    private static Dictionary<string, Sound> hitsDic;
    private static Dictionary<string, Sound> OSTDic;

    //                  Basic functionality

    void Awake () {
        // Allow only one AudioManager instance
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        // Keep the object alive when changing scenes.
        DontDestroyOnLoad(gameObject);
        
        // Create Sound dictionary mappings.
        effectsDic    = new Dictionary<string, Sound>();
        playerGunsDic = new Dictionary<string, Sound>();
        enemyGunsDic  = new Dictionary<string, Sound>();
        voicesDic     = new Dictionary<string, Sound>();
        hitsDic       = new Dictionary<string, Sound>();
        OSTDic        = new Dictionary<string, Sound>();
        populateDicWithSounds(effectsDic, effects);
        populateDicWithSounds(playerGunsDic, playerGuns);
        populateDicWithSounds(enemyGunsDic, enemyGuns);
        populateDicWithSounds(voicesDic, voices);
        populateDicWithSounds(hitsDic, hits);
        populateDicWithSounds(OSTDic, OST);
    }

    void Start () {
        currentlyPlayingOST = "CalmBeforeTheStorm";
        Play(OSTDic, "CalmBeforeTheStorm");
    }

    //                  Helpers

    // Create a dictionary mapping of Sound.name to its Sound for each Sound item.
    // Create an AudioSource for each Sound and instantiate it with values from the sounds array.
    void populateDicWithSounds (Dictionary<string, Sound> dic, Sound[] sounds) {
        foreach (Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.loop = sound.loop;
            dic[sound.name] = sound;
        }
    }

    // Play the sound given by soundName if it exists in the Sound Dictionary dic.
    private static void Play (Dictionary<string, Sound> dic, string soundName) {
        // Set volume and pitch before playing
        if (dic[soundName] != null) {
            dic[soundName].source.volume = dic[soundName].volume;
            dic[soundName].source.pitch = dic[soundName].source.pitch;
            dic[soundName].source.Play();
        } else {
            Debug.Log("Sound clip " + soundName + " was not found");
        }
    }

    // Play a random sound from the category given by soundCategory that may be found in the Dictionary dic.
    // upperRange describes the max number of sounds available for soundCategory in dic.
    private static void PlayRandom (Dictionary<string, Sound> dic, string soundCategory, int upperRange) {
        int soundIndex = Random.Range(1, upperRange + 1);
        Play(dic, soundCategory + soundIndex);
    }

    //                  Interacting with Manager

    public static void PlayEffect (string effect) {
        Play(effectsDic, effect);
    }

    public static void PlayPlayerGunSound (string gunCategory) {
        PlayRandom(playerGunsDic, gunCategory, 4);
    }

    public static void PlayEnemyGunSound (string enemyName) {
        PlayRandom(enemyGunsDic, "SS10-" + enemyName, 4);
    }

    public static void PlayVoice (string soundCategory) {
        if (soundCategory == "player") {
            Play(voicesDic, soundCategory);
        } else {
            PlayRandom(voicesDic, soundCategory, 6);
        }
    }

    public static void PlayHitSound () {
        PlayRandom(hitsDic, "hit", 4);
    }

    public static void SwitchOST (string OSTName) {
        // If there is an OST currently playing, stop it.
        if (currentlyPlayingOST != "" && currentlyPlayingOST != null)
            OSTDic[currentlyPlayingOST].source.Stop();
        else
            Debug.Log("Note: No OST named " + OSTName + " was playing");

        currentlyPlayingOST = OSTName;
        Play(OSTDic, OSTName);
    }
}
