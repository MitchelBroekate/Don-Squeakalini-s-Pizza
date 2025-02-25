using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerWalkBehavior : MonoBehaviour
{
    Transform checkpointParent;
    [SerializeField] List<Transform> checkpoints = new();
    [SerializeField] int currentCheckpoint;
    bool reachedLastCheckpoint = false;
    Rigidbody rb;

    Quaternion targetRotation;
    [SerializeField] float rotateSpeed;

    void Start()
    {
        AddCheckpoints();
    }

    void Update()
    {
        if(!reachedLastCheckpoint)
        {
            GoToCheckpoint();
        }
    }

    void AddCheckpoints()
    {
        checkpointParent = GameObject.Find("WalkCheckpoints").transform;

        for (int i = 0; i < checkpointParent.childCount; i++)
        {
            checkpoints.Add(checkpointParent.GetChild(i).transform);
        }

        currentCheckpoint = 0;
    }

    void GoToCheckpoint()
    {
        targetRotation = Quaternion.LookRotation(new Vector3(checkpoints[currentCheckpoint].transform.position.x, transform.position.y, checkpoints[currentCheckpoint].transform.position.z) - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        //walk

        if(Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(checkpoints[currentCheckpoint].position.x, checkpoints[currentCheckpoint].position.z)) < 1)
        {
            if(currentCheckpoint == 0)
            {
                currentCheckpoint++;
            }
            else if(currentCheckpoint == 1)
            {
                reachedLastCheckpoint = true;
                rb.velocity = Vector3.zero;
            }

            
        }

    }
}
