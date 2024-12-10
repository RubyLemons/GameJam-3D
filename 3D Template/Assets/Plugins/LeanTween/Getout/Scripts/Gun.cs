using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    
    [SerializeField] Freelook freelook;

    [SerializeField] Transform camAnimated;
    [SerializeField] Animator camShake;

    [Space(10)]

    [SerializeField] TextMeshProUGUI ammoLabel;

    [SerializeField] GameObject bulletScar;
    [SerializeField] LayerMask layers;

    float elaspedTime;

    [Space(10)]

    bool ammoFull;
    bool ammoEmpty;


    bool fireDeb;
    bool reloadDeb;

    bool fireAction;
    bool reloadAction;

    void Update()
    {
        //Ammo

        AmmoClamp();

        reloadAction = WeaponSelect.equpped.animator.GetCurrentAnimatorStateInfo(0).IsName("reload");
        fireAction = WeaponSelect.equpped.animator.GetCurrentAnimatorStateInfo(0).IsName("fire");

        //Fire

        elaspedTime += Time.deltaTime;
        fireDeb = (elaspedTime < WeaponSelect.equpped.fireRate);

        ammoFull = WeaponSelect.equpped.ammo == WeaponSelect.equpped.ammoLimit;
        ammoEmpty = WeaponSelect.equpped.ammo <= 0;

        if (Input.GetMouseButton(0) && !ammoEmpty && (!fireDeb && !reloadAction)) {

            elaspedTime = 0;

            for (int i = 0; i < WeaponSelect.equpped.bullets; i++)
                Fire();
        }

        if (Input.GetMouseButtonDown(0) && ammoEmpty)
            Reload();

        //Reload

        if (Input.GetKeyDown(KeyCode.R))
            Reload();

        //cam animated recoil
        camAnimated.localRotation = Quaternion.Lerp(camAnimated.localRotation, Quaternion.identity, WeaponSelect.equpped.recoilSmooth);
    }

    void AmmoClamp()
    {
        WeaponSelect.equpped.ammo = Mathf.Clamp(WeaponSelect.equpped.ammo, 0, WeaponSelect.equpped.ammoLimit);

        ammoLabel.text = WeaponSelect.equpped.ammo.ToString();
    }

    public void Fire()
    {
        //aniamte

        WeaponSelect.equpped.animator.Play("fire", 0, 0.0f);
        camShake.Play("shake", 0, 0.0f);

        WeaponSelect.equpped.ammo -= 1;

        camAnimated.transform.localRotation *= Quaternion.Euler(Vector3.left * WeaponSelect.equpped.recoil / WeaponSelect.equpped.bullets);


        //Raycast

        Vector3 spread = (freelook.cam.transform.right * Random.Range((float)-WeaponSelect.equpped.spread, WeaponSelect.equpped.spread)) + (freelook.cam.transform.up * Random.Range((float)-WeaponSelect.equpped.spread, WeaponSelect.equpped.spread));
        bool ray = Physics.Raycast(freelook.cam.transform.position, freelook.cam.transform.forward + spread, out RaycastHit hit, 100, layers);


        //Attack

        if (ray)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();

            bool head = (enemy == null && hit.collider.tag == "Enemy");
            enemy = (head) ? hit.collider.transform.parent.GetComponent<Enemy>() : enemy;

            if (enemy)
            {
                enemy.health -= WeaponSelect.equpped.damage * ((head) ? 2 : 1);
            }
            else
            {
                BulletScar(hit);
            }
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


    void Reload()
    {
        if (reloadDeb || reloadAction || fireAction || ammoFull) return;
        reloadDeb = true;

        WeaponSelect.equpped.animator.Play("reload");

        float sleep = WeaponSelect.equpped.reloadTime * 1000;

        StartCoroutine(Tks.SetTimeout(() => {
            WeaponSelect.equpped.ammo = WeaponSelect.equpped.ammoLimit;
            reloadDeb = false;
        }, sleep));
    }
}
