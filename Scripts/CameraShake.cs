using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnShake(float duration, float strenght)
    {
        transform.DOShakePosition(duration, strenght);
        transform.DOShakeRotation(duration, strenght);
    }

    public static void Shake(float duration, float strenght)
    {
        instance.OnShake(duration, strenght);
    }
}
