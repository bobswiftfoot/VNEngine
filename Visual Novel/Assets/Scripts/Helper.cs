using System.Collections.Generic;

public class Helper
{
    public static List<string> CreatePathList(Conversation conversation)
    {
        List<string> pathList = new List<string>();
        for(int i = 0; i < conversation.Scenes.Count; i++)
        {
            for(int j = 0; j < conversation.Scenes[i].Paths.Count; j++)
            {
                pathList.Add(conversation.Scenes[i].Name + ", " + conversation.Scenes[i].Paths[j].PathName);
            }
        }

        return pathList;
    }

    public static int GetIndexOfPathList(Conversation conversation, int sceneIndex, int pathIndex)
    {
        int index = 0;
        for (int i = 0; i < conversation.Scenes.Count; i++)
        {
            for (int j = 0; j < conversation.Scenes[i].Paths.Count; j++)
            {
                if (sceneIndex == i && pathIndex == j)
                {
                    return index;
                }
                index++;
            }
        }

        return 0;
    }

    public static int GetPathFromIndex(Conversation conversation, int findIndex)
    {
        int index = 0;
        for (int i = 0; i < conversation.Scenes.Count; i++)
        {
            for (int j = 0; j < conversation.Scenes[i].Paths.Count; j++)
            {
                if (index == findIndex)
                {
                    return j;
                }
                index++;
            }
        }

        return 0;
    }

    public static int GetSceneFromIndex(Conversation conversation, int findIndex)
    {
        int index = 0;
        for (int i = 0; i < conversation.Scenes.Count; i++)
        {
            for (int j = 0; j < conversation.Scenes[i].Paths.Count; j++)
            {
                if (index == findIndex)
                {
                    return i;
                }
                index++;
            }
        }

        return 0;
    }
}