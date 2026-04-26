using System.Collections;
using UnityEngine;

public class HitStopUtils : MonoBehaviour
{
    public static HitStopUtils Instance;

    [Header("HitStop Settings")]
    public float defaultHitStopDuration = 0.1f;

    private void Awake()
    {
        Instance = this;
    }

    public static void TriggerHitStop()
    {
        Instance.StartCoroutine(Instance.HitStopRoutine(Instance.defaultHitStopDuration));
    }

    public static void TriggerHitStop(float duration)
    {
        Instance.StartCoroutine(Instance.HitStopRoutine(duration));
    }

    private IEnumerator HitStopRoutine(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }
}