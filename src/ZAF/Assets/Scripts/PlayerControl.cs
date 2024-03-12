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
    public bool inStun = false;


    public PlayerInput playerInput;
    public Rigidbody2D rb;
    public Camera cam;

    private Vector2 MousePos;
    private Vector3 rawInputMovement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        MousePos = cam.ScreenToWorldPoint(Input.mousePosition);

    }
    private void FixedUpdate()
    {
        //transform.Translate(new Vector2(horizontalInput, verticalInput) * playerSpeed * Time.deltaTime);
        rb.MovePosition(rb.position + movement * playerSpeed * Time.fixedDeltaTime);
        Vector2 lookDir = MousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
    public void IsMoving()
    {
        if (movement.x <= -0.1)
        {
            transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
            //launchOffset.transform.Rotate(0f, -180f, 0f);
        }
        if (movement.x >= 0.1)
        {
            transform.localScale = new Vector2((Mathf.Sign(rb.velocity.x)), transform.localScale.y);
            //transform.Rotate(0f, 180f, 0f);
            //launchOffset.transform.Rotate(0f, 180f, 0f);

        }
        if (movement.y <= -0.1)
        {
            transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.y)), transform.localScale.x);
            //launchOffset.transform.Rotate(0f, -180f, 0f);
        }
        if (movement.y >= 0.1)
        {
            transform.localScale = new Vector2((Mathf.Sign(rb.velocity.y)), transform.localScale.x);
            //transform.Rotate(0f, 180f, 0f);
            //launchOffset.transform.Rotate(0f, 180f, 0f);

        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {

    }
    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
    }




}
