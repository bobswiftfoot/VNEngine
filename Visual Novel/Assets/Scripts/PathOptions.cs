using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PathOptions : MonoBehaviour
{
    public enum PathOptionsType
    {
        Dialogue,
        CharacterMove,
        FullScreenArt
    }

    public enum CharacterAction
    {
        Enter,
        Exit,
        Start,
    }

    public enum CharacterDirection
    {
        Left,
        Right,
        Top,
        Bottom
    }

    public enum CharaterPosition
    {
        FarLeft,
        Left,
        Center,
        Right,
        FarRight
    }

    public enum CharacterOrientation
    {
        Normal,
        Flipped
    }

    public enum CharacterMultipleMovementType
    {
        AllAtOnce,
        OneAtATime,
    }

    //Dialogue 
    public PathOptionsType Type;
    public Character Character;
    public string CustomCharacterName;
    public int CharacterIndex;
    public string DialogueLine;
    [SerializeField]
    public AudioClip DialogueAudio;

    //Character Move
    [Serializable]
    public class CharacterMovement
    {
        public CharacterAction Action;
        public CharacterDirection Direction;
        public CharaterPosition Position;
        public CharacterOrientation Orientation;
        public float MovementSpeed = 5.0f;
        public Character Character;
        public int CharacterIndex;
    } 
    public List<CharacterMovement> MultipleCharacters;
    public CharacterMultipleMovementType MultipleMovementType;

    //Art
    public Sprite FullscreenArt;

    public void Initialize()
    {
        MultipleCharacters = new List<CharacterMovement>();
        Type = PathOptionsType.Dialogue;
        Character = Characters.Instance.CharacterList[0];
        CharacterIndex = 0;
        gameObject.name = Character.CharacterName + ": ";
    }

    private void OnDestroy()
    {
        if (gameObject.GetComponentInParent<Path>())
        {
            (gameObject.GetComponentInParent<Path>()).PathOptions.Remove(this);
        }
    }
}