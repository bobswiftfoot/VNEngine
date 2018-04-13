using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Path
{
    public string PathName;
    public List<Dialogue> DialogueLines;
    public List<Questions> QuestionLines;

    public Path()
    {
        PathName = "";
        DialogueLines = new List<Dialogue>();
        QuestionLines = new List<Questions>();
    }
}

[Serializable]
public class Dialogue
{
    public string Character;
    public int CharacterIndex;
    public string DialogueLine;
    [SerializeField]
    public AudioClip DialogueAudio;
}

[Serializable]
public class Questions
{
    public string Response;
    public int LeadsToScene;
    public int LeadsToPath;
}



