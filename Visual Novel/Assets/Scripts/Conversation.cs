using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    public static Conversation _conversation;
    public static Conversation Instance
    {
        get
        {
            if (_conversation == null)
                _conversation = (Conversation)FindObjectOfType(typeof(Conversation));
            return _conversation;
        }
    }

    private void Awake()
    {
        if (_conversation != null && _conversation != this)
            Destroy(_conversation);
        else
            _conversation = this;
    }

    private void Reset()
    {
        if(FindObjectsOfType(typeof(Conversation)).Length > 1)
        {
            Debug.LogError("There is already a Conversation in scene");
        }
    }

    public List<Scene> Scenes;

    public Conversation()
    {
        Scenes = new List<Scene>();
    }
}
