using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Combo : MonoBehaviour
{
    [Range(0, 1)] public static float points;

    [Range(0, 1)] [SerializeField] float decrease = 0.01f;
    float initialDecrease;

    float idleTime;
    [SerializeField] float sleepTime = 3.75f;

    [Header("Gui")]

    [SerializeField] CanvasGroup group;

    [SerializeField] Image fill;
    [SerializeField] Image backdrop;

    [SerializeField] TextMeshProUGUI label;

    [Header("Animate")]

    [SerializeField] float scalePop = 0.5f;

    Color initialColor;

    void Awake()
    {
        points = 0;

        initialDecrease = decrease;
        initialColor = backdrop.color;

        StartCoroutine(Decrease());
    }

    void Update()
    {
        points = Mathf.Clamp01(points);

        idleTime += Time.deltaTime;

        Tks.OnValueChanged((p) => 
        {
            if (p)
            {
                idleTime = 0;

                //aniamte
                label.transform.localScale += Vector3.one * scalePop;
                label.transform.localRotation = Quaternion.Euler(label.transform.localRotation.eulerAngles + Vector3.forward * (scalePop * 10 * Tks.GetRandomSign()));

                backdrop.color = Color.white;
                fill.color = Color.white;
            }
        }, points, "Combo");

        AnimateGui();

        //debug
        if (Input.GetKeyDown(KeyCode.F))
            points += 0.05f;
    }


    void AnimateGui()
    {
        label.text = Mathf.FloorToInt(points * 100) + "%";

        //label
        label.transform.localScale = Vector3.Lerp(label.transform.localScale, Vector3.one, 0.05f);
        label.transform.localRotation = Quaternion.Lerp(label.transform.localRotation, Quaternion.identity, 0.05f);

        //fill backdrop
        backdrop.color = Color.Lerp(backdrop.color, initialColor, 0.05f);

        //fill
        fill.fillAmount = Mathf.Lerp(fill.fillAmount, points, 0.25f);
        fill.color = Color.Lerp(fill.color, new Color(initialColor.r, initialColor.g, initialColor.b, 1.0f), 0.05f);

        //group
        if (points > 0)
            group.alpha = 1;

        group.alpha = Mathf.Lerp(group.alpha, (points > 0) ? 1 : 0, 0.1f);
    }


    //run once
    private IEnumerator Decrease(float sleep = 50)
    {
        if (idleTime > sleepTime) {
            points -= decrease;

            decrease += (decrease / 100);
        }
        else {
            decrease = initialDecrease;
        }

        yield return new WaitForSeconds(sleep / 1000);

        StartCoroutine(Decrease());
    }
}
