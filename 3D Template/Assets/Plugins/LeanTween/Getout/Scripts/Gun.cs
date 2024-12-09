using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    
    [SerializeField] Freelook freelook;
    [SerializeField] Animator camShake;

    [Space(10)]

    [SerializeField] TextMeshProUGUI ammoLabel;

    [SerializeField] GameObject bulletScar;
    [SerializeField] LayerMask layers;

    bool fireDeb;

    bool reloadAction;

    float elaspedTime;

    void Update()
    {
        //Ammo

        AmmoClamp();

        //Fire

        elaspedTime += Time.deltaTime;
        fireDeb = (elaspedTime < WeaponSelect.equpped.fireRate);

        bool ammoFull = WeaponSelect.equpped.ammo == WeaponSelect.equpped.ammoLimit;
        bool ammoEmpty = WeaponSelect.equpped.ammo <= 0;

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

        reloadAction = WeaponSelect.equpped.animator.GetCurrentAnimatorStateInfo(0).IsName("reload");

        if (Input.GetKeyDown(KeyCode.R) && !ammoFull) {
            Reload();
        }

        //recoil animated
        freelook.animatedRecoil.localRotation = Quaternion.Lerp(freelook.animatedRecoil.localRotation, Quaternion.identity, WeaponSelect.equpped.recoilSmooth);
    }

    void AmmoClamp()
    {
        WeaponSelect.equpped.ammo = Mathf.Clamp(WeaponSelect.equpped.ammo, 0, WeaponSelect.equpped.ammoLimit);

        ammoLabel.text = WeaponSelect.equpped.ammo.ToString();
    }

    public void Fire()
    {
        Recoil();

        WeaponSelect.equpped.animator.Play("fire", 0, 0.0f);
        camShake.Play("shake", 0, 0.0f);
        WeaponSelect.equpped.ammo -= 1;

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
        if (reloadAction) return;

        WeaponSelect.equpped.animator.Play("reload");

        float sleep = WeaponSelect.equpped.reloadTime * 1000;

        StartCoroutine(Tks.SetTimeout(() =>
            WeaponSelect.equpped.ammo = WeaponSelect.equpped.ammoLimit, sleep));
    }
}
