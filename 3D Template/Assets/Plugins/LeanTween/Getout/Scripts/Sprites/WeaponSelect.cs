using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour
{
    [Header("Inventory")]

    [SerializeField] Transform weaponsRapper;

    [Space(10)]

    [SerializeField] Tks.Collectable[] collectable;
    public static Tks.Collectable equpped;

    [SerializeField] static int index;

    [Header("Gui")]

    bool wheelEnabled = false;

    float wheelAlpha;

    bool deb;

    [SerializeField] CanvasGroup canvasGroup;

    [Header("Animte")]

    [SerializeField] float dur = 0.25f;

    void Awake()
    {
        transform.localScale = Vector3.zero;
        canvasGroup.alpha = 1;

        ValidSlot();
        Equip();
    }

    void Update()
    {
        equpped = collectable[index];

        if (Input.GetKeyDown(KeyCode.Mouse2))
            ToggleWheel(true);
        else if (Input.GetKeyUp(KeyCode.Mouse2))
            ToggleWheel(false);

        GetBtns();

        OnIndexChanged(() => {
            Equip();
        }, "WeaponSelect");

        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, wheelAlpha, 0.1f);

    }

    void ValidSlot()
    {
        for (int i = 0; i < collectable.Length; i++)
        {
            if (collectable[i].enabled) {
                index = i;
                break;
            }
        }
    }

    void Equip()
    {
        Viewmodel.show = false;

        //change item enabled
        for (int i = 0; i < weaponsRapper.childCount; i++)
        {
            GameObject thisCollectable = weaponsRapper.GetChild(i).gameObject;

            thisCollectable.SetActive(false);

            if (i != index) continue;
            deb = true;

            StartCoroutine(Tks.SetTimeout(() => {
                Viewmodel.show = true;

                deb = false;
            }, 250));

            thisCollectable.SetActive(true);
        }
    }

    static Dictionary<string, int> currentIndex = new Dictionary<string, int>();
    static Dictionary<string, int> lateIndex = new Dictionary<string, int>();

    public static void OnIndexChanged(System.Action action, string key)
    {
        if (!currentIndex.ContainsKey(key)) {
            currentIndex.Add(key, 0);
            lateIndex.Add(key, 0);
        }

        currentIndex[key] = index;

        if (currentIndex[key] != lateIndex[key]) {
            action.Invoke();
        }

        lateIndex[key] = index;
    }

    void GetBtns()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform thisSlot = transform.GetChild(i).GetChild(0);
            BtnContainer btnEvent = thisSlot.GetComponent<BtnContainer>();

            Image ico = thisSlot.GetChild(0).GetChild(0).GetComponent<Image>();
            ico.sprite = (InSlotRange(i)) ? collectable[i].ico : null;
            ico.gameObject.SetActive(InSlotRange(i) && collectable[i].enabled);

            if (!btnEvent.over) continue;

            if (Input.GetKeyUp(KeyCode.Mouse2) && InSlotRange(i)) {
                if (collectable[i].enabled)
                    index = i;
            }
        }
    }

    bool InSlotRange(int _index)
    {
        return (_index < collectable.Length);
    }

    void ToggleWheel(bool state)
    {
        if (Input.GetKey(KeyCode.LeftControl)) return;

        if (deb) return;

        wheelEnabled = state;

        Tks.cursorState = (wheelEnabled) ? CursorLockMode.None : CursorLockMode.Locked;

        Tks.freelook = !wheelEnabled;
        Tks.freeroam = !wheelEnabled;

        LeanTween.cancel(gameObject);
        transform.localScale = (!wheelEnabled) ? Vector3.one * 2 : Vector3.zero;

        LeanTween.scale(gameObject, (wheelEnabled) ? Vector3.one * 2 : Vector3.zero, wheelEnabled ? dur : dur / 0.75f)
            .setEase(wheelEnabled ? LeanTweenType.easeInOutCubic : LeanTweenType.easeInCubic);

        wheelAlpha = (wheelEnabled) ? 1 : 0;
    }
}
