using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Tower_shoot Turret;
    float startTime;


    public float damage;
    private float per;//penetration

    Rigidbody2D rb;
    private void Awake()
    {
        damage = Turret.bullet_damage;
        per = Turret.bullet_penetration;
        rb = GetComponent<Rigidbody2D>();
    }
    public void Init(float damage, float per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per > -1)
        {
            this.rb.velocity = dir * 15f;

        }

    }



    void Start()
    {
        startTime = Time.time;

        transform.Rotate(Vector3.forward * -90);
        transform.localScale = Vector3.one * Turret.bullet_size;
    }




    void Update()
    {
        transform.Translate(Vector2.up * Time.deltaTime * Turret.bullet_velocity);

        if (Time.time >= startTime + Turret.bullet_life_time)
        {
            Destroy(gameObject, 0f);
        }
    }



    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1)
            return;

        per--;

        if (per == -1)
        {   
            rb.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }


    }
}
