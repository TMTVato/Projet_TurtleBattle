using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Scan pour les cibles dans la zone
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0f, targetLayer);
        nearestTarget = FindNearestTarget();
    }
    // Trouve la cible la plus proche parmi celles détectées
    Transform FindNearestTarget()
    {
        Transform nearest = null;
        float diff = 100;
        // Parcours toutes les cibles détectées
        foreach (RaycastHit2D hit in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = hit.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);
            if (curDiff < diff)
            {
                diff = curDiff;
                nearest = hit.transform;
            }
        }


        return nearest;
    }
}
