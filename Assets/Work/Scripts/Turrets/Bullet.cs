using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float per; // pénétration
    public float velocity;
    public float lifeTime;
    public float size;
    [SerializeField] private Tower_shoot Turret;

    Rigidbody2D rb;
    float startTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (Turret == null)
            Turret = GetComponentInParent<Tower_shoot>();

        // Vérification pour éviter le NullReferenceException
        if (Turret != null)
        {
            damage = Turret.bullet_damage;
            per = Turret.bullet_penetration;
        }
        else
        {
            Debug.LogWarning("Turret n'est pas assigné sur le prefab Bullet.");
        }
        AudioManager.instance.PlaySFX(AudioManager.SFX.Range);
    }

    // On reçoit toutes les stats bonus ici depuis le turret
    public void Init(float damage, float per, float velocity, float lifeTime, float size, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;
        this.velocity = velocity;
        this.lifeTime = lifeTime;
        this.size = size;

        rb.velocity = dir * velocity;
        transform.localScale = Vector3.one * size;
        startTime = Time.time;
    }
    // Spécifique pour la pelle
    public void InitShovel(float damage, float per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per > -1)
        {
            this.rb.velocity = dir * 15f;
        }

    }



    void Update()
    {

        if (Turret != null)
        {
            transform.Translate(Vector2.up * Time.deltaTime * Turret.bullet_velocity); //Déplace la balle 

            if (Time.time >= startTime + Turret.bullet_life_time)
            {
                Destroy(gameObject, 0f); //Détruit la balle après un certain temps
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1)
            return;

        // Appliquer les dégâts à l'ennemi
        var enemy = collision.GetComponent<EnemyLogic>();
        if (enemy != null)
            enemy.TakeDamage(damage);

        per--; // Réduire la pénétration = nb ennemis traversés

        if (per == -1) // Si la pénétration est épuisée, détruire la balle
        {
            rb.velocity = Vector2.zero;
            Destroy(gameObject);
        }
    }
}
