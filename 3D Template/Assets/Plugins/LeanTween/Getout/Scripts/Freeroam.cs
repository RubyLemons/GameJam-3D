using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeroam : MonoBehaviour
{

    [SerializeField] CharacterController controller;

    [SerializeField] Freelook freelook;

    [Header("Movement")]

    [SerializeField] float speed = 3.5f;
    [SerializeField] float speedFast = 3.5f;

    [SerializeField] float smooth = 0.375f;

    Vector3 moveInput;
    public bool fast;
    public bool moving;
    public bool backwards;

    float initialFov;
    [SerializeField] float fovRun = 10f;
    [SerializeField] float fovSmooth = 0.2f;

    float movementBlend;

    [Header("Slide Movement")]

    [SerializeField] float targetSlideForce = 22.5f;
    float slideForce;

    [SerializeField][Range(0, 1)] float slideSmooth = 0.1f;

    bool slideDeb;
    [SerializeField] float slideTime = 475;

    [Space(10)]

    [SerializeField] float dropHeight = -0.375f;
    [SerializeField] float slideTilt = 7.5f;

    [SerializeField] float animSpeed = 0.175f;

    [Header("Gravity")]

    [SerializeField] float gravityScale = 35;
    [SerializeField] float jumpHeight = 7.375f;
    float velo = -2.5f;

    public bool ground;

    void Start()
    {
        initialFov = freelook.cam.fieldOfView;
        StopSlide();
    }

    void Update()
    {
        //Move

        Vector3 move = MoveInput() + Gravity();
        controller.Move(move);

        SlideInput();

        //Jump

        RaycastHit hit;
        ground = Physics.BoxCast(transform.position, new Vector3(controller.radius - 0.1f, controller.radius / 2, controller.radius - 0.1f), Vector3.down, out hit, Quaternion.identity, controller.height / 2);

        if (Input.GetKeyDown(KeyCode.Space) && ground && Tks.freeroam)
            velo = jumpHeight;

        //Fast

        freelook.cam.fieldOfView = Mathf.Lerp(freelook.cam.fieldOfView, initialFov + (fast || slideDeb ? fovRun : 0), fovSmooth);

        fast = (Input.GetKey(KeyCode.LeftShift) && moving && !backwards);

        //aniamte vm

        movementBlend = Mathf.Lerp(movementBlend, (fast && ground) ? 1 : 0, 0.45f);

        WeaponSelect.equpped.animator.SetFloat("movement", movementBlend);
    }


    Vector3 MoveInput()
    {
        Vector3 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveInput = Vector3.Lerp(moveInput, Vector3.Normalize(direction), smooth);

        moving = direction.magnitude > 0;
        backwards = direction.y < 0;


        float moveSpeed = (speed + (fast ? speedFast : 0));

        moveInput.y = (slideDeb) ? 0 : moveInput.y; //force move if sliding
        moveInput.x = (slideDeb) ? 0 : moveInput.x; //force move if sliding

        Vector3 top = transform.forward * (moveInput.y * moveSpeed + slideForce) * Time.deltaTime;
        Vector3 left = transform.right * (moveInput.x * moveSpeed) * Time.deltaTime;

        return Tks.freeroam ? (top + left) : Vector3.zero;
    }

    void SlideInput()
    {
        if (!Tks.freeroam) return;

        if (Input.GetKeyUp(KeyCode.C)) //COMEBACK
            StopSlide();

        if (Input.GetKeyDown(KeyCode.C) && !slideDeb && fast) {
            slideDeb = true;
            slideForce = targetSlideForce;

            LeanTween.moveLocal(freelook.cam.gameObject, Vector3.up * dropHeight, animSpeed).setEaseOutCubic(); ;
            LeanTween.rotateLocal(freelook.animatedSlide.gameObject, Vector3.forward * slideTilt, animSpeed).setEaseOutCubic();

            StartCoroutine(Tks.SetTimeout(() => {
                StopSlide();
            }, slideTime));
        }

        if (slideForce < 0.01f) //COMEBACK
            StopSlide();

        slideForce = Mathf.Lerp(slideForce, 0, slideSmooth);
    }

    void StopSlide()
    {
        slideDeb = false;

        LeanTween.moveLocal(freelook.cam.gameObject, Vector3.up * 0.75f, animSpeed).setEaseOutCubic(); ; //COMEBACK
        LeanTween.rotateLocal(freelook.animatedSlide.gameObject, Vector3.zero, animSpeed).setEaseOutCubic();
    }

    Vector3 Gravity()
    {
        velo = Mathf.Clamp(velo, -25, 25);

        if (!ground)
            velo -= gravityScale * Time.deltaTime;
        else if (ground && velo < 0)
            velo = -2.5f;

        return (transform.up * velo) * Time.deltaTime;
    }
}
