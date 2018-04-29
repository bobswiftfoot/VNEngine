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
    public GameObject FullScreenArt;

    public static int MoveCharacterIndex = 0;
    public static bool CharacterMoving = false;

    enum States
    {
        EnterScene,
        ChooseType,
        MoveCharacter,
        WaitForCharacterMove,
        TextStart,
        TextScroll,
        WaitForInput,
        ShowArt,
        StartQuestion,
        WaitForQuestionInput,
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
                Questions.SetActive(false);
                Dialogs.SetActive(false);
                FullScreenArt.SetActive(false);
                Characters.Instance.HideCharacters();
                //TODO: Transition in
                //Set Current Background for scene
                Backgrounds.Instance.SetCurrentBackground(Conversation.Instance.Scenes[currentScene].BackgroundIndex);
                //TODO: Play Music
                currentState = States.ChooseType;
                break;
            case States.ChooseType:
                switch (Conversation.Instance.Scenes[currentScene].Paths[currentPath].PathOptions[currentDialogueLine].Type)
                {
                    case PathOptions.PathOptionsType.CharacterMove:
                        currentState = States.MoveCharacter;
                        break;
                    case PathOptions.PathOptionsType.Dialogue:
                        currentState = States.TextStart;
                        break;
                    case PathOptions.PathOptionsType.FullScreenArt:
                        currentState = States.ShowArt;
                        break;
                }
                break;
            case States.MoveCharacter:
                {
                    PathOptions currentOption = Conversation.Instance.Scenes[currentScene].Paths[currentPath].PathOptions[currentDialogueLine];
                    if (currentOption.MultipleCharacters.Count == 1 || currentOption.MultipleMovementType == PathOptions.CharacterMultipleMovementType.AllAtOnce)
                    {
                        foreach (PathOptions.CharacterMovement movement in currentOption.MultipleCharacters)
                        {
                            movement.Character.gameObject.SetActive(true);
                            StartCoroutine(MoveCharacter(movement.Character, movement.Action, movement.Direction, movement.Position, true));
                        }
                    }
                    else
                    {
                        StartCoroutine(MoveCharactersOneAtATime(currentOption.MultipleCharacters));
                    }
                    currentState = States.WaitForCharacterMove;
                }
                break;
            case States.WaitForCharacterMove:
                //MoveCharacter will handle leaving this state
                break;
            case States.TextStart:
                Questions.SetActive(false);
                Dialogs.SetActive(true);
                if (Conversation.Instance.Scenes[currentScene].Paths[currentPath].PathOptions[currentDialogueLine].Type == PathOptions.PathOptionsType.Dialogue)
                {
                    nameText.text = Conversation.Instance.Scenes[currentScene].Paths[currentPath].PathOptions[currentDialogueLine].Character.CharacterName;
                    StartCoroutine(ScrollText(Conversation.Instance.Scenes[currentScene].Paths[currentPath].PathOptions[currentDialogueLine].DialogueLine));
                }
                //TODO: Play voice clip
                currentState = States.TextScroll;
                break;
            case States.TextScroll:
                if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("space"))
                {
                    StopAllCoroutines();
                    //TODO: Stop Voice clip
                    if (Conversation.Instance.Scenes[currentScene].Paths[currentPath].PathOptions[currentDialogueLine].Type == PathOptions.PathOptionsType.Dialogue)
                    {
                        dialogueText.text = Conversation.Instance.Scenes[currentScene].Paths[currentPath].PathOptions[currentDialogueLine].DialogueLine;
                    }
                    currentState = States.WaitForInput;
                }
                break;
            case States.ShowArt:
                Dialogs.SetActive(false);
                Questions.SetActive(false);
                FullScreenArt.SetActive(true);
                FullScreenArt.GetComponent<SpriteRenderer>().sprite = Conversation.Instance.Scenes[currentScene].Paths[currentPath].PathOptions[currentDialogueLine].FullscreenArt;
                currentState = States.WaitForInput;
                break;
            case States.WaitForInput:
                if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("space"))
                {
                    FullScreenArt.SetActive(false);
                    if (currentDialogueLine == Conversation.Instance.Scenes[currentScene].Paths[currentPath].PathOptions.Count - 1)
                    {
                        if (Conversation.Instance.Scenes[currentScene].Paths[currentPath].QuestionLines.Count > 0)
                        {
                            currentState = States.StartQuestion;
                        }
                        else
                        {
                            currentState = States.EndDialogueLine;
                        }
                    }
                    else
                    {
                        currentState = States.EndDialogueLine;
                    }
                }
                break;
            case States.StartQuestion:
                Questions.SetActive(true);
                Dialogs.SetActive(false);
                for (int i = 0; i < questionObjects.Count; i++)
                {
                    if (i < Conversation.Instance.Scenes[currentScene].Paths[currentPath].QuestionLines.Count)
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
                //Responce on Click will handle moving to a new state
                break;
            case States.EndDialogueLine:
                if (currentDialogueLine == Conversation.Instance.Scenes[currentScene].Paths[currentPath].PathOptions.Count - 1)
                {
                    if (currentPath == Conversation.Instance.Scenes[currentScene].Paths.Count - 1)
                    {
                        //Game Over?
                        //Last path in scene, last dialogue line
                    }
                    else
                    {
                        //End of dialogue lines, continue onto next path.
                        currentPath++;
                        currentState = States.ChooseType;
                    }
                }
                else
                {
                    //Continue onto next line
                    currentDialogueLine++;
                    currentState = States.ChooseType;
                }
                break;
            case States.ExitScene:
                //TODO: Transition Out
                //TODO: Remove characters?
                //TODO: End Music
                currentScene = nextScene;
                currentPath = nextPath;
                currentDialogueLine = 0;
                currentState = States.EnterScene;
                break;
        }
    }

    private void ResponceOnClick(int buttonID)
    {
        nextScene = Conversation.Instance.Scenes[currentScene].Paths[currentPath].QuestionLines[buttonID].LeadsToScene;
        nextPath = Conversation.Instance.Scenes[currentScene].Paths[currentPath].QuestionLines[buttonID].LeadsToPath;

        if (nextScene == currentScene)
        {
            currentPath = nextPath;
            currentState = States.ChooseType;
        }
        else
        {
            currentState = States.ExitScene;
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

    IEnumerator MoveCharactersOneAtATime(List<PathOptions.CharacterMovement> characterMovements)
    {
        foreach (PathOptions.CharacterMovement movement in characterMovements)
        {
            while (CharacterMoving)
            {
                yield return null;
            }
            CharacterMoving = true;
            movement.Character.gameObject.SetActive(true);
            StartCoroutine(MoveCharacter(movement.Character, movement.Action, movement.Direction, movement.Position, false));
        }

        yield return null;
    }  

    IEnumerator MoveCharacter(Character character, PathOptions.CharacterAction action, PathOptions.CharacterDirection direction, PathOptions.CharaterPosition position, bool AllAtOnce)
    {
        MoveCharacterIndex++;

        Vector3 targetPosition = new Vector3();
        switch (action)
        {
            case PathOptions.CharacterAction.Enter:
                {
                    switch (position)
                    {
                        case PathOptions.CharaterPosition.FarLeft:
                            targetPosition = character.FarLeftPosition;
                            break;
                        case PathOptions.CharaterPosition.Left:
                            targetPosition = character.LeftPosition;
                            break;
                        case PathOptions.CharaterPosition.Center:
                            targetPosition = character.CenterPosition;
                            break;
                        case PathOptions.CharaterPosition.Right:
                            targetPosition = character.RightPosition;
                            break;
                        case PathOptions.CharaterPosition.FarRight:
                            targetPosition = character.FarRightPosition;
                            break;
                    }

                    switch (direction)
                    {
                        case PathOptions.CharacterDirection.Left:
                            character.transform.position = new Vector2(-12, targetPosition.y);
                            break;
                        case PathOptions.CharacterDirection.Right:
                            character.transform.position = new Vector2(12, targetPosition.y);
                            break;
                        case PathOptions.CharacterDirection.Top:
                            character.transform.position = new Vector2(targetPosition.x, 12);
                            break;
                        case PathOptions.CharacterDirection.Bottom:
                            character.transform.position = new Vector2(targetPosition.x, -12);
                            break;
                    }
                }
                break;
            case PathOptions.CharacterAction.Exit:
                {
                    targetPosition = character.gameObject.transform.position;

                    switch (direction)
                    {
                        case PathOptions.CharacterDirection.Left:
                            targetPosition = new Vector2(-12, targetPosition.y);
                            break;
                        case PathOptions.CharacterDirection.Right:
                            targetPosition = new Vector2(10, targetPosition.y);
                            break;
                        case PathOptions.CharacterDirection.Top:
                            targetPosition = new Vector2(targetPosition.x, 10);
                            break;
                        case PathOptions.CharacterDirection.Bottom:
                            targetPosition = new Vector2(targetPosition.x, -10);
                            break;
                    }
                }
                break;
            case PathOptions.CharacterAction.Start:
                {
                    switch (position)
                    {
                        case PathOptions.CharaterPosition.FarLeft:
                            targetPosition = character.FarLeftPosition;
                            break;
                        case PathOptions.CharaterPosition.Left:
                            targetPosition = character.LeftPosition;
                            break;
                        case PathOptions.CharaterPosition.Center:
                            targetPosition = character.CenterPosition;
                            break;
                        case PathOptions.CharaterPosition.Right:
                            targetPosition = character.RightPosition;
                            break;
                        case PathOptions.CharaterPosition.FarRight:
                            targetPosition = character.FarRightPosition;
                            break;
                    }

                    character.transform.position = new Vector2(targetPosition.x, targetPosition.y);
                }
                break;
        }
        //Make sure all characters are in front
        targetPosition.z = -1;

        while (character.transform.position.x != targetPosition.x || character.transform.position.y != targetPosition.y)
        {
            if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("space"))
            {
                character.transform.position = targetPosition;
            }
            else
            {
                float speed = 5.0f;
                float step = speed * Time.deltaTime;
                character.transform.position = Vector3.MoveTowards(character.transform.position, targetPosition, step);
                yield return null;
            }
        }

        MoveCharacterIndex--;
        if (MoveCharacterIndex == 0)
        {
            if (AllAtOnce)
            {
                while (currentState != States.WaitForCharacterMove)
                {
                    //Sometimes a start move will reach this before its in the correct state
                    yield return null;
                }
                currentState = States.EndDialogueLine;
            }
            else
            {
                CharacterMoving = false;
            }
        }
        yield return null;
    }
}
