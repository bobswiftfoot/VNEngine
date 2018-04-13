using System;
using System.Collections.Generic;
using UnityEngine;

public class Backgrounds : MonoBehaviour
{
    public static Backgrounds _backgrounds;
    public static Backgrounds Instance
    {
        get
        {
            if (_backgrounds == null)
                _backgrounds = (Backgrounds)FindObjectOfType(typeof(Backgrounds));
            return _backgrounds;
        }
    }

    [Serializable]
    public class Background
    {
        [SerializeField]
        public String BackgroundName;
        [SerializeField]
        public Sprite BackgroundSprite;
    }

    public List<Background> BackgroundList;
    SpriteRenderer spriteRenderer;

    public List<String> GetNames()
    {
        List<String> Names = new List<string>();

        foreach (Background background in BackgroundList)
        {
            Names.Add(background.BackgroundName);
        }
        return Names;
    }

    public void SetCurrentBackground(int backgroundIndex)
    {
        spriteRenderer.sprite = BackgroundList[backgroundIndex].BackgroundSprite;
    }

    private void Awake()
    {
        if (_backgrounds != null && _backgrounds != this)
            Destroy(_backgrounds);
        else
            _backgrounds = this;

        spriteRenderer =(SpriteRenderer)GetComponent(typeof(SpriteRenderer));
    }

}
