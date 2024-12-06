using UnityEngine;
using UnityEngine.Events;

public class ButtonInteractable : BaseInteractable
{
    [Header("Events")]
    public UnityEvent OnButtonPress;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        _animator.enabled = false;
    }
    public override void LeaveInteraction()
    {
        base.LeaveInteraction();
    }

    protected override void DoInteractableAction(bool value)
    {
        base.DoInteractableAction(value);

        if (value)
        {
            OnButtonPress?.Invoke();
            ButtonAnimation();
        }
    }

    private void ButtonAnimation()
    {
        _animator.enabled = true;
    }
    public void ButtonAnimEnd()
    {
        _animator.enabled = false;
    }
}
