
using UnityEngine;
using UnityEngine.InputSystem;

public class PizzariaController : MonoBehaviour
{
    #region Variables
    [Header("Config Movement Variables")]
    [SerializeField] float moveSpeed;

    [Header("Config Camera Variables")]
    public float mouseDPI;

    [Header("Linked Camera Variables")]
    [SerializeField] Transform cam;
 
    //Used inside script
    float xRotation = 0f;
    Vector2 _moveDirection;
    Rigidbody _rb;
    bool _playerLock;
    #endregion 
    

    /// <summary>
    /// This function gets the ridgedbody component and locks the Cursor for the FPS Mode
    /// </summary>
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// The Update function updates the walk and camera function for the player movement, also checks if the movement is locked
    /// </summary>
    private void Update()
    {
        if(!_playerLock)
        {
            Walk();
            CameraMovement();  
        }
        else
        {
            _rb.velocity = Vector3.zero;
        }
    }  

    /// <summary>
    /// This function sets the velocity for any walking direction.
    /// </summary>
    void Walk()
    {
        Vector3 playerV = new Vector3(_moveDirection.x * moveSpeed, _rb.velocity.y, _moveDirection.y * moveSpeed);
        _rb.velocity = transform.TransformDirection(playerV);
    } 

    /// <summary>
    /// This function sets the movement vector2 in a vector2 variable.
    /// </summary>
    /// <param name="value"></param>
    void OnMovement(InputValue value)
    {
        _moveDirection = value.Get<Vector2>();
    }

    /// <summary>
    /// this function sets the movement for the camera and sets the clamp for the camera movement
    /// </summary>
    void CameraMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseDPI * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseDPI * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);

    }

    /// <summary>
    /// This Function sets the state of the _playerLock bool
    /// </summary>
    /// <param name="allowState"></param>
    public void LockPlayer(bool allowState)
    {
        _playerLock = allowState;
    }

    public bool PlayerLock
    {
        get { return _playerLock; }
    }
}
