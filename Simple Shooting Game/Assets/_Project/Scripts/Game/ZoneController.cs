using PatchworkGames;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
    [Header("Zone Targets")]
    [SerializeField] private List<Target> zoneTargets = new List<Target>();
    [SerializeField] private float _zoneTimer = 30f;
    [SerializeField] private bool _zoneStarted = false;

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _completionText;

    private int zoneCount = 0;
    private float _timerProgress;
    public float timerProgress => _timerProgress;

    public Action<bool> OnZoneStarted = delegate { };

    private void Awake()
    {
        if(zoneTargets.Count == 0)
        {
            zoneTargets = new List<Target>(transform.GetComponentsInChildren<Target>(true));
        }

        zoneCount = zoneTargets.Count;
    }
    private void Start()
    {
        if(_timerText != null) if(_timerText.gameObject.activeInHierarchy) _timerText.gameObject.SetActive(false);
        if(_completionText!= null) if (_completionText.gameObject.activeInHierarchy) _completionText.gameObject.SetActive(false);

    }
    private void OnEnable()
    {
        OnZoneStarted += EnableTimerText;
    }
    private void OnDisable()
    {
        OnZoneStarted += EnableTimerText;
    }
    private void Update()
    {
        if (_zoneStarted)
        {
            _timerProgress -= Time.deltaTime;
            if(_timerProgress <= 0)
            {
                _zoneStarted = false;
                if(zoneCount > 0)
                {
                    ZoneFailed();
                }
            }

            RefreshTimerText();
        }
    }

    private void RefreshTimerText()
    {
        if (_timerText == null) return;

        float seconds = _timerProgress;
        _timerText.text = Utility.DisplayTimeSeconds(seconds, false);
    }
    private void EnableTimerText(bool enabled)
    {
        if (_timerText == null) return;

        _timerText.gameObject.SetActive(enabled);
    }
    public void StartZone()
    {
        if(_zoneStarted) return;

        zoneCount = zoneTargets.Count;
        _zoneStarted = true;
        _timerProgress = _zoneTimer;
        
        OnZoneStarted?.Invoke(true);
    }
    public void HitZone()
    {
        if(_zoneStarted == false) return;

        zoneCount--;

        if (zoneCount <= 0)
        {
            _zoneStarted = false;
            ZoneComplete();
        }
    }
    private void ZoneComplete()
    {
        OnZoneStarted?.Invoke(false);

        ScoreManager.Instance.AddPoints(100);

        if(_completionText != null)
        {
            _completionText.gameObject.SetActive(true);
            _completionText.text = "Zone Complete!";
            StartCoroutine(DelayDisable(_completionText.gameObject, 2f));
        }
    }
    private void ZoneFailed()
    {
        OnZoneStarted?.Invoke(false);

        // disable all targets
        foreach (var target in zoneTargets)
        {
            target.EndAnim();
        }

        if (_completionText != null)
        {
            _completionText.gameObject.SetActive(true);
            _completionText.text = "Zone Failed!";
            StartCoroutine(DelayDisable(_completionText.gameObject, 2f));
        }
    }

    IEnumerator DelayDisable(GameObject gameObject, float delay)
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false);
    }
}
