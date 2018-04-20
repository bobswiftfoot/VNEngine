using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[Serializable]
public class Path : MonoBehaviour
{
    public string PathName;
    public List<PathOptions> PathOptions;
    public List<Questions> QuestionLines;

    public Path()
    {
        PathName = "";
        PathOptions = new List<PathOptions>();
        QuestionLines = new List<Questions>();
    }

    private void OnDestroy()
    {
        if (gameObject.GetComponentInParent<Scene>())
        {
            (gameObject.GetComponentInParent<Scene>()).Paths.Remove(this);
        }
    }
}

[Serializable]
public class PathOptions
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
        Right,
        FarRight
    }

    public enum CharacterOrientation
    {
        Normal,
        Flipped
    }

    //Dialogue 
    public PathOptionsType Type;
    public Character Character;
    public int CharacterIndex;
    public string DialogueLine;
    [SerializeField]
    public AudioClip DialogueAudio;

    //Character Move
    public CharacterAction Action;
    public CharacterDirection Direction;
    public CharaterPosition Position;
    public CharacterOrientation Orientation;

    //Art
    public Sprite FullscreenArt;

    public PathOptions(PathOptionsType type)
    {
        Type = type;
    }
}

[Serializable]
public class Questions
{
    public string Response;
    public int LeadsToScene;
    public int LeadsToPath;
}



