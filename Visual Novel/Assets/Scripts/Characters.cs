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

    [Serializable]
    public class Character
    {
        [SerializeField]
        public String CharacterName;
        [SerializeField]
        public Sprite CharacterSprite;
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

    private void Awake()
    {
        if (_characters != null && _characters != this)
            Destroy(_characters);
        else
            _characters = this;
    }

}
