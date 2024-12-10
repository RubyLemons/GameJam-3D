using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        [Space(10)]

        public Animator animator;

        [Header("Fire")]

        [Range(0, 1)] public float damage = 0.01f;

        [Space(10)]

        [Range(0, 1)] public float spread = 0.1f;
        [Range(0, 90)] public float recoil = 5;
        [Range(0, 1)] public float recoilSmooth = 0.025f;
        [Range(1, 10)] public int bullets = 1;

        [Space(10)]

        [Range(0, 10)] public float reloadTime;
        [Range(0, 10)] public float fireRate = 0.25f;

        [Header("Ammunation")]

        public int ammo;
        [Range(0, 999)] public int ammoLimit;
    }

    //run everyframe
    static Dictionary<Image, bool> flickerDebFlags = new Dictionary<Image, bool>();

    public static IEnumerator FlickerImg(Image img, float timeout = 50)
    {
        if (!flickerDebFlags.ContainsKey(img))
            flickerDebFlags.Add(img, false);

        if (flickerDebFlags[img]) yield break;
        flickerDebFlags[img] = true;

        img.gameObject.SetActive(false);
        yield return new WaitForSeconds(timeout / 1000);
        img.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeout / 1000);

        flickerDebFlags[img] = false;
    }


    //neg or pos

    public static int GetRandomSign()
    {
        int random = Random.Range(0, 2);

        int result = (random == 0) ? 1 : -1;

        return result;
    }

    //Changed listener (once only)

    static Dictionary<string, float> current = new Dictionary<string, float>();
    static Dictionary<string, float> late = new Dictionary<string, float>();

    public static void OnValueChanged(System.Action<bool> action, float real, string key)
    {
        if (!current.ContainsKey(key))
        {
            current.Add(key, 0);
            late.Add(key, 0);
        }

        current[key] = real;

        if (current[key] != late[key])
            action.Invoke(current[key] > late[key]);


        late[key] = real;
    }
}
