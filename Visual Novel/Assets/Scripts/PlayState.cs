using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayState : BaseState
{
    int currentScene;
    int currentPath;
    int currentDialogueLine;

    int nextScene;
    int nextPath;

    States currentState;

    public Text nameText;
    public Text dialogueText;

    public List<Button> questionObjects;
    public List<Text> questionText;

    public GameObject Dialogs;
    public GameObject Questions;

    enum States
    {
        EnterScene,
        EnterCharacter,
        TextStart,
        TextScroll,
        WaitForInput,
        StartQuestion,
        WaitForQuestionInput,
        ExitCharater,
        EndDialogueLine,
        ExitScene,
    }

    public override void Init()
    {
        base.Init();
        currentScene = 0;
        currentPath = 0;
        currentDialogueLine = 0;
        currentState = States.EnterScene;
    }

    public override void Enter()
    {
        base.Enter();
        //TODO: load/select current scene/path/dialogueLine
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        switch (currentState)
        {
            case States.EnterScene:
                //TODO: Transition in
                //Set Current Background for scene
                Backgrounds.Instance.SetCurrentBackground(Conversation.Instance.Scenes[currentScene].BackgroundIndex);
                //TODO: Play Music
                currentState = States.EnterCharacter;
                break;
            case States.EnterCharacter:
                //TODO: Slide character in if needed
                currentState = States.TextStart;
                break;
            case States.TextStart:
                Questions.SetActive(false);
                Dialogs.SetActive(true);
                nameText.text = Conversation.Instance.Scenes[currentScene].Paths[currentPath].DialogueLines[currentDialogueLine].Character;
                StartCoroutine(ScrollText(Conversation.Instance.Scenes[currentScene].Paths[currentPath].DialogueLines[currentDialogueLine].DialogueLine));
                //TODO: Play voice clip
                currentState = States.TextScroll;
                break;
            case States.TextScroll:
                if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("space"))
                {
                    StopAllCoroutines();
                    //TODO: Stop Voice clip
                    dialogueText.text = Conversation.Instance.Scenes[currentScene].Paths[currentPath].DialogueLines[currentDialogueLine].DialogueLine;
                    currentState = States.WaitForInput;
                }
                break;
            case States.WaitForInput:
                if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("space"))
                {
                    if (currentDialogueLine == Conversation.Instance.Scenes[currentScene].Paths[currentPath].DialogueLines.Count - 1)
                    {
                        if(Conversation.Instance.Scenes[currentScene].Paths[currentPath].QuestionLines.Count > 0)
                        {
                            currentState = States.StartQuestion;
                        }
                        else
                        {
                            currentState = States.ExitCharater;
                        }
                    }
                    else
                    {
                        currentDialogueLine++;
                        currentState = States.TextStart;
                    }
                }
                break;
            case States.StartQuestion:
                Questions.SetActive(true);
                Dialogs.SetActive(false);
                for(int i = 0; i < questionObjects.Count; i++)
                {
                    if(i < Conversation.Instance.Scenes[currentScene].Paths[currentPath].QuestionLines.Count)
                    {
                        questionObjects[i].gameObject.SetActive(true);
                        questionObjects[i].onClick.RemoveAllListeners();
                        int b = i;
                        questionObjects[i].onClick.AddListener(delegate { ResponceOnClick(b); });
                        questionText[i].text = Conversation.Instance.Scenes[currentScene].Paths[currentPath].QuestionLines[i].Response;
                    }
                    else
                    {
                        questionObjects[i].gameObject.SetActive(false);
                    }
                }
                currentState = States.WaitForQuestionInput;
                break;
            case States.WaitForQuestionInput:
                break;
            case States.ExitCharater:
                //TODO: Slide Character out if needed
                currentState = States.EndDialogueLine;
                break;
            case States.EndDialogueLine:
                if (currentDialogueLine == Conversation.Instance.Scenes[currentScene].Paths[currentPath].DialogueLines.Count - 1)
                {
                    //Game Over?
                }
                else
                {
                    currentPath++;
                    currentState = States.EnterCharacter;
                }
                break;
            case States.ExitScene:
                //TODO: Transition Out
                //TODO: Remove characters?
                //TODO: End Music
                currentScene = nextScene;
                currentPath = nextPath;
                currentState = States.EnterScene;
                break;
        }
    }

    private void ResponceOnClick(int buttonID)
    {
        nextScene = Conversation.Instance.Scenes[currentScene].Paths[currentPath].QuestionLines[buttonID].LeadsToScene;
        nextPath = Conversation.Instance.Scenes[currentScene].Paths[currentPath].QuestionLines[buttonID].LeadsToPath;

        if(nextScene == currentScene)
        {
            currentPath = nextPath;
            currentState = States.EnterCharacter;
        }
        else
        {
            currentState = States.EnterScene;
        }
    }

    IEnumerator ScrollText(string dialogueLine)
    {
        dialogueText.text = "";
        foreach (char letter in dialogueLine)
        {
            dialogueText.text += letter;
            yield return null;
        }

        currentState = States.WaitForInput;
    }
}
