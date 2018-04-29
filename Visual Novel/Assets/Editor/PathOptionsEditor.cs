using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(PathOptions))]
public class PathOptionsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PathOptions myPathOption = (PathOptions)target;

        myPathOption.Type = (PathOptions.PathOptionsType)EditorGUILayout.Popup((int)myPathOption.Type, Enum.GetNames(typeof(PathOptions.PathOptionsType)), GUILayout.Width(100));

        switch (myPathOption.Type)
        {
            case PathOptions.PathOptionsType.Dialogue:
                {
                    string name = "";
                    GUILayout.BeginHorizontal();
                    List<String> Names = Characters.Instance.GetNames();
                    Names.Add("Custom Name");
                    myPathOption.CharacterIndex = EditorGUILayout.Popup(myPathOption.CharacterIndex, Names.ToArray(), GUILayout.Width(100));
                    if (myPathOption.CharacterIndex == Characters.Instance.CharacterList.Count)
                    {
                        //Custom Name
                        name = myPathOption.CustomCharacterName = EditorGUILayout.TextField(myPathOption.CustomCharacterName, GUILayout.Width(100));
                    }
                    else
                    {
                        myPathOption.Character = Characters.Instance.CharacterList[myPathOption.CharacterIndex];
                        name = myPathOption.Character.CharacterName;
                    }
                    GUILayout.EndHorizontal();

                    EditorStyles.textField.wordWrap = true;
                    float height = EditorStyles.textField.CalcHeight(new GUIContent(myPathOption.DialogueLine), EditorGUIUtility.currentViewWidth - 115);
                    EditorGUIUtility.labelWidth = 105;
                    myPathOption.DialogueLine = EditorGUILayout.TextField("Dialogue: ", myPathOption.DialogueLine, GUILayout.Height(height));
                    myPathOption.name = name + ": " + myPathOption.DialogueLine;
                    EditorStyles.textField.wordWrap = false;

                    myPathOption.DialogueAudio = (AudioClip)EditorGUILayout.ObjectField("Clip: ", myPathOption.DialogueAudio, typeof(AudioClip), true);
                    EditorGUIUtility.labelWidth = 0;
                }   
                break;
            case PathOptions.PathOptionsType.CharacterMove:
                {
                    GUILayout.BeginHorizontal();
                    if (myPathOption.MultipleCharacters == null)
                    {
                        myPathOption.MultipleCharacters = new List<PathOptions.CharacterMovement>();
                    }
                    if (myPathOption.MultipleCharacters.Count == 0)
                    {
                        myPathOption.MultipleCharacters.Add(new PathOptions.CharacterMovement());
                    }

                    List<String> Names = Characters.Instance.GetNames();
                    Names.Add("Multiple Characters");
                    myPathOption.CharacterIndex = EditorGUILayout.Popup(myPathOption.CharacterIndex, Names.ToArray(), GUILayout.Width(100));
                    if (myPathOption.CharacterIndex == Characters.Instance.CharacterList.Count)
                    {
                        myPathOption.MultipleMovementType = (PathOptions.CharacterMultipleMovementType)EditorGUILayout.Popup((int)myPathOption.MultipleMovementType, Enum.GetNames(typeof(PathOptions.CharacterMultipleMovementType)), GUILayout.Width(100));

                        GUILayout.EndHorizontal();

                        myPathOption.gameObject.name = "Multiple characters moving";

                        for(int i = 0; i < myPathOption.MultipleCharacters.Count; i++)
                        {
                            GUILayout.BeginHorizontal();
                            EditorGUI.indentLevel = 0;

                            myPathOption.MultipleCharacters[i].CharacterIndex = EditorGUILayout.Popup(myPathOption.MultipleCharacters[i].CharacterIndex, Characters.Instance.GetNames().ToArray(), GUILayout.Width(100));
                            myPathOption.MultipleCharacters[i].Character = Characters.Instance.CharacterList[myPathOption.MultipleCharacters[i].CharacterIndex];

                            myPathOption.MultipleCharacters[i].Action = (PathOptions.CharacterAction)EditorGUILayout.Popup((int)myPathOption.MultipleCharacters[i].Action, Enum.GetNames(typeof(PathOptions.CharacterAction)), GUILayout.Width(70));

                            string label = "";
                            if (myPathOption.MultipleCharacters[0].Action == PathOptions.CharacterAction.Enter)
                            {
                                EditorGUIUtility.labelWidth = 40;
                                label = "from";
                                myPathOption.MultipleCharacters[0].Direction = (PathOptions.CharacterDirection)EditorGUILayout.Popup(label, (int)myPathOption.MultipleCharacters[0].Direction, Enum.GetNames(typeof(PathOptions.CharacterDirection)), GUILayout.Width(100));
                            }
                            else if (myPathOption.MultipleCharacters[0].Action == PathOptions.CharacterAction.Exit)
                            {
                                EditorGUIUtility.labelWidth = 20;
                                label = "to";
                                myPathOption.MultipleCharacters[0].Direction = (PathOptions.CharacterDirection)EditorGUILayout.Popup(label, (int)myPathOption.MultipleCharacters[0].Direction, Enum.GetNames(typeof(PathOptions.CharacterDirection)), GUILayout.Width(100));
                            }
                            else if (myPathOption.MultipleCharacters[0].Action == PathOptions.CharacterAction.Start)
                            {
                                EditorGUIUtility.labelWidth = 20;
                                label = "at";
                                myPathOption.MultipleCharacters[0].Position = (PathOptions.CharaterPosition)EditorGUILayout.Popup(label, (int)myPathOption.MultipleCharacters[0].Position, Enum.GetNames(typeof(PathOptions.CharaterPosition)), GUILayout.Width(100));
                            }

                            if (GUILayout.Button("Delete Movement", GUILayout.MinWidth(125)))
                            {
                                myPathOption.MultipleCharacters.Remove(myPathOption.MultipleCharacters[i]);
                                GUILayout.EndHorizontal();
                                break;
                            }

                            GUILayout.EndHorizontal();
                            if (myPathOption.MultipleCharacters[i].Action == PathOptions.CharacterAction.Enter)
                            {
                                EditorGUIUtility.labelWidth = 75;
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(100);
                                myPathOption.MultipleCharacters[i].Position = (PathOptions.CharaterPosition)EditorGUILayout.Popup("To position:", (int)myPathOption.MultipleCharacters[i].Position, Enum.GetNames(typeof(PathOptions.CharaterPosition)), GUILayout.Width(135));
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal();
                                GUILayout.Space(100);
                                myPathOption.MultipleCharacters[i].Orientation = (PathOptions.CharacterOrientation)EditorGUILayout.Popup("Orientation:", (int)myPathOption.MultipleCharacters[i].Orientation, Enum.GetNames(typeof(PathOptions.CharacterOrientation)), GUILayout.Width(135));
                                GUILayout.EndHorizontal();
                            }

                            if (myPathOption.MultipleCharacters[i].Action != PathOptions.CharacterAction.Start)
                            {
                                EditorGUIUtility.labelWidth = 75;
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(100);
                                myPathOption.MultipleCharacters[i].MovementSpeed = EditorGUILayout.FloatField("Speed:", myPathOption.MultipleCharacters[i].MovementSpeed, GUILayout.Width(135));
                                GUILayout.EndHorizontal();
                            }
                        }
                        if (GUILayout.Button("Add Movement"))
                        {
                            myPathOption.MultipleCharacters.Add(new PathOptions.CharacterMovement());
                        }
                    }
                    else
                    {
                        myPathOption.MultipleCharacters[0].CharacterIndex = myPathOption.CharacterIndex;
                        myPathOption.MultipleCharacters[0].Character = Characters.Instance.CharacterList[myPathOption.MultipleCharacters[0].CharacterIndex];
                        myPathOption.gameObject.name = myPathOption.MultipleCharacters[0].Character.CharacterName + " " + myPathOption.MultipleCharacters[0].Action.ToString() + "s";

                        myPathOption.MultipleCharacters[0].Action = (PathOptions.CharacterAction)EditorGUILayout.Popup((int)myPathOption.MultipleCharacters[0].Action, Enum.GetNames(typeof(PathOptions.CharacterAction)), GUILayout.Width(70));

                        string label = "";
                        if (myPathOption.MultipleCharacters[0].Action == PathOptions.CharacterAction.Enter)
                        {
                            EditorGUIUtility.labelWidth = 40;
                            label = "from";
                            myPathOption.MultipleCharacters[0].Direction = (PathOptions.CharacterDirection)EditorGUILayout.Popup(label, (int)myPathOption.MultipleCharacters[0].Direction, Enum.GetNames(typeof(PathOptions.CharacterDirection)), GUILayout.Width(100));
                        }
                        else if (myPathOption.MultipleCharacters[0].Action == PathOptions.CharacterAction.Exit)
                        {
                            EditorGUIUtility.labelWidth = 20;
                            label = "to";
                            myPathOption.MultipleCharacters[0].Direction = (PathOptions.CharacterDirection)EditorGUILayout.Popup(label, (int)myPathOption.MultipleCharacters[0].Direction, Enum.GetNames(typeof(PathOptions.CharacterDirection)), GUILayout.Width(100));
                        }
                        else if (myPathOption.MultipleCharacters[0].Action == PathOptions.CharacterAction.Start)
                        {
                            EditorGUIUtility.labelWidth = 20;
                            label = "at";
                            myPathOption.MultipleCharacters[0].Position = (PathOptions.CharaterPosition)EditorGUILayout.Popup(label, (int)myPathOption.MultipleCharacters[0].Position, Enum.GetNames(typeof(PathOptions.CharaterPosition)), GUILayout.Width(100));
                        }

                        if (myPathOption.MultipleCharacters[0].Action == PathOptions.CharacterAction.Enter)
                        {
                            EditorGUIUtility.labelWidth = 55;
                            myPathOption.MultipleCharacters[0].Position = (PathOptions.CharaterPosition)EditorGUILayout.Popup("Position:", (int)myPathOption.MultipleCharacters[0].Position, Enum.GetNames(typeof(PathOptions.CharaterPosition)), GUILayout.Width(125));
                            EditorGUIUtility.labelWidth = 75;
                            myPathOption.MultipleCharacters[0].Orientation = (PathOptions.CharacterOrientation)EditorGUILayout.Popup("Orientation:", (int)myPathOption.MultipleCharacters[0].Orientation, Enum.GetNames(typeof(PathOptions.CharacterOrientation)), GUILayout.Width(135));
                        }
                        GUILayout.EndHorizontal();

                        if (myPathOption.MultipleCharacters[0].Action != PathOptions.CharacterAction.Start)
                        {
                            EditorGUIUtility.labelWidth = 75;
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(100);
                            myPathOption.MultipleCharacters[0].MovementSpeed = EditorGUILayout.FloatField("Speed:", myPathOption.MultipleCharacters[0].MovementSpeed, GUILayout.Width(135));
                            GUILayout.EndHorizontal();
                        }
                    }
                }
                break;
            case PathOptions.PathOptionsType.FullScreenArt:
                {
                    GUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 90;
                    myPathOption.FullscreenArt = (Sprite)EditorGUILayout.ObjectField("Fullscreen Art: ", myPathOption.FullscreenArt, typeof(Sprite), true, GUILayout.MaxHeight(16));
                    myPathOption.name = "Fullscreen Art: ";
                    if(myPathOption.FullscreenArt != null)
                    {
                        myPathOption.name += myPathOption.FullscreenArt.name;
                    }
                    GUILayout.EndHorizontal();
                }
                break;

        }

        EditorUtility.SetDirty(target);
        if (!EditorApplication.isPlaying)
            EditorSceneManager.MarkAllScenesDirty();
    }
}
