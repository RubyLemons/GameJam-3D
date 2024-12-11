using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarController : MonoBehaviour
{
    [Range(0, 1)] public float value;

    [Range(-1, 1)] [SerializeField] float change = 0.01f;
    protected float initialChange;

    [Range(0, 1)] [SerializeField] float multiplierChange = 0.01f;

    [SerializeField] float loopSleep = 0.05f;

    protected float idleTime;
    [SerializeField] protected float sleepTime = 3.75f;

    public string baseKey;

    [Header("Gui")]

    [SerializeField] protected CanvasGroup group;

    [SerializeField] protected Image fill;
    [SerializeField] protected Image backdrop;

    [SerializeField] protected TextMeshProUGUI label;

    protected bool hide;

    [Header("Animate")]

    public float pop = 0.5f;

    protected Color initialColor;

    [SerializeField] bool baseAnimOnDecrease;

    protected virtual void Awake()
    {
        initialChange = change;
        initialColor = backdrop.color;

        StartCoroutine(Change(loopSleep * 1000));
    }

    protected virtual void Update()
    {
        value = Mathf.Clamp01(value);

        idleTime = (hide) ? idleTime + Time.deltaTime : 0;

        Tks.OnValueChanged((p) =>
        {
            if (!baseAnimOnDecrease ? p : !p)
            {
                idleTime = 0;

                //aniamte
                backdrop.color = Color.white;
                fill.color = Color.white;
            }
        }, value, baseKey);

        AnimateGui();
    }

    protected virtual void AnimateGui()
    {
        label.text = Mathf.FloorToInt(value * 100) + "%";

        //label
        label.transform.localScale = Vector3.Lerp(label.transform.localScale, Vector3.one, 0.05f);
        label.transform.localRotation = Quaternion.Lerp(label.transform.localRotation, Quaternion.identity, 0.05f);

        //fill backdrop
        backdrop.color = Color.Lerp(backdrop.color, initialColor, 0.05f);

        //fill
        fill.fillAmount = Mathf.Lerp(fill.fillAmount, value, 0.25f);
        fill.color = Color.Lerp(fill.color, new Color(initialColor.r, initialColor.g, initialColor.b, 1.0f), 0.05f);

        //group
        if (hide)
            group.alpha = 1;

        group.alpha = Mathf.Lerp(group.alpha, (hide) ? 1 : 0, 0.1f);
    }

    //run once
    public virtual IEnumerator Change(float sleep = 50)
    {
        if (idleTime > sleepTime)
        {
            value += change;

            change += (change * multiplierChange);
        }
        else
        {
            change = initialChange;
        }

        yield return new WaitForSeconds(sleep / 1000);

        StartCoroutine(Change(loopSleep * 1000));
    }
}
