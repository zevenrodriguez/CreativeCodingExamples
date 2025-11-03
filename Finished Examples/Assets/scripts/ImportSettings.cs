using UnityEngine;
using System;


[Serializable]
public class Setting
{
    public string name;
    public string value;
}

[Serializable]
class SettingArray
{
    public Setting[] settings;
}

public class ImportSettings : MonoBehaviour
{
    [SerializeField] TextAsset file;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SettingArray settingOptions = JsonUtility.FromJson<SettingArray>(file.text);
        foreach(var item in settingOptions.settings)
        {
            Debug.Log(item.name + " " + item.value);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
