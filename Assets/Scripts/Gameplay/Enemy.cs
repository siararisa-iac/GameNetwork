using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5.0f;

    private Transform target;

    private void OnEnable()
    {
        LookAtTarget();
    }

    private void LookAtTarget()
    {
        Quaternion newRotation;
        //Make the enemy face the target
        //If we do not have a specific target, make the object look at the center
        if(target == null)
        {
            newRotation = Quaternion.LookRotation(transform.position, Vector3.forward);
        }
        else
        {
            newRotation = Quaternion.LookRotation(transform.position - target.transform.position, Vector3.forward);
        }

        //Since rotation is only based on the z-axis
        newRotation.x = 0;
        newRotation.y = 0;
        transform.rotation = newRotation;
    }

    private void Move()
    {
        //Since the object has been rotated, just make it move 
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    private void Update()
    {
        Move();
    }
}
