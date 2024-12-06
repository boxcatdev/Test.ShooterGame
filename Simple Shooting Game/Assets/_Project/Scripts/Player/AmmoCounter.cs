using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounter : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color _emptyColor;
    [SerializeField] private Color _normalColor;

    [Header("References")]
    [SerializeField] private GunTest gun;
    [Space]
    [SerializeField] private List<Image> _bullets = new List<Image>();

    [Header("Prompt")]
    [SerializeField] private Image _buttonPrompt;
    [SerializeField] private UIDictionarySO _uIDictionarySO;

    private InputHandler _input;

    private void Awake()
    {
        SetupCounter();

        _input = gun.GetComponent<InputHandler>();
    }
    private void Start()
    {
        if(_buttonPrompt != null) _buttonPrompt.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        gun.OnAmmoChanged += UpdateCounter;
        gun.OnReloadPrompt += UpdatePrompt;

        _input.OnDeviceUpdated += UpdateSprite;
    }
    private void OnDisable()
    {
        gun.OnAmmoChanged -= UpdateCounter;
        gun.OnReloadPrompt -= UpdatePrompt;

        _input.OnDeviceUpdated -= UpdateSprite;
    }

    private void SetupCounter()
    {
        if(gun == null) return;

        if(_bullets.Count > gun.maxAmmoCount)
        {
            for(int i = 0; i < _bullets.Count; i++)
            {
                if(i > gun.maxAmmoCount)
                {
                    _bullets[i].gameObject.SetActive(false);
                }
            }
        }
    }

    private void UpdateCounter(int count)
    {
        for (int i = 0; i < _bullets.Count; i++)
        {
            if(i < count)
            {
                _bullets[i].color = _normalColor;
            }
            else
            {
                _bullets[i].color = _emptyColor;
            }
        }
    }
    private void UpdatePrompt(bool active)
    {
        if(_buttonPrompt == null) return;
        if(_uIDictionarySO == null) return;

        if (active)
        {
            _buttonPrompt.gameObject.SetActive(true);
            _buttonPrompt.sprite = _uIDictionarySO.GetSprite(PGInput.Reload, _input.isGamepad);
        }
        else
        {
            _buttonPrompt.gameObject.SetActive(false);
        }
    }
    private void UpdateSprite()
    {
        _buttonPrompt.sprite = _uIDictionarySO.GetSprite(PGInput.Reload, _input.isGamepad);
    }
}
