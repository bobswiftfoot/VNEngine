using System;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    public static Characters _characters;
    public static Characters Instance
    {
        get
        {
            if (_characters == null)
                _characters = (Characters)FindObjectOfType(typeof(Characters));
            return _characters;
        }
    }

    public List<Character> CharacterList;

    public List<String> GetNames()
    {
        List<String> Names = new List<string>();

        foreach (Character character in CharacterList)
        {
            Names.Add(character.CharacterName);
        }
        return Names;
    }

    public void HideCharacters()
    {
        foreach(Character character in CharacterList)
        {
            character.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        if (_characters != null && _characters != this)
            Destroy(_characters);
        else
            _characters = this;
    }
}

[ExecuteInEditMode]
public class Character : MonoBehaviour
{
    public String CharacterName;
    public Vector2 FarLeftPosition;
    public Vector2 LeftPosition;
    public Vector2 CenterPosition;
    public Vector2 RightPosition;
    public Vector2 FarRightPosition;

    private void OnDestroy()
    {
        if (gameObject.GetComponentInParent<Characters>())
        {
            (gameObject.GetComponentInParent<Characters>()).CharacterList.Remove(this);
        }
    }
}
