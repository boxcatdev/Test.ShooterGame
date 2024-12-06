using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool _startOff = true;

    [Header("Targets")]
    [SerializeField] private List<Target> targets = new List<Target>();

    private void Awake()
    {
        if(targets.Count == 0) targets = new List<Target>(FindObjectsByType<Target>(FindObjectsSortMode.None));
    }
    private void Start()
    {
        if(_startOff) DisableAllTargets();
    }

    public void DisableAllTargets()
    {
        foreach (var target in targets)
        {
            target.gameObject.SetActive(false);
        }
    }
    public void EnableAllTargets()
    {
        foreach(var target in targets)
        {
            target.gameObject.SetActive(true);
        }
    }

    public void RespawnTarget(Target target, float respawnTime)
    {
        StartCoroutine(DelayRespawn(target, respawnTime));
    }
    IEnumerator DelayRespawn(Target target, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        target.gameObject.SetActive(true);
    }
}
