using System.Collections;
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
    float movementSpeed = 150;

    CustomerWaitTime customerWaitTime;
    CustomerManager customerManager;

    Transform counterLookAt;
    bool counterLookState = true;
    bool lookSwitch = false;

    Animator animator;

    void Start()
    {
        AddCheckpoints();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        customerWaitTime = GetComponent<CustomerWaitTime>();
        customerManager = GameObject.Find("Script Managers").GetComponent<CustomerManager>();

        counterLookAt = checkpoints[6].transform;
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
        if(!lookSwitch)
        {
            targetRotation = Quaternion.LookRotation(new Vector3(checkpoints[currentCheckpoint].transform.position.x, transform.position.y, checkpoints[currentCheckpoint].transform.position.z) - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            
            animator.SetBool("Walking", true);
        }


        if(Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(checkpoints[currentCheckpoint].position.x, checkpoints[currentCheckpoint].position.z)) < 0.5f)
        {
            switch(currentCheckpoint)
            {
                case 0:
                    currentCheckpoint++;
                    break;

                case 1:
                    currentCheckpoint++;
                    break;

                case 2:

                    animator.SetBool("Walking", false);

                    rb.velocity = Vector3.zero;
                    
                    if(counterLookState)
                    {
                        Quaternion lookAtCounter = Quaternion.LookRotation(new Vector3(counterLookAt.position.x, transform.position.y, counterLookAt.position.z) - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, lookAtCounter, rotateSpeed * Time.deltaTime);
                        if(!lookSwitch)
                        {
                            StartCoroutine(LazyLookTimer());
                        }
                        lookSwitch = true;

                    }
                    else
                    {
                        customerWait = true;
                        rb.isKinematic = true;
                        gameObject.layer = LayerMask.NameToLayer("Interactable");
                        StartCoroutine(customerWaitTime.WaitTime());
                        lookSwitch = false;
                    }
                    break;

                case 3:
                    currentCheckpoint++;
                    break;

                case 4:
                    currentCheckpoint++;
                    break;

                case 5:
                    customerManager.CustomerSpawner();
                    Destroy(gameObject);
                    break;
                default:
                    Debug.LogWarning("Int out of switch bounds (CustomerBehavior/GoToCheckpoint)");
                    break;
            } 
        }
        if(!rb.isKinematic && !lookSwitch)
        {
            rb.velocity = transform.forward * movementSpeed * Time.deltaTime;   
        }
    }

    IEnumerator LazyLookTimer()
    {
        yield return new WaitForSeconds(1);

        counterLookState = false;

        StopCoroutine(LazyLookTimer());

    }
}
