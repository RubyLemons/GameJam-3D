using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Combo : MonoBehaviour
{
    public static int combo;

    float idleTime;

    [Header("Gui")]

    public CanvasGroup fill;
    public TextMeshProUGUI label;

    [Header("Animate")]

    [Space(10)]

    int current;
    int late;

    void Update()
    {
        idleTime += Time.deltaTime;

        OnComboChanged((increased) => {
            if (increased)
            {
                label.transform.localScale += Vector3.one;
            }
        });

        //aniamte
        label.transform.localScale = Vector3.Lerp(label.transform.localScale, Vector3.one, 0.05f);

        if (Input.GetKeyDown(KeyCode.F))
        {
            combo++;
        }
    }


    //once only
    private void OnComboChanged(System.Action<bool> action)
    {
        current = combo;

        if (current != late)
            action.Invoke(current > late);

        late = combo;
    }
}
