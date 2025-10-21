using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    private string sceneName = "level";
    public string SceneName
    {
        get => PlayerPrefs.GetString(sceneName);
    }
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.Instance.TransitionToMain();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
        }
    }

    public void SavePlayerData()
    {
        if (GameManager.Instance.playerStates != null && GameManager.Instance.playerStates.characterData
            != null)
            Save(GameManager.Instance.playerStates.characterData,GameManager.Instance.playerStates.characterData.name);
    }

    public void LoadPlayerData()
    {
        if (GameManager.Instance.playerStates != null && GameManager.Instance.playerStates.characterData
            != null)
            Load(GameManager.Instance.playerStates.characterData,GameManager.Instance.playerStates.characterData.name);
    }
    public void Save(object data,string key)
    {
        var jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key,jsonData);
        PlayerPrefs.SetString(sceneName,SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }

    public void Load(object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key),data);
        }
    }
}
