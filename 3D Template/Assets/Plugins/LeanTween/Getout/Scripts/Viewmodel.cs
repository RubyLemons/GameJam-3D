using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewmodel : MonoBehaviour
{

    [SerializeField] Freelook freelook;

    [Header("Animate")]

    Vector2 sway;
    [SerializeField] float smooth = 0.225f;

    [SerializeField] float sens = -75f;

    [Space(10)]

    public static bool show;

    void Update()
    {
        if (!Tks.freelook) return;

        sway = new Vector2(Input.GetAxis("Mouse X") * sens * (freelook.maxLook ? 0 : 1), Input.GetAxis("Mouse Y") * sens * (freelook.maxLook ? 0 : 1)) * Time.deltaTime;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(sway.y, -sway.x, 0), smooth);

        //Animate vm show

        Vector3 showPose = (show) ? Vector3.zero : Vector3.one * -5;
        transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, showPose, (show) ? 0.25f : 1);
    }
}
