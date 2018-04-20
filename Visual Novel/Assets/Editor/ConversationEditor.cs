using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Conversation))]
public class ConversationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Conversation myConversation = (Conversation)target;

        //Display Scene
        for (int i = 0; i < myConversation.Scenes.Count; i++)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Scene " + (i+1) + ": " + myConversation.Scenes[i].Name);
            if (GUILayout.Button("Delete Scene"))
            {
                foreach (Scene s in myConversation.gameObject.GetComponentsInChildren<Scene>())
                {
                    if (s.Equals(myConversation.Scenes[i]))
                    {
                        DestroyImmediate(s.gameObject);
                        break;
                    }
                }
                break;
            }
            GUILayout.EndHorizontal();
        }
        if (GUILayout.Button("Add Scene"))
        {
            GameObject newObject = new GameObject();
            Scene newScene = newObject.AddComponent<Scene>();
            newObject.name = newScene.Name = "Scene: " + (myConversation.Scenes.Count + 1);
            myConversation.Scenes.Add(newScene);
            GameObjectUtility.SetParentAndAlign(newObject, myConversation.gameObject);
            EditorUtility.SetDirty(newObject);
        }
        EditorUtility.SetDirty(target);
        if(!EditorApplication.isPlaying)
            EditorSceneManager.MarkAllScenesDirty();
    }
}
