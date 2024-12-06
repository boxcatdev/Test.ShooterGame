using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private Camera _playerCam;
    [SerializeField] private UIDictionarySO _uiDictionary;

    public static Camera playerCamera;

    [Header("Menu")]
    [SerializeField] private LerpUI _menu;
    [SerializeField] private LerpUI _menuPrompt;

    private InputHandler _input;
    private bool _isPaused = false;

    private void Awake()
    {
        playerCamera = _playerCam;
        _input = FindAnyObjectByType<InputHandler>();
    }
    private void OnEnable()
    {
        _input.OnPauseAction += TogglePause;
        _input.OnDeviceUpdated += UpdateSprite;
    }
    private void OnDisable()
    {
        _input.OnPauseAction -= TogglePause;
        _input.OnDeviceUpdated -= UpdateSprite;
    }

    public void TogglePause()
    {
        _menuPrompt.NextTarget();
        _menu.NextTarget();

        _isPaused = !_isPaused;

        if(_input.isGamepad == false)
        {
            if (_isPaused)
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else
        {
                Cursor.lockState = CursorLockMode.Locked;
        }
    }
    private void UpdateSprite()
    {
        if (_menuPrompt == null) return;

        _menuPrompt.GetComponent<Image>().sprite = _uiDictionary.GetSprite(PGInput.Pause, _input.isGamepad);
    }
    public void ReloadScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
