using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager _stateManager;
    public static StateManager Instance
    {
        get
        {
            if (_stateManager == null)
                _stateManager = (StateManager)FindObjectOfType(typeof(StateManager));
            return _stateManager;
        }
    }

    private void Awake()
    {
        if (_stateManager != null && _stateManager != this)
            Destroy(_stateManager);
        else
            _stateManager = this;
    }

    public PlayState playState;

    public enum StateNames
    {
        Play,
    }

    BaseState currentState;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    public void ChangeState(StateNames newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }

        switch(newState)
        {
            case StateNames.Play:
                {
                    currentState = playState;
                }
                break;
        }

        currentState.Enter();
    }
}
