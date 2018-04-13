using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Scene
{
    public string Name;
    public int BackgroundIndex;
    public string Background;
    [SerializeField]
    public AudioClip Music;

    public List<Path> Paths;

    public Scene()
    {
        Name = "";
        BackgroundIndex = 0;
        Background = "";
        Music = null;
        Paths = new List<Path>();
    }
}
