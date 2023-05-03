using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : SingleTon<SaveManager>
{
    bool isSave;
    bool isLoad;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
            isSave = true;
        if(Input.GetKeyDown(KeyCode.L))
            isLoad = true;
    }

    private void FixedUpdate()
    {
        if(isSave)
        {
            print(1111);
            SavePlayerData();
            isSave = false;
        }

        if (isLoad)
        {
            print(11112);
            LoadPlayerData();
            isLoad = false;
        }
    }

    public void SavePlayerData()
    {
        if(GameManager.Instance.playerData != null)
        {
            var data = GameManager.Instance.playerData.characterData;
            Save(data, data.name);
        }
    }

    public void LoadPlayerData()
    {
            var data = GameManager.Instance.playerData.characterData;
            Load(data, data.name);
    }

    public void Save(Object data,string key)
    {
        var jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.Save();
    }

    public void Load(Object data, string key)
    {
        if(PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }

}
