using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScar : MonoBehaviour
{
    void Awake()
    {
        //animate
        StartCoroutine(Tks.SetTimeout(() =>
        {
            LeanTween.scale(gameObject, Vector3.zero, 1.0f)
            .setOnComplete(() => Destroy(gameObject));
        }, 7500));
    }
}
