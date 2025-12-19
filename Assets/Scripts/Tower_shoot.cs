using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tower_shoot : MonoBehaviour
{

    [Header("References")]
    public GameObject projectile;
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private GameObject Arrow;
    [SerializeField] private LayerMask enemyMask;

    [Header("Attributes")]
    [SerializeField] float fire_rate;
    [SerializeField] private float targetingRange = 5f;


    private Transform target = null;
    private bool shooting = false;




    void Start() 
    {
    
    }








    private void Update()
    {

        if (target == null) //if no target, find a target
        {
            if (shooting == true) { shooting = false; CancelInvoke(); } //stop shooting if no target
            if (Arrow.activeInHierarchy == true) { Arrow.SetActive(false); } //hide arrow if no target

            FindTarget(); //will search for a target and set it to "target" variable    

            return;
        }
        else
        {
            if (shooting == false) { shooting = true; InvokeRepeating("SpawnBullet", fire_rate, fire_rate); } //start shooting if there is a target
            if (Arrow.activeInHierarchy == false) { Arrow.SetActive(true); } //show arrow if target
        }

        RotateTowardsTarget(); //rotate turret towards target


        if (!CheckTargetIsInRange()) //if target is out of range, lose current target
        {
            target = null;
        }

    }
    

    private void FindTarget() //target the first enemy in range in the ennemy layer mask
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        Debug.Log(hits.Length);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private void SpawnBullet() //spawn a bullet
    {
        Instantiate(projectile, transform.position, turretRotationPoint.rotation);
    }

    private void RotateTowardsTarget() //rotate turret towards target
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg ;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = targetRotation;
    }


    private bool CheckTargetIsInRange() //check if target is still in range
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }


    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }//draw max range when tower is selected in the editor





}
