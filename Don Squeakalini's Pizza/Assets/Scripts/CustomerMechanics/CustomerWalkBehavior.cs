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
    float movementSpeed = 75;

    CustomerWaitTime customerWaitTime;
    CustomerManager customerManager;

    Transform counterLookAt;
    [SerializeField] bool counterLookState = true;
    public bool lookSwitch = false;

    Animator animator;
    [SerializeField] GameObject handBox;

    void Start()
    {
        AddCheckpoints();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        customerWaitTime = GetComponent<CustomerWaitTime>();
        customerManager = GameObject.Find("Script Managers").GetComponent<CustomerManager>();

        counterLookAt = checkpoints[checkpoints.Count -1].transform;
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
            

            if(!animator.GetBool("Wrong") && !animator.GetBool("Correct"))
            {
                animator.SetBool("Walking", true);
            }
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
                    }
                    else
                    {
                        rb.isKinematic = true;
                        lookSwitch = false;

                        customerWait = true;
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
                    handBox.SetActive(false);
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
        lookSwitch = true;

        yield return new WaitForSeconds(1);

        gameObject.layer = LayerMask.NameToLayer("Interactable");

        counterLookState = false;
    }
}
