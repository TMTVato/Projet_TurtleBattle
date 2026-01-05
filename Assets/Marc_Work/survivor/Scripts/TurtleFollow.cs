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

        //Rotate
        float angle = Mathf.Atan2(player.position.y - transform.position.y, player.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle + 90));
        transform.rotation = targetRotation;
    }
}
