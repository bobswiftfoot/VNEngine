using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(Path))]
public class PathEditor : Editor
{
    private static bool myQuestionFoldout;

    public override void OnInspectorGUI()
    {
        Path myPath = (Path)target;

        EditorGUIUtility.labelWidth = 75;
        myPath.PathName = EditorGUILayout.TextField("Path Name", myPath.PathName);
        myPath.gameObject.name = myPath.PathName;

        for (int d = 0; d < myPath.PathOptions.Count; d++)
        {
            if (myPath.PathOptions[d].Type == PathOptions.PathOptionsType.Dialogue)
            {
                GUILayout.BeginHorizontal();
                myPath.PathOptions[d].CharacterIndex = EditorGUILayout.Popup(myPath.PathOptions[d].CharacterIndex, Characters.Instance.GetNames().ToArray(), GUILayout.Width(70));
                myPath.PathOptions[d].Character = Characters.Instance.CharacterList[myPath.PathOptions[d].CharacterIndex];

                EditorStyles.textField.wordWrap = true;
                float height = EditorStyles.textField.CalcHeight(new GUIContent(myPath.PathOptions[d].DialogueLine), 300);
                myPath.PathOptions[d].DialogueLine = EditorGUILayout.TextField(myPath.PathOptions[d].DialogueLine, GUILayout.Width(300), GUILayout.Height(height));
                EditorStyles.textField.wordWrap = false;

                EditorGUIUtility.labelWidth = 40;
                myPath.PathOptions[d].DialogueAudio = (AudioClip)EditorGUILayout.ObjectField("Clip: ", myPath.PathOptions[d].DialogueAudio, typeof(AudioClip), true);
                EditorGUIUtility.labelWidth = 134;

                if (GUILayout.Button("Delete", GUILayout.Width(80)))
                {
                    myPath.PathOptions.Remove(myPath.PathOptions[d]);
                    break;
                }
                GUILayout.EndHorizontal();
            }
            if (myPath.PathOptions[d].Type == PathOptions.PathOptionsType.CharacterMove)
            {
                GUILayout.BeginHorizontal();
                myPath.PathOptions[d].CharacterIndex = EditorGUILayout.Popup(myPath.PathOptions[d].CharacterIndex, Characters.Instance.GetNames().ToArray(), GUILayout.Width(70));
                myPath.PathOptions[d].Character = Characters.Instance.CharacterList[myPath.PathOptions[d].CharacterIndex];

                myPath.PathOptions[d].Action = (PathOptions.CharacterAction)EditorGUILayout.Popup((int)myPath.PathOptions[d].Action, Enum.GetNames(typeof(PathOptions.CharacterAction)), GUILayout.Width(70));
                string label = "";
                if (myPath.PathOptions[d].Action == PathOptions.CharacterAction.Enter)
                {
                    EditorGUIUtility.labelWidth = 40;
                    label = "from";
                }
                else
                {
                    EditorGUIUtility.labelWidth = 20;
                    label = "to";
                }
                myPath.PathOptions[d].Direction = (PathOptions.CharacterDirection)EditorGUILayout.Popup(label, (int)myPath.PathOptions[d].Direction, Enum.GetNames(typeof(PathOptions.CharacterDirection)), GUILayout.Width(100));

                if (myPath.PathOptions[d].Action == PathOptions.CharacterAction.Enter)
                {
                    EditorGUIUtility.labelWidth = 55;
                    myPath.PathOptions[d].Position = (PathOptions.CharaterPosition)EditorGUILayout.Popup("Position:", (int)myPath.PathOptions[d].Position, Enum.GetNames(typeof(PathOptions.CharaterPosition)), GUILayout.Width(125));
                    EditorGUIUtility.labelWidth = 75;
                    myPath.PathOptions[d].Orientation = (PathOptions.CharacterOrientation)EditorGUILayout.Popup("Orientation:", (int)myPath.PathOptions[d].Orientation, Enum.GetNames(typeof(PathOptions.CharacterOrientation)), GUILayout.Width(135));
                }
                EditorGUILayout.Space();
                if (GUILayout.Button("Delete", GUILayout.Width(80)))
                {
                    myPath.PathOptions.Remove(myPath.PathOptions[d]);
                    break;
                }

                GUILayout.EndHorizontal();
            }
            if (myPath.PathOptions[d].Type == PathOptions.PathOptionsType.FullScreenArt)
            {
                GUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 90;
                myPath.PathOptions[d].FullscreenArt = (Sprite)EditorGUILayout.ObjectField("Fullscreen Art: ", myPath.PathOptions[d].FullscreenArt, typeof(Sprite), true, GUILayout.MaxHeight(16));
                if (GUILayout.Button("Delete", GUILayout.Width(80)))
                {
                    myPath.PathOptions.Remove(myPath.PathOptions[d]);
                    break;
                }
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Line"))
        {
            myPath.PathOptions.Add(new PathOptions(PathOptions.PathOptionsType.Dialogue));
        }
        if (GUILayout.Button("Add Character Movement"))
        {
            myPath.PathOptions.Add(new PathOptions(PathOptions.PathOptionsType.CharacterMove));
        }
        if (GUILayout.Button("Add Fullscreen Art"))
        {
            myPath.PathOptions.Add(new PathOptions(PathOptions.PathOptionsType.FullScreenArt));
        }
        GUILayout.EndHorizontal();

        myQuestionFoldout = EditorGUILayout.Foldout(myQuestionFoldout, "Question");
        if (myQuestionFoldout)
        {
            for (int q = 0; q < myPath.QuestionLines.Count; q++)
            {
                GUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 80;
                EditorStyles.textField.wordWrap = true;
                float height = EditorStyles.textField.CalcHeight(new GUIContent(myPath.QuestionLines[q].Response), 300);
                myPath.QuestionLines[q].Response = EditorGUILayout.TextField("Response: " + (q + 1), myPath.QuestionLines[q].Response, GUILayout.Width(375), GUILayout.Height(height));
                EditorStyles.textField.wordWrap = false;

                int index = Helper.GetIndexOfPathList(Conversation.Instance, myPath.QuestionLines[q].LeadsToScene, myPath.QuestionLines[q].LeadsToPath);
                EditorGUIUtility.labelWidth = 60;
                index = EditorGUILayout.Popup("Leads to: ", index, Helper.CreatePathList(Conversation.Instance).ToArray());
                EditorGUIUtility.labelWidth = 134;
                myPath.QuestionLines[q].LeadsToScene = Helper.GetSceneFromIndex(Conversation.Instance, index);
                myPath.QuestionLines[q].LeadsToPath = Helper.GetPathFromIndex(Conversation.Instance, index);

                if (GUILayout.Button("Delete", GUILayout.Width(80)))
                {
                    myPath.QuestionLines.RemoveAt(myPath.QuestionLines.Count - 1);
                    break;
                }
                GUILayout.EndHorizontal(); 
            }

            if (GUILayout.Button("Add Responce"))
            {
                myPath.QuestionLines.Add(new Questions());
            }
        }

        EditorUtility.SetDirty(target);
        if (!EditorApplication.isPlaying)
            EditorSceneManager.MarkAllScenesDirty();
    }
}
