using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    [Header("Input")]
    public Vector2 move;
    public Vector2 look;
    [Space]
    public bool isGamepad;

    public Action OnShootAction = delegate { };
    public Action OnReloadAction = delegate { };
    public Action OnUseAction = delegate { };
    public Action OnPauseAction = delegate { };

    public Action OnDeviceUpdated = delegate { };

    private PlayerInput playerInput;

    private string _currentControlScheme;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        CheckDevice(playerInput.currentControlScheme);
    }

    private void CheckDevice(string current)
    {
        if (current.Equals("Keyboard"))
        {
            isGamepad = false;
        }
        else
        {
            isGamepad = true;
        }

        OnDeviceUpdated?.Invoke();
    }

    public void OnMove(InputValue value)
    {
        CheckDevice(playerInput.currentControlScheme);

        MoveInput(value.Get<Vector2>());
    }
    public void OnLook(InputValue value)
    {
        CheckDevice(playerInput.currentControlScheme);

        LookInput(value.Get<Vector2>());
    }
    public void OnShoot(InputValue value)
    {
        CheckDevice(playerInput.currentControlScheme);

        ShootInput(value.isPressed);
    }
    public void OnReload(InputValue value)
    {
        CheckDevice(playerInput.currentControlScheme);

        ReloadInput(value.isPressed);
    }
    public void OnUse(InputValue value)
    {
        CheckDevice(playerInput.currentControlScheme);

        UseInput(value.isPressed);
    }
    public void OnPause(InputValue value)
    {
        CheckDevice(playerInput.currentControlScheme);

        PauseInput(value.isPressed);
    }


    public void MoveInput(Vector2 moveDirection)
    {
        move = moveDirection;
    }
    public void LookInput(Vector2 lookDirection)
    {
        look = lookDirection;
    }
    public void ShootInput(bool shootValue)
    {
        if (shootValue) OnShootAction?.Invoke();
    }
    public void ReloadInput(bool reloadValue)
    {
        if (reloadValue) OnReloadAction?.Invoke();
    }
    public void UseInput(bool useValue)
    {
        if (useValue) OnUseAction?.Invoke();
    }
    public void PauseInput(bool pauseValue)
    {
        if(pauseValue) OnPauseAction?.Invoke();
    }
}
