using System.Collections.Generic;
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

    public List<Scene> Scenes;
}
