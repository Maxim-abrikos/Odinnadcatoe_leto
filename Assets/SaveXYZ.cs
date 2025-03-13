using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveXYZ : MonoBehaviour
{
    public int target = 30;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target;
    }

    void Update()
    {
        if (Application.targetFrameRate != target)
            Application.targetFrameRate = target;
    }
    //private Transform playerTransform;

    //void Start()
    //{
    //    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    //    DontDestroyOnLoad(this.gameObject);
    //}

    //void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    //void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    if (scene.buildIndex != 0) // Check if not loading the initial scene
    //    {
    //        playerTransform.position = new Vector3(PlayerPrefs.GetFloat("PlayerPosX"), PlayerPrefs.GetFloat("PlayerPosY"), PlayerPrefs.GetFloat("PlayerPosZ"));
    //    }
    //}

    //public void SavePlayerPosition()
    //{
    //    PlayerPrefs.SetFloat("PlayerPosX", playerTransform.position.x);
    //    PlayerPrefs.SetFloat("PlayerPosY", playerTransform.position.y);
    //    PlayerPrefs.SetFloat("PlayerPosZ", playerTransform.position.z);
    //}
}
