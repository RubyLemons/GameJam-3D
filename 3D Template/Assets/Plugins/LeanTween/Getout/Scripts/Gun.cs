using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    
    [SerializeField] Freelook freelook;

    [Space(10)]

    [SerializeField] Animator animator;

    [Space(10)]

    [SerializeField] TextMeshProUGUI ammoLabel;

    [SerializeField] GameObject bulletScar;
    [SerializeField] LayerMask layers;

    bool fireDeb;
    bool reloadDeb;

    bool reloadAction;

    float elaspedTime;

    void Update()
    {
        //Ammo

        AmmoClamp();

        //Fire

        elaspedTime += Time.deltaTime;
        fireDeb = (elaspedTime < WeaponSelect.equpped.fireRate);
        reloadDeb = (elaspedTime < WeaponSelect.equpped.reloadTime);

        bool ammoFull = WeaponSelect.equpped.ammo[0] == WeaponSelect.equpped.ammoLimit[0];
        bool ammoEmpty = WeaponSelect.equpped.ammo[0] <= 0;

        if (!fireDeb && !reloadAction && Input.GetMouseButton(0))
        {
            if (ammoEmpty) {
                Reload();
                return;
            }

            elaspedTime = 0;

            for (int i = 0; i < WeaponSelect.equpped.bullets; i++) {
                Fire();
            }
        }

        //Reload

        reloadAction = animator.GetCurrentAnimatorStateInfo(0).IsName("Reload");

        if (Input.GetKeyDown(KeyCode.R) && !ammoFull) {
            Reload();
        }

        //recoil animated
        freelook.animatedRecoil.localRotation = Quaternion.Lerp(freelook.animatedRecoil.localRotation, Quaternion.identity, WeaponSelect.equpped.recoilSmooth);
    }

    void AmmoClamp()
    {
        WeaponSelect.equpped.ammo[0] = Mathf.Clamp(WeaponSelect.equpped.ammo[0], 0, WeaponSelect.equpped.ammoLimit[0]);
        WeaponSelect.equpped.ammo[1] = Mathf.Clamp(WeaponSelect.equpped.ammo[1], 0, WeaponSelect.equpped.ammoLimit[1]);
    }

    public void Fire()
    {
        Recoil();

        animator.Play("Fire", 0, 0.0f);
        WeaponSelect.equpped.ammo[0] -= 1;

        Vector3 spread = (freelook.cam.transform.right * Random.Range(-WeaponSelect.equpped.spread, WeaponSelect.equpped.spread)) + (freelook.cam.transform.up * Random.Range(-WeaponSelect.equpped.spread, WeaponSelect.equpped.spread));
        bool ray = Physics.Raycast(freelook.cam.transform.position, freelook.cam.transform.forward + spread, out RaycastHit hit, 100, layers);

        if (ray) {
            BulletScar(hit);
        }
    }

    void BulletScar(RaycastHit _hit)
    {
        GameObject newBulletScar = Instantiate(bulletScar, null);
        newBulletScar.SetActive(true);
        newBulletScar.transform.localScale = Vector3.one * 0.00375f;

        newBulletScar.transform.position = _hit.point - (freelook.cam.transform.forward * 0.001f);
        newBulletScar.transform.rotation = Quaternion.LookRotation(_hit.normal) * Quaternion.Euler(Vector3.left * -90) * Quaternion.Euler(Vector3.up * Random.Range(0, 90));
    }

    void Recoil()
    {
        freelook.animatedRecoil.transform.localRotation *= Quaternion.Euler(Vector3.left * WeaponSelect.equpped.recoil / WeaponSelect.equpped.bullets);
    }

    void Reload()
    {
        if (reloadDeb || reloadAction || WeaponSelect.equpped.ammo[1] <= 0) return;

        animator.Play("Reload");

        StartCoroutine(Tks.SetTimeout(() => {
            for (int i = 0; i < WeaponSelect.equpped.ammoLimit[0]; i++)
            {
                if (WeaponSelect.equpped.ammo[1] > 0)
                {
                    WeaponSelect.equpped.ammo[1] -= 1;
                    WeaponSelect.equpped.ammo[0] += 1;
                }
            }

            ammoLabel.text = WeaponSelect.equpped.ammo[1] + " - " + WeaponSelect.equpped.ammo[0];
        }, WeaponSelect.equpped.reloadTime * 1000));
    }
}
