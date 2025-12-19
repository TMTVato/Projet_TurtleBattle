using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class content : MonoBehaviour
{
    
    
    public float velocity;
    [SerializeField] float life_time;

    float startTime;


   
    void Start()
    {
        startTime = Time.time;
    }
    


    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * velocity);

        if (Time.time >= startTime + life_time)
        {
            Destroy(gameObject,0f);
        }
    }

}
