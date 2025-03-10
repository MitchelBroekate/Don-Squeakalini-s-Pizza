using System.Collections.Generic;
using UnityEngine;

public class CustomerWalkBehavior : MonoBehaviour
{
    public int currentCheckpoint;
    [SerializeField] List<Transform> checkpoints = new();
    Transform checkpointParent;
    public bool customerWait = false;
    bool reachedGround = false;
    public Rigidbody rb;

    Quaternion targetRotation;
    [SerializeField] float rotateSpeed;
    float movementSpeed = 100;

    CustomerWaitTime customerWaitTime;
    CustomerManager customerManager;

    void Start()
    {
        AddCheckpoints();
        rb = GetComponent<Rigidbody>();
        customerWaitTime = GetComponent<CustomerWaitTime>();
        customerManager = GameObject.Find("Script Managers").GetComponent<CustomerManager>();
    }

    void FixedUpdate()
    {
        if(!reachedGround)
        {
            rb.velocity = -transform.up * movementSpeed * Time.deltaTime; 
        }
        else
        {
            if(!customerWait)
            {
                GoToCheckpoint();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            reachedGround = true;
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

        if(Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(checkpoints[currentCheckpoint].position.x, checkpoints[currentCheckpoint].position.z)) < 0.5f)
        {
            switch(currentCheckpoint)
            {
                case 0:
                    currentCheckpoint++;
                    break;
                case 1:
                    customerWait = true;

                    StartCoroutine(customerWaitTime.WaitTime());

                    rb.velocity = Vector3.zero;
                    rb.isKinematic = true;

                    gameObject.layer = LayerMask.NameToLayer("Interactable");

                    break;
                case 2:
                    currentCheckpoint++;
                    break;
                case 3:
                    customerManager.CustomerSpawner();
                    Destroy(gameObject);
                    break;
                default:
                    Debug.LogWarning("Int out of switch bounds (CustomerBehavior/GoToCheckpoint)");
                    break;
            } 
        }

        rb.velocity = transform.forward * movementSpeed * Time.deltaTime;   
    }
}
