using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tks
{

    //PLAYER

    public static bool freelook = true; //COMEBACK
    public static bool freeroam = true;

    //

    public static CursorLockMode cursorState = CursorLockMode.Locked;

    public static IEnumerator SetTimeout(System.Action action, float delay, bool realTime = false)
    {
        if (!realTime)
            yield return new WaitForSeconds(delay / 1000);
        else
            yield return new WaitForSecondsRealtime(delay / 1000);
        action();
    }

    //

    public enum WeaponType
    {
        Revolver,
        MicroSMG,
        Shotgun,
    }

    [System.Serializable]
    public class Collectable
    {
        public WeaponType type;
        public Sprite ico;
        public bool enabled;

        [Header("Fire")]

        [Range(0, 1)] public float spread = 0.1f;
        [Range(0, 90)] public float recoil = 5;
        [Range(0, 1)] public float recoilSmooth = 0.025f;
        [Range(1, 10)] public int bullets = 1;

        [Space(10)]

        [Range(0, 10)] public float reloadTime;
        [Range(0, 10)] public float fireRate = 0.25f;

        [Header("Ammunation")]

        [Range(0, 999)] public int[] ammo = new int[2];
        [Range(0, 999)] public int[] ammoLimit = new int[2];
    }
}
