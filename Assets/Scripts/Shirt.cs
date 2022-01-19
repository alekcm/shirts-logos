using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shirt : MonoBehaviour
{
    public SpriteRenderer[] ShirtLayers;

    public Color[] colors;
    public string ShirtName;
    public GameObject hanger;
    public class ShirtSave
    {
        public Color[] colors;
    }
    // Start is called before the first frame update
    void Start()
    {
        ShirtSave save = new ShirtSave();
        if (PlayerPrefs.HasKey(ShirtName))
        {
            save = JsonUtility.FromJson<ShirtSave>(PlayerPrefs.GetString(ShirtName));
        }
        else
        {
            save.colors = colors;
            PlayerPrefs.SetString(ShirtName, JsonUtility.ToJson(save));
        }

        ShirtManager manager = GameObject.Find("ShirtManager").GetComponent<ShirtManager>();
        manager.CurrentShirt = this;
        manager.ShirtLayers = ShirtLayers;
        for (int i = 0; i < 3; i++)
        {
            manager.SetColor(save.colors[i], i);
        }

        ShirtManager SM = GameObject.Find("ShirtManager").GetComponent<ShirtManager>();
        for (int i = 0; i < 3; i ++)
            if (SM.LayerButtons[i].IsActive)
                SM.LayerButtons[i].SetActive();
    }

    public void SaveColors()
    {
        ShirtSave save = new ShirtSave();
        save.colors = colors;
        PlayerPrefs.SetString(ShirtName, JsonUtility.ToJson(save));
    }
    private void OnApplicationQuit()
    {
        SaveColors();
    }

    private void OnDestroy()
    {
        SaveColors();
    }
}
