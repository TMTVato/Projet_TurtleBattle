using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tower_shoot : MonoBehaviour
{

    [Header("References")]
    public GameObject projectile;
    [SerializeField] private GameObject Arrow;
    [SerializeField] private Transform ArrowRotationPoint;
    [SerializeField] private LayerMask enemyMask;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float fire_rate;
    public float bullet_velocity;
    public float bullet_life_time;
    public float bullet_size;
    public float bullet_damage;
    public float bullet_penetration;

    private Transform target = null;
    private bool shooting = false;



    private void Update()
    {

        if (target != null) //if there is a target
        {
            StartShooting(); //Start shooting phase
            RotateTowardsTarget(); //rotate turret towards target

            if (!CheckTargetIsInRange()) //if target is out of range, lose current target
            {
                target = null;
            }
        }
        else //if there is no target
        {
            StopShooting(); //Stop shooting phase
            FindTarget(); //will search for a target. If it finds it, set it to "target" variable  

        }
    }
    

    private void FindTarget() //target the first enemy in range in the ennemy layer mask
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        //Debug.Log(hits.Length);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private void RotateTowardsTarget() //rotate turret towards target
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        ArrowRotationPoint.rotation = targetRotation;
    }

    private bool CheckTargetIsInRange() //check if target is still in range
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void StartShooting()
    {
        if (shooting == false) { shooting = true; InvokeRepeating("SpawnBullet", fire_rate, fire_rate); } //start shooting 
        if (Arrow.activeInHierarchy == false) { Arrow.SetActive(true); } //show arrow 
    } //Start shooting phase

    private void StopShooting()
    {
        if (shooting == true) { shooting = false; CancelInvoke(); } //stop shooting 
        if (Arrow.activeInHierarchy == true) { Arrow.SetActive(false); } //hide arrow 
    } //Stop shooting phas

    private void SpawnBullet() //spawn a bullet
    {
        Instantiate(projectile, transform.position, ArrowRotationPoint.rotation);
    }






    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }//draw max range when tower is selected in the editor


    public void SetFireRate(float newRate)
    {
        fire_rate = newRate;
        if (shooting)
        {
            CancelInvoke();
            InvokeRepeating("SpawnBullet", fire_rate, fire_rate);
        }
    }

    public float GetFireRate()
    {
        return fire_rate;
    }
}
