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
        transform.Translate(Vector2.right * Time.deltaTime * Turret.bullet_velocity);

        if (Time.time >= startTime + Turret.bullet_life_time)
        {
            Destroy(gameObject,0f);
        }
    }

}
