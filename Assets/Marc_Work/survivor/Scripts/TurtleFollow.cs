using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleFollow : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 3f;
    public float minDistance = 10f; // Distance minimale à garder

    void Update()
    {
        if (player == null) return;
        Vector3 targetPos = player.position;
        targetPos.z = transform.position.z;

        // Vérifie la distance
        float distance = Vector3.Distance(transform.position, targetPos);
        if (distance > minDistance)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime / distance);
        }
    }
}
