using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLogic : MonoBehaviour
{

    public Vector2 movementInput;
    public float speed = 5f;
    public Scanner scanner;

    Rigidbody2D rb;
    SpriteRenderer sR;
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    void Start()
    {

    }

    void Update()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        //bouge player
        rb.MovePosition(rb.position + movementInput.normalized * speed * Time.fixedDeltaTime);
    }

    //input system
    /*void OnMove(InputValue value)
    {
        //movementInput = value.Get<Vector2>();
    }*/

    private void LateUpdate()
    {
        //joue animation de marche
        animator.SetFloat("speed", movementInput.magnitude);


        if (movementInput.x != 0)
        {
            //flip sprite dépendant de la direction
            sR.flipX = movementInput.x < 0;
        }

    }
}
