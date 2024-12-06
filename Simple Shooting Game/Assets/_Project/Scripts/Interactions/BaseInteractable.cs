using UnityEngine;

public class BaseInteractable : MonoBehaviour, IInteractable
{
    [Header("IInteractable variables")]
    [SerializeField] string _interactText;
    [SerializeField] bool _isInteracting = false;

    /*[Header("Demo")]
    [SerializeField] protected GameObject _demoVisual;
    [Space]*/

    [SerializeField]
    protected PGInput interactionInput;
    protected Transform _otherTransform;
    bool _canInteract = true;

    #region IInteractable Methods
    public string GetInteractText()
    {
        return _interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }
    public PGInput GetInteractionInput()
    {
        return interactionInput;
    }

    public void TriggerInteraction(Transform transform, bool isInteracting)
    {
        if (_isInteracting != isInteracting)
        {
            _isInteracting = isInteracting;
            _otherTransform = transform;
            DoInteractableAction(isInteracting);
        }

    }
    public virtual void LeaveInteraction()
    {
        //Debug.Log("LeaveInteraction()");
        if (_isInteracting == true)
        {
            _isInteracting = false;
            _otherTransform = null;
            DoInteractableAction(false);
        }
    }
    protected virtual void DoInteractableAction(bool value)
    {

    }

    public bool CanInteract()
    {
        return _canInteract;
    }

    public void DisableInteractions()
    {
        _canInteract = false;
    }

    public void EnableInteractions()
    {
        _canInteract = true;
    }
    #endregion

}
