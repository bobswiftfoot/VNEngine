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
            GUILayout.BeginHorizontal();

            EditorStyles.label.wordWrap = true;
            string name = myPath.PathOptions[d].gameObject.name;
            float width = EditorGUIUtility.currentViewWidth - 115;
            float height = EditorStyles.label.CalcHeight(new GUIContent(name), width);
            EditorGUILayout.LabelField(name, GUILayout.Width(width), GUILayout.Height(height));
            EditorStyles.label.wordWrap = false;

            if (GUILayout.Button("Delete", GUILayout.Width(80)))
            {
                foreach (PathOptions pathoption in myPath.gameObject.GetComponentsInChildren<PathOptions>())
                {
                    if (pathoption.Equals(myPath.PathOptions[d]))
                    {
                        myPath.PathOptions.Remove(pathoption);
                        DestroyImmediate(pathoption.gameObject);
                        break;
                    }
                }
                break;
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Path Option"))
        {
            GameObject newObject = new GameObject();
            PathOptions newPathOption = newObject.AddComponent<PathOptions>();
            newPathOption.Initialize();
            myPath.PathOptions.Add(newPathOption);
            GameObjectUtility.SetParentAndAlign(newObject, myPath.gameObject);
            EditorUtility.SetDirty(newObject);
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
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(80);
                int index = Helper.GetIndexOfPathList(Conversation.Instance, myPath.QuestionLines[q].LeadsToScene, myPath.QuestionLines[q].LeadsToPath);
                EditorGUIUtility.labelWidth = 60;
                index = EditorGUILayout.Popup("Leads to: ", index, Helper.CreatePathList(Conversation.Instance).ToArray());
                EditorGUIUtility.labelWidth = 134;
                myPath.QuestionLines[q].LeadsToScene = Helper.GetSceneFromIndex(Conversation.Instance, index);
                myPath.QuestionLines[q].LeadsToPath = Helper.GetPathFromIndex(Conversation.Instance, index);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(80);
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
