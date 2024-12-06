using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    [SerializeField]
    PlayerInteract playerInteract;
    [SerializeField]
    TextMeshProUGUI _interactionText;
    [SerializeField]
    Image _buttonPrompt;

    [Header("Reference")]
    [SerializeField] private UIDictionarySO _uiDictionarySO;

    bool _hasInteractable;
    bool _isInteracting;

    private InputHandler _input;

    private void Awake()
    {
        //getting components
        playerInteract = FindFirstObjectByType<PlayerInteract>();
        _input = playerInteract.GetComponent<InputHandler>();
        //_interactionText = GetComponentInChildren<TextMeshProUGUI>();

        RefreshUI(false);
    }
    private void OnEnable()
    {
        playerInteract.OnGetInteractable += RefreshUI;
        playerInteract.OnPressInteractionKey += RefreshUI;
    }
    private void OnDisable()
    {
        playerInteract.OnGetInteractable -= RefreshUI;
        playerInteract.OnPressInteractionKey -= RefreshUI;
    }

    #region Show and Hide the UI
    //shows and hides UI
    private void Show(IInteractable interactable)
    {
        if (interactable.CanInteract())
        {
            _interactionText.gameObject.SetActive(true);
            _interactionText.text = interactable.GetInteractText();

            _buttonPrompt.gameObject.SetActive(true);
            _buttonPrompt.sprite = _uiDictionarySO.GetSprite(interactable.GetInteractionInput(), _input.isGamepad);
        }
    }
    private void Hide()
    {
        _interactionText.gameObject.SetActive(false);
        _buttonPrompt.gameObject.SetActive(false);
    }
    #endregion

    //updates UI based on interaction action
    private void RefreshUI(bool actionValue)
    {
        //Debug.Log("RefreshUI()");

        //update interaction values
        _hasInteractable = playerInteract.currentInteractable != null ? true : false;
        _isInteracting = playerInteract.IsInteracting;

        //switch
        if (_hasInteractable == true && _isInteracting == false)
        {
            Show(playerInteract.currentInteractable);
        }
        else if (_hasInteractable == true && _isInteracting == true)
        {
            Hide();
        }
        else if (_hasInteractable == false)
        {
            Hide();
        }
    }

}
