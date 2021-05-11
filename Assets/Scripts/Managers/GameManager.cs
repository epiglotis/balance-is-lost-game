using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Camera mainCamera;
    public GameObject terraPrefab;
    public Grid gridObject;
    public GameObject fireThrow;
    public AudioClip EarthSFX;
    public AudioClip WaterSFX;
    public AudioClip FireSFX;
    public AudioClip AirSFX;


    [HideInInspector]
    public AudioSource source;

    public PlayerController player;
    public GameObject gameOverPanel;
    public GameObject cutscenePanel;
    void Awake() {
        if (!instance)
        {
            instance = this;

        }
        else
            Destroy(gameObject);
        source = GetComponent<AudioSource>();
        source.clip = EarthSFX;
        source.Play();
    }

    public void GameFinish() {
        player.OlmeSFXCal();
        print("Game end");
        gameOverPanel.SetActive(true);
    }
 

}
