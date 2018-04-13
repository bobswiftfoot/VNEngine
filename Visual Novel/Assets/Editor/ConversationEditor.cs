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
    private static List<List<bool>> myQuestionFoldouts = new List<List<bool>>();

    public override void OnInspectorGUI()
    {
        Conversation myConversation = (Conversation)target;

        EditorGUIUtility.labelWidth = 134;
        int ListSize = EditorGUILayout.IntField("Scene Count: ", myConversation.Scenes.Count);
        //Make Sure scene list is correct size
        while (ListSize > myConversation.Scenes.Count)
        {
            myConversation.Scenes.Add(new Scene());
        }   
        while(ListSize < myConversation.Scenes.Count)
        {
            myConversation.Scenes.RemoveAt(myConversation.Scenes.Count - 1);
        }

        //Make sure foldouts are correct size
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

        while (ListSize > myQuestionFoldouts.Count)
        {
            myQuestionFoldouts.Add(new List<bool>());
        }
        while (ListSize < myQuestionFoldouts.Count)
        {
            myQuestionFoldouts.RemoveAt(myQuestionFoldouts.Count - 1);
        }

        //Display Scene
        for (int i = 0; i < myConversation.Scenes.Count; i++)
        {
            GUILayout.BeginVertical();
            mySceneFoldouts[i] = EditorGUILayout.Foldout(mySceneFoldouts[i], "Scene " + (i+1) + ": " + myConversation.Scenes[i].Name);
            if (mySceneFoldouts[i])
            {
                myConversation.Scenes[i].Name = EditorGUILayout.TextField("Scene Name: ", myConversation.Scenes[i].Name);

                myConversation.Scenes[i].BackgroundIndex = EditorGUILayout.Popup("Background: ", myConversation.Scenes[i].BackgroundIndex, Backgrounds.Instance.GetNames().ToArray());
                myConversation.Scenes[i].Background = Backgrounds.Instance.GetNames()[myConversation.Scenes[i].BackgroundIndex];

                myConversation.Scenes[i].Music = (AudioClip)EditorGUILayout.ObjectField("Music", myConversation.Scenes[i].Music, typeof(AudioClip), true);

                //Display Paths
                int PathListSize = EditorGUILayout.IntField("Path Count: ", myConversation.Scenes[i].Paths.Count);
                //Make sure Path List is correct
                while (PathListSize > myConversation.Scenes[i].Paths.Count)
                {
                    myConversation.Scenes[i].Paths.Add(new Path());
                }
                while (PathListSize < myConversation.Scenes[i].Paths.Count)
                {
                    myConversation.Scenes[i].Paths.RemoveAt(myConversation.Scenes[i].Paths.Count - 1);
                }

                //Make sure Path and Question foldouts are correct
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

                for (int p = 0; p < myConversation.Scenes[i].Paths.Count; p++)
                {
                    myPathFoldouts[i][p] = EditorGUILayout.Foldout(myPathFoldouts[i][p], "Path " + (p + 1) + ": " + myConversation.Scenes[i].Paths[p].PathName);
                    if (myPathFoldouts[i][p])
                    {
                        myConversation.Scenes[i].Paths[p].PathName = EditorGUILayout.TextField("Path Name", myConversation.Scenes[i].Paths[p].PathName);

                        //Dialogue Count
                        int DialogueListSize = EditorGUILayout.IntField("Dialogue Count: ", myConversation.Scenes[i].Paths[p].DialogueLines.Count);
                        //Make sure Dialogue List is correct
                        while (DialogueListSize > myConversation.Scenes[i].Paths[p].DialogueLines.Count)
                        {
                            myConversation.Scenes[i].Paths[p].DialogueLines.Add(new Dialogue());
                        }
                        while (DialogueListSize < myConversation.Scenes[i].Paths[p].DialogueLines.Count)
                        {
                            myConversation.Scenes[i].Paths[p].DialogueLines.RemoveAt(myConversation.Scenes[i].Paths[p].DialogueLines.Count - 1);
                        }
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
                            GUILayout.EndHorizontal();
                        }

                        myQuestionFoldouts[i][p] = EditorGUILayout.Foldout(myQuestionFoldouts[i][p], "Questions");
                        if (myQuestionFoldouts[i][p])
                        {
                            //Question Count
                            int QuestionListSize = EditorGUILayout.IntField("Question Count: ", myConversation.Scenes[i].Paths[p].QuestionLines.Count);
                            //Make sure Question List is correct
                            while (QuestionListSize > myConversation.Scenes[i].Paths[p].QuestionLines.Count)
                            {
                                myConversation.Scenes[i].Paths[p].QuestionLines.Add(new Questions());
                            }
                            while (QuestionListSize < myConversation.Scenes[i].Paths[p].QuestionLines.Count)
                            {
                                myConversation.Scenes[i].Paths[p].QuestionLines.RemoveAt(myConversation.Scenes[i].Paths[p].QuestionLines.Count - 1);
                            }

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
                                GUILayout.EndHorizontal();
                            }
                        }
                    }
                }
            }
            GUILayout.EndVertical();
        }
        EditorUtility.SetDirty(target);
        if(!EditorApplication.isPlaying)
            EditorSceneManager.MarkAllScenesDirty();
    }
}
