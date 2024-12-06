using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool over;

    [Space(10)]

    [SerializeField] Image raycastTarget;
    [SerializeField] Image img;

    [Header("Aniamte")]

    [SerializeField] bool animate;

    [SerializeField] Color[] colors = new Color[2];

    [Header("Animate Wheel Btn")]

    [SerializeField] bool animateWheel;

    [SerializeField] float scale = 1.25f;

    bool deb;

    Vector3 goal;
    [SerializeField] [Range(0, 1)] float smooth = 0.25f;

    [SerializeField] float rise = 10;


    void Start()
    {
        img = (img == null) ?  GetComponent<Image>() : img;
        raycastTarget = (raycastTarget == null) ? GetComponent<Image>() : raycastTarget;

        if (!animate) return;

        img.color = colors[0];
    }

    void Update()
    {
        if (!animateWheel) return;
        raycastTarget.transform.localPosition = Vector3.Lerp(raycastTarget.transform.localPosition, goal, smooth);
    }


    public void OnPointerEnter(PointerEventData e)
    {
        over = true;

        if (!animate || deb) return;

        deb = true;
        StartCoroutine(Tks.SetTimeout(() => deb = false, 100));

        img.color = colors[1];
        raycastTarget.transform.localScale = Vector2.one * scale;


        if (!animateWheel) return;
        goal = raycastTarget.transform.up * rise;
    }
    
    public void OnPointerExit(PointerEventData e)
    {
        over = false;

        if (!animate) return;

        img.color = colors[0];
        raycastTarget.transform.localScale = Vector2.one;


        if (!animateWheel) return;
        goal = Vector3.zero;
    }
}
