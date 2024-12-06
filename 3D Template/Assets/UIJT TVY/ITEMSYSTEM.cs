using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ITEMSYSTEM : MonoBehaviour
{
    //old stuff (subject to change)
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public GameObject bulletPrefab2;
    public GameObject bulletPrefab3;
    public float bulletSpeed = 10;
    public float distance = 1;
    bool firestate = true;
    float elaspedTime;
    public float delayTime = 0.1f;
    public float BulletCount = 13;
    //

    bool aim1 = false;
    public Camera cam1;
    float camfov = 60;
    public ParticleSystem ParticleSystem1;
    

    //stats

    public Animator animator;
    public int dmg = 5;
    public int Rate = 5;
    public int spread = 5;
    //

    public bool cooldown = false;
    // auto depicts wether or no the gun will fire after a shot and cooldown(rate of fire)
    public bool auto = true;
    public weaponType PREVwepon;
    public weaponType currentWeapon;
    public int weaponnumber = 0;
    public enum weaponType
    {
        smg,
        shotgun,
        revolver

    }

    void Start()
    {


        weaponnumber = 1;
    }


    void Update()
    {
       
        if (weaponnumber > 3)
        {
            currentWeapon = weaponType.revolver;
        }

        //send the check
        if (weaponnumber == 1)
        {
            currentWeapon = weaponType.revolver;
        }

        if (weaponnumber == 2)
        {
            currentWeapon = weaponType.shotgun;

        }

        if (weaponnumber == 3)
        {
            currentWeapon = weaponType.smg;

        }

        if (currentWeapon != PREVwepon)
        {
            cooldown = true;

        }
         PREVwepon = currentWeapon;

        //check what weapon i have and set it up
        if (currentWeapon == weaponType.smg && cooldown == false)
        {
            animator.SetTrigger("PutAway");
            StartCoroutine(Delay1());

            IEnumerator Delay1()
            {
                cooldown = true;
                yield return new WaitForSeconds(0.45f);
                transform.Find("SHOTGUN").gameObject.SetActive(false);
                transform.Find("SMG").gameObject.SetActive(false);
                transform.Find("REVOLVER").gameObject.SetActive(true);
                //need to acces fire state
                animator.SetTrigger("PullOut");//NEED MODEL FOR ANIM

            }


        }

        if (currentWeapon == weaponType.shotgun && cooldown == false)
        {
            animator.Play("PutAway");
            StartCoroutine(Delay2());

            IEnumerator Delay2()
            {
                cooldown = true;
                yield return new WaitForSeconds(0.45f);
                transform.Find("SHOTGUN").gameObject.SetActive(false);
                transform.Find("SMG").gameObject.SetActive(false);
                transform.Find("REVOLVER").gameObject.SetActive(true);
                
                //need to acces fire state
                animator.Play("PullOut");//NEED MODEL FOR ANIM

            }

        }

        if (currentWeapon == weaponType.revolver && cooldown == false)
        {

            animator.Play("PutAway");
            StartCoroutine(Delay3());

            IEnumerator Delay3()
            {
                cooldown = true;
                yield return new WaitForSeconds(0.45f);
                transform.Find("SHOTGUN").gameObject.SetActive(false);
                transform.Find("SMG").gameObject.SetActive(false);
                transform.Find("REVOLVER").gameObject.SetActive(true);
               
                //need to acces fire state
                animator.Play("PullOut");//NEED MODEL FOR ANIM

            }



        }
    }
}
    

