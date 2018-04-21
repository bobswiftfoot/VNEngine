using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[Serializable]
public class Path : MonoBehaviour
{
    public string PathName;
    public List<PathOptions> PathOptions;
    public List<Questions> QuestionLines;

    public Path()
    {
        PathName = "";
        PathOptions = new List<PathOptions>();
        QuestionLines = new List<Questions>();
    }

    private void OnDestroy()
    {
        if (gameObject.GetComponentInParent<Scene>())
        {
            (gameObject.GetComponentInParent<Scene>()).Paths.Remove(this);
        }
    }
}

[Serializable]
public class Questions
{
    public string Response;
    public int LeadsToScene;
    public int LeadsToPath;
}



