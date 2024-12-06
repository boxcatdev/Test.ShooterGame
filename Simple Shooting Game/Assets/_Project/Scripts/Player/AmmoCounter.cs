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

    private void Awake()
    {
        SetupCounter();
    }
    private void OnEnable()
    {
        gun.OnAmmoChanged += UpdateCounter;
    }
    private void OnDisable()
    {
        gun.OnAmmoChanged -= UpdateCounter;
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
}
