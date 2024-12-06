using UnityEngine;

public class PromptPanel : MonoBehaviour
{
    private InputHandler input;

    [Header("Swap")]
    [SerializeField] private GameObject _keyboardObj;
    [SerializeField] private GameObject _gamepadObj;

    private void Awake()
    {
        input = FindAnyObjectByType<InputHandler>();
    }
    private void Start()
    {
        Swap();
    }
    private void OnEnable()
    {
        input.OnDeviceUpdated += Swap;
    }
    private void OnDisable()
    {
        input.OnDeviceUpdated -= Swap;
    }

    private void Swap()
    {
        if(_keyboardObj == null) return;
        if(_gamepadObj == null) return;

        if(input.isGamepad == false)
        {
            _keyboardObj.SetActive(true);
            _gamepadObj.SetActive(false);
        }
        else
        {
            _keyboardObj.SetActive(false);
            _gamepadObj.SetActive(true);
        }
    }
}
