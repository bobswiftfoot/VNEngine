using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(Character))]
public class CharacterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Character myCharacter = (Character)target;

        EditorGUIUtility.labelWidth = 100;
        myCharacter.gameObject.name = myCharacter.CharacterName = EditorGUILayout.TextField("Name: ", myCharacter.CharacterName);

        GUILayout.BeginHorizontal();
        myCharacter.FarLeftPosition = EditorGUILayout.Vector2Field("FarLeft Position: ", myCharacter.FarLeftPosition, GUILayout.MaxWidth(275));
        if (GUILayout.Button("Show Position", GUILayout.Width(100)))
        {
            myCharacter.gameObject.transform.position = new Vector3(myCharacter.FarLeftPosition.x, myCharacter.FarLeftPosition.y);
        }
        if (GUILayout.Button("Use Current Position", GUILayout.Width(150)))
        {
            myCharacter.FarLeftPosition = new Vector2(myCharacter.gameObject.transform.position.x, myCharacter.gameObject.transform.position.y);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        myCharacter.LeftPosition = EditorGUILayout.Vector2Field("Left Position: ", myCharacter.LeftPosition, GUILayout.MaxWidth(275));
        if (GUILayout.Button("Show Position", GUILayout.Width(100)))
        {
            myCharacter.gameObject.transform.position = new Vector3(myCharacter.LeftPosition.x, myCharacter.LeftPosition.y);
        }
        if (GUILayout.Button("Use Current Position", GUILayout.Width(150)))
        {
            myCharacter.LeftPosition = new Vector2(myCharacter.gameObject.transform.position.x, myCharacter.gameObject.transform.position.y);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        myCharacter.CenterPosition = EditorGUILayout.Vector2Field("Center Position: ", myCharacter.CenterPosition, GUILayout.MaxWidth(275));
        if (GUILayout.Button("Show Position", GUILayout.Width(100)))
        {
            myCharacter.gameObject.transform.position = new Vector3(myCharacter.CenterPosition.x, myCharacter.CenterPosition.y);
        }
        if (GUILayout.Button("Use Current Position", GUILayout.Width(150)))
        {
            myCharacter.CenterPosition = new Vector2(myCharacter.gameObject.transform.position.x, myCharacter.gameObject.transform.position.y);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        myCharacter.RightPosition = EditorGUILayout.Vector2Field("Right Position: ", myCharacter.RightPosition, GUILayout.MaxWidth(275));
        if (GUILayout.Button("Show Position", GUILayout.Width(100)))
        {
            myCharacter.gameObject.transform.position = new Vector3(myCharacter.RightPosition.x, myCharacter.RightPosition.y);
        }
        if (GUILayout.Button("Use Current Position", GUILayout.Width(150)))
        {
            myCharacter.RightPosition = new Vector2(myCharacter.gameObject.transform.position.x, myCharacter.gameObject.transform.position.y);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        myCharacter.FarRightPosition = EditorGUILayout.Vector2Field("FarRight Position: ", myCharacter.FarRightPosition, GUILayout.MaxWidth(275));
        if (GUILayout.Button("Show Position", GUILayout.Width(100)))
        {
            myCharacter.gameObject.transform.position = new Vector3(myCharacter.FarRightPosition.x, myCharacter.FarRightPosition.y);
        }
        if (GUILayout.Button("Use Current Position", GUILayout.Width(150)))
        {
            myCharacter.FarRightPosition = new Vector2(myCharacter.gameObject.transform.position.x, myCharacter.gameObject.transform.position.y);
        }
        GUILayout.EndHorizontal();


        EditorUtility.SetDirty(target);
        if (!EditorApplication.isPlaying)
            EditorSceneManager.MarkAllScenesDirty();
    }
}
