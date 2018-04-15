using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;

[CustomEditor(typeof(Conversation))]
public class ConversationEditor : Editor
{
    private static List<bool> mySceneFoldouts = new List<bool>();
    private static List<List<bool>> myPathFoldouts = new List<List<bool>>();
    private static List<List<bool>> myDialogueFoldouts = new List<List<bool>>();
    private static List<List<bool>> myQuestionFoldouts = new List<List<bool>>();

    public override void OnInspectorGUI()
    {
        Conversation myConversation = (Conversation)target;

        int ListSize = myConversation.Scenes.Count;
        //Make sure foldouts are correct size, This should only be hit on a recompile or first time loading Unity
        while (ListSize > mySceneFoldouts.Count)
        {
            mySceneFoldouts.Add(false);
        }
        while (ListSize < mySceneFoldouts.Count)
        {
            mySceneFoldouts.RemoveAt(mySceneFoldouts.Count - 1);
        }

        while (ListSize > myPathFoldouts.Count)
        {
            myPathFoldouts.Add(new List<bool>());
        }
        while (ListSize < myPathFoldouts.Count)
        {
            myPathFoldouts.RemoveAt(myPathFoldouts.Count - 1);
        }

        while (ListSize > myDialogueFoldouts.Count)
        {
            myDialogueFoldouts.Add(new List<bool>());
        }
        while (ListSize < myDialogueFoldouts.Count)
        {
            myDialogueFoldouts.RemoveAt(myDialogueFoldouts.Count - 1);
        }

        while (ListSize > myQuestionFoldouts.Count)
        {
            myQuestionFoldouts.Add(new List<bool>());
        }
        while (ListSize < myQuestionFoldouts.Count)
        {
            myQuestionFoldouts.RemoveAt(myQuestionFoldouts.Count - 1);
        }

        EditorGUIUtility.labelWidth = 134;
        //Display Scene
        for (int i = 0; i < myConversation.Scenes.Count; i++)
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            mySceneFoldouts[i] = EditorGUILayout.Foldout(mySceneFoldouts[i], "Scene " + (i+1) + ": " + myConversation.Scenes[i].Name);
            if(GUILayout.Button("Delete Scene"))
            {
                myConversation.Scenes.RemoveAt(myConversation.Scenes.Count - 1);
                mySceneFoldouts.RemoveAt(mySceneFoldouts.Count - 1);
                myPathFoldouts.RemoveAt(myPathFoldouts.Count - 1);
                myQuestionFoldouts.RemoveAt(myQuestionFoldouts.Count - 1);
                break;
            }
            GUILayout.EndHorizontal();

            if (mySceneFoldouts[i])
            {
                myConversation.Scenes[i].Name = EditorGUILayout.TextField("Scene Name: ", myConversation.Scenes[i].Name);

                myConversation.Scenes[i].BackgroundIndex = EditorGUILayout.Popup("Background: ", myConversation.Scenes[i].BackgroundIndex, Backgrounds.Instance.GetNames().ToArray());
                myConversation.Scenes[i].Background = Backgrounds.Instance.GetNames()[myConversation.Scenes[i].BackgroundIndex];

                myConversation.Scenes[i].Music = (AudioClip)EditorGUILayout.ObjectField("Music", myConversation.Scenes[i].Music, typeof(AudioClip), true);

                //Display Paths
                int PathListSize = myConversation.Scenes[i].Paths.Count;
                //Make sure Path and Question foldouts are correct, This should only be hit on a recompile or first time loading Unity
                while (PathListSize > myPathFoldouts[i].Count)
                {
                    myPathFoldouts[i].Add(false);
                }
                while (PathListSize < myPathFoldouts[i].Count)
                {
                    myPathFoldouts[i].RemoveAt(myPathFoldouts[i].Count - 1);
                }

                while (PathListSize > myQuestionFoldouts[i].Count)
                {
                    myQuestionFoldouts[i].Add(false);
                }
                while (PathListSize < myQuestionFoldouts[i].Count)
                {
                    myQuestionFoldouts[i].RemoveAt(myQuestionFoldouts[i].Count - 1);
                }

                while (PathListSize > myDialogueFoldouts[i].Count)
                {
                    myDialogueFoldouts[i].Add(false);
                }
                while (PathListSize < myDialogueFoldouts[i].Count)
                {
                    myDialogueFoldouts[i].RemoveAt(myDialogueFoldouts[i].Count - 1);
                }


                for (int p = 0; p < myConversation.Scenes[i].Paths.Count; p++)
                {
                    GUILayout.BeginHorizontal();
                    myPathFoldouts[i][p] = EditorGUILayout.Foldout(myPathFoldouts[i][p], "Path " + (p + 1) + ": " + myConversation.Scenes[i].Paths[p].PathName);
                    if (GUILayout.Button("Delete Path"))
                    {
                        myConversation.Scenes[i].Paths.RemoveAt(myConversation.Scenes[i].Paths.Count - 1);
                        myPathFoldouts[i].RemoveAt(myPathFoldouts[i].Count - 1);
                        myQuestionFoldouts[i].RemoveAt(myQuestionFoldouts[i].Count - 1);
                        break;
                    }
                    GUILayout.EndHorizontal();

                    if (myPathFoldouts[i][p])
                    {
                        myConversation.Scenes[i].Paths[p].PathName = EditorGUILayout.TextField("Path Name", myConversation.Scenes[i].Paths[p].PathName);

                        myDialogueFoldouts[i][p] = EditorGUILayout.Foldout(myDialogueFoldouts[i][p], "Dialogue Lines");
                        if (myDialogueFoldouts[i][p])
                        {
                            for (int d = 0; d < myConversation.Scenes[i].Paths[p].DialogueLines.Count; d++)
                            {
                                GUILayout.BeginHorizontal();
                                myConversation.Scenes[i].Paths[p].DialogueLines[d].CharacterIndex = EditorGUILayout.Popup(myConversation.Scenes[i].Paths[p].DialogueLines[d].CharacterIndex, Characters.Instance.GetNames().ToArray(), GUILayout.Width(130));
                                myConversation.Scenes[i].Paths[p].DialogueLines[d].Character = Characters.Instance.GetNames()[myConversation.Scenes[i].Paths[p].DialogueLines[d].CharacterIndex];

                                EditorStyles.textField.wordWrap = true;
                                float height = EditorStyles.textField.CalcHeight(new GUIContent(myConversation.Scenes[i].Paths[p].DialogueLines[d].DialogueLine), 300);
                                myConversation.Scenes[i].Paths[p].DialogueLines[d].DialogueLine = EditorGUILayout.TextField(myConversation.Scenes[i].Paths[p].DialogueLines[d].DialogueLine, GUILayout.Width(300), GUILayout.Height(height));
                                EditorStyles.textField.wordWrap = false;

                                EditorGUIUtility.labelWidth = 70;
                                myConversation.Scenes[i].Paths[p].DialogueLines[d].DialogueAudio = (AudioClip)EditorGUILayout.ObjectField("Voice Clip: ", myConversation.Scenes[i].Paths[p].DialogueLines[d].DialogueAudio, typeof(AudioClip), true);
                                EditorGUIUtility.labelWidth = 134;

                                if (GUILayout.Button("Delete"))
                                {
                                    myConversation.Scenes[i].Paths[p].DialogueLines.RemoveAt(myConversation.Scenes[i].Paths[p].DialogueLines.Count - 1);
                                    break;
                                }

                                GUILayout.EndHorizontal();
                            }

                            if (GUILayout.Button("Add Line"))
                            {
                                myConversation.Scenes[i].Paths[p].DialogueLines.Add(new Dialogue());
                                break;
                            }
                        }

                        myQuestionFoldouts[i][p] = EditorGUILayout.Foldout(myQuestionFoldouts[i][p], "Question");
                        if (myQuestionFoldouts[i][p])
                        {
                            for (int q = 0; q < myConversation.Scenes[i].Paths[p].QuestionLines.Count; q++)
                            {
                                GUILayout.BeginHorizontal();
                                EditorStyles.textField.wordWrap = true;
                                float height = EditorStyles.textField.CalcHeight(new GUIContent(myConversation.Scenes[i].Paths[p].QuestionLines[q].Response), 300);
                                myConversation.Scenes[i].Paths[p].QuestionLines[q].Response = EditorGUILayout.TextField("Response: " + (q + 1), myConversation.Scenes[i].Paths[p].QuestionLines[q].Response, GUILayout.Width(434), GUILayout.Height(height));
                                EditorStyles.textField.wordWrap = false;

                                int index = Helper.GetIndexOfPathList(myConversation, myConversation.Scenes[i].Paths[p].QuestionLines[q].LeadsToScene, myConversation.Scenes[i].Paths[p].QuestionLines[q].LeadsToPath);
                                EditorGUIUtility.labelWidth = 70;
                                index = EditorGUILayout.Popup("Leads to: ", index, Helper.CreatePathList(myConversation).ToArray());
                                EditorGUIUtility.labelWidth = 134;
                                myConversation.Scenes[i].Paths[p].QuestionLines[q].LeadsToScene = Helper.GetSceneFromIndex(myConversation, index);
                                myConversation.Scenes[i].Paths[p].QuestionLines[q].LeadsToPath = Helper.GetPathFromIndex(myConversation, index);

                                if (GUILayout.Button("Delete"))
                                {
                                    myConversation.Scenes[i].Paths[p].QuestionLines.RemoveAt(myConversation.Scenes[i].Paths[p].QuestionLines.Count - 1);
                                    break;
                                }
                                GUILayout.EndHorizontal();
                            }

                            if (GUILayout.Button("Add Responce"))
                            {
                                myConversation.Scenes[i].Paths[p].QuestionLines.Add(new Questions());
                                break;
                            }
                        }
                    }
                }

                if (GUILayout.Button("Add Path"))
                {
                    myConversation.Scenes[i].Paths.Add(new Path());
                    myPathFoldouts[i].Add(false);
                    myQuestionFoldouts[i].Add(false);
                }
            }
            GUILayout.EndVertical();
        }
        if (GUILayout.Button("Add Scene"))
        {
            myConversation.Scenes.Add(new Scene());
            mySceneFoldouts.Add(false);
            myPathFoldouts.Add(new List<bool>());
            myQuestionFoldouts.Add(new List<bool>());
        }
        EditorUtility.SetDirty(target);
        if(!EditorApplication.isPlaying)
            EditorSceneManager.MarkAllScenesDirty();
    }
}
