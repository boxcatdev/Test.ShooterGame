using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameController))]
public class GameFSM : StateMachineMB
{
    private GameController _controller;

    // state instances
    public GameSetupState SetupState { get; private set; }

    private void Awake()
    {
        _controller = GetComponent<GameController>();

        // construct states
        SetupState = new GameSetupState(this, _controller);
    }

    private void Start()
    {
        ChangeState(SetupState);
    }
}