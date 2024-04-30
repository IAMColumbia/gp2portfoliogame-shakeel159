using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public float playerSpeed;

    Vector2 movement;
    public bool isShooting;
    public bool IsInteracting;

    public PlayerInput playerInput;
    public Rigidbody2D rb;
    public Camera cam;

    private Vector2 MousePos;
    private Vector3 rawInputMovement;


    public bool isSelectingWeaponPast;
    public bool isSelectingWeaponNext;
    public bool nextWeapon;
    public bool prevWeapon;
    Vector2 lookDir;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        isSelectingWeaponNext = Input.GetKeyUp(KeyCode.E);
        isSelectingWeaponPast = Input.GetKeyUp(KeyCode.Q);
        if (isSelectingWeaponNext == true)
        {
            nextWeapon = true;
        }
        if (isSelectingWeaponPast == true)
        {
            prevWeapon = true;
        }

        MousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log("F PRESSED");
            IsInteracting = true;
        }

        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.K))
        {
            Debug.Log("QUITTING");
            Application.Quit();
        }
    }
    private void FixedUpdate()
    {
        //transform.Translate(new Vector2(horizontalInput, verticalInput) * playerSpeed * Time.deltaTime);
        rb.MovePosition(rb.position + movement * playerSpeed * Time.fixedDeltaTime);
        lookDir = MousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

    }
    public Vector2 GetPlayerFacingDirection()
    {
        return lookDir.normalized;
    }
    public void IsMoving()
    {
    //    if (movement.x <= -0.1)
    //    {
    //        transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
    //        //launchOffset.transform.Rotate(0f, -180f, 0f);
    //    }
    //    if (movement.x >= 0.1)
    //    {
    //        transform.localScale = new Vector2((Mathf.Sign(rb.velocity.x)), transform.localScale.y);
    //        //transform.Rotate(0f, 180f, 0f);
    //        //launchOffset.transform.Rotate(0f, 180f, 0f);

    //    }
    //    if (movement.y <= -0.1)
    //    {
    //        transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.y)), transform.localScale.x);
    //        //launchOffset.transform.Rotate(0f, -180f, 0f);
    //    }
    //    if (movement.y >= 0.1)
    //    {
    //        transform.localScale = new Vector2((Mathf.Sign(rb.velocity.y)), transform.localScale.x);
    //        //transform.Rotate(0f, 180f, 0f);
    //        //launchOffset.transform.Rotate(0f, 180f, 0f);

    //    }
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isShooting = true;
        }
    }

}
