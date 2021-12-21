using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class AssetBundleLoader
{
    public string[] imageName;
    public string assetName;
    public Dictionary<string, Sprite> sprites;


    public void LoadAssets()
    {
        string path = Application.streamingAssetsPath + "/" + assetName + ".assetbundle";
        AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
        sprites = new Dictionary<string, Sprite>();
        foreach (string name in imageName)
        {
            sprites.Add(name, assetBundle.LoadAsset<Sprite>(name));
        }
    }
}