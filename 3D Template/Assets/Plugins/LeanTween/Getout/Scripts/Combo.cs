using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo : BarController
{
    protected override void Update()
    {
        hide = (value > 0);

        base.Update();

        Tks.OnValueChanged((p) => 
        {
            if (p)
            {
                //aniamte
                label.transform.localScale += Vector3.one * pop;
                label.transform.localRotation = Quaternion.Euler(label.transform.localRotation.eulerAngles + Vector3.forward * (pop * 10 * Tks.GetRandomSign()));
            }
        }, value, "Combo");

        //debug
        if (Input.GetKeyDown(KeyCode.F))
            value += 0.05f;
    }
}
