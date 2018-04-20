using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(Characters))]
public class CharactersEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Characters myCharacters = (Characters)target;

        for (int i = 0; i < myCharacters.CharacterList.Count; i++)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Character " + (i + 1) + ": " + myCharacters.CharacterList[i].CharacterName);
            if (GUILayout.Button("Delete Character"))
            {
                foreach (Character c in myCharacters.gameObject.GetComponentsInChildren<Character>())
                {
                    if (c.Equals(myCharacters.CharacterList[i]))
                    {
                        DestroyImmediate(c.gameObject);
                        break;
                    }
                }
                break;
            }
            GUILayout.EndHorizontal();
        }
        if (GUILayout.Button("Add Character"))
        {
            GameObject newObject = new GameObject();
            Character newCharacter = newObject.AddComponent<Character>();
            newObject.AddComponent<SpriteRenderer>();
            newObject.name = newCharacter.CharacterName = "Character: " + (myCharacters.CharacterList.Count + 1);
            myCharacters.CharacterList.Add(newCharacter);
            GameObjectUtility.SetParentAndAlign(newObject, myCharacters.gameObject);
            EditorUtility.SetDirty(newObject);
        }
        EditorUtility.SetDirty(target);
        if (!EditorApplication.isPlaying)
            EditorSceneManager.MarkAllScenesDirty();
    }
}
