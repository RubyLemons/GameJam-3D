using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Range(0, 1)] public static float health = 1;

    [Range(0, 1)] [SerializeField] float increase = 0.001f;
    float initialIncrease;

    float idleTime;
    [SerializeField] float sleepTime = 3.75f;

    [Header("Gui")]

    [SerializeField] CanvasGroup group;

    [SerializeField] Image fill;
    [SerializeField] Image backdrop;

    [SerializeField] CanvasGroup vignette;

    [Header("Animate")]

    [SerializeField] Transform camAnimated;

    [SerializeField] float pop = 25;

    Color initialColor;

    void Awake()
    {
        health = 1;

        initialIncrease = increase;
        initialColor = backdrop.color;

        StartCoroutine(Heal());
    }

    void Update()
    {
        health = Mathf.Clamp01(health);

        idleTime += Time.deltaTime;

        Tks.OnValueChanged((p) =>
        {
            if (!p)
            {
                idleTime = 0;

                //animate
                vignette.alpha = 1;

                backdrop.color = Color.white;
                fill.color = Color.white;

                camAnimated.localRotation *= Quaternion.Euler(Vector3.left * (pop / 2) + Vector3.forward * (pop));
            }
        }, health, "Health");

        AnimateGui();
    }

    void AnimateGui()
    {
        //vignette
        vignette.alpha = Mathf.Lerp(vignette.alpha, 0.0f, 0.05f);

        //fill backdrop
        backdrop.color = Color.Lerp(backdrop.color, initialColor, 0.05f);

        //fill
        fill.fillAmount = Mathf.Lerp(fill.fillAmount, health, 0.25f);
        fill.color = Color.Lerp(fill.color, new Color(initialColor.r, initialColor.g, initialColor.b, 1.0f), 0.05f);

        //group
        if (health < 1)
            group.alpha = 1;

        group.alpha = Mathf.Lerp(group.alpha, (health < 1) ? 1 : 0, 0.1f);

        //camera
        camAnimated.localRotation = Quaternion.Lerp(camAnimated.localRotation, Quaternion.identity, 0.175f);

        if (health < 0.375f)
            StartCoroutine(Tks.FlickerImg(backdrop, 100));
    }


    private IEnumerator Heal(float sleep = 50)
    {
        if (idleTime > sleepTime) {
            health += increase;

            increase += (increase / 10);
        }
        else {
            increase = initialIncrease;
        }

        yield return new WaitForSeconds(sleep / 1000);

        StartCoroutine(Heal());
    }
}
