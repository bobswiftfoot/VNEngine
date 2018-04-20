using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Scene))]
public class SceneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Scene myScene = (Scene)target;

        myScene.Name = EditorGUILayout.TextField("Scene Name: ", myScene.Name);
        myScene.gameObject.name = myScene.Name;

        myScene.BackgroundIndex = EditorGUILayout.Popup("Background: ", myScene.BackgroundIndex, Backgrounds.Instance.GetNames().ToArray());
        myScene.Background = Backgrounds.Instance.GetNames()[myScene.BackgroundIndex];

        myScene.Music = (AudioClip)EditorGUILayout.ObjectField("Music", myScene.Music, typeof(AudioClip), true);

        for (int p = 0; p < myScene.Paths.Count; p++)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Path " + (p + 1) + ": " + myScene.Paths[p].PathName);
            if (GUILayout.Button("Delete Path"))
            {
                foreach (Path path in myScene.gameObject.GetComponentsInChildren<Path>())
                {
                    if (path.Equals(myScene.Paths[p]))
                    {
                        myScene.Paths.Remove(path);
                        DestroyImmediate(path.gameObject);
                        break;
                    }
                }
                break;
            }
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Path"))
        {
            GameObject newObject = new GameObject();
            Path newPath = newObject.AddComponent<Path>();
            newObject.name = newPath.PathName = "Path: " + (myScene.Paths.Count + 1);
            myScene.Paths.Add(newPath);
            GameObjectUtility.SetParentAndAlign(newObject, myScene.gameObject);
            EditorUtility.SetDirty(newObject);
        }
    }
}
