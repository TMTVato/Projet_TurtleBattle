using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        if (!GameManager.instance.isLive) return;

        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive) return;
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
        if (!GameManager.instance.isLive) return;
        //joue animation de marche
        animator.SetFloat("speed", movementInput.magnitude);


        if (movementInput.x != 0)
        {
            //flip sprite dépendant de la direction
            sR.flipX = movementInput.x < 0;
        }

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(!GameManager.instance.isLive) return;
        // Si la collision est avec la tortue, ne rien faire
        if (collision.gameObject.CompareTag("Turtle")) return;

        GameManager.instance.HP -= Time.deltaTime * 10;

        if (GameManager.instance.HP < 0) // player dead
        {
            for (int i = 2; i < transform.childCount; i++) // disable all children except the first two (shadow and area)
            {
                transform.GetChild(i).gameObject.SetActive(false);  
            }
            animator.SetTrigger("dead");
            GameManager.instance.GameOver();
        }
    }
}
