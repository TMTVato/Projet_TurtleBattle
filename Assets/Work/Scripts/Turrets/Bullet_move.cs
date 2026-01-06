using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class content : MonoBehaviour
{
    [SerializeField] private Tower_shoot Turret;

    float startTime;

   
    void Start()
    {
        startTime = Time.time;
    }
    


    void Update()
    {
        // Déplace la balle en fonction de la vélocité définie dans Turret
        transform.Translate(Vector2.right * Time.deltaTime * Turret.bullet_velocity);

        // Détruit la balle après son temps de vie défini dans Turret
        if (Time.time >= startTime + Turret.bullet_life_time)
        {
            Destroy(gameObject,0f);
        }
    }

}
