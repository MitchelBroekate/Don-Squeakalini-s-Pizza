
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

    [Header("Linked PlayerUI GameObject")]
    [SerializeField] GameObject playerUI;
    [SerializeField] GameObject tablet;

    public bool tabletMoveToLeft = false;
    public bool tabletMoveToRight = false;
 
    //Used inside script
    float xRotation = 0f;
    Vector2 _moveDirection;
    Rigidbody _rb;
    [SerializeField] bool _playerLock;


    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject quotaScreen;
    [SerializeField] GameObject lossScreen;
    [SerializeField] GameObject winScreen;
    #endregion 
    

    /// <summary>
    /// This function gets the ridgedbody component and locks the Cursor for the FPS Mode
    /// </summary>
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerUI.SetActive(false);
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

        TabletMove();
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

    //This function activates the tablets UI
    void OnTablet(InputValue value)
    {
        if(value.isPressed)
        {   
            //activate player UI
            if(!playerUI.activeInHierarchy)
            {
                playerUI.SetActive(true);

                tabletMoveToLeft = false;
                tabletMoveToRight = true;
            }
            else
            {
                tabletMoveToRight = false;
                tabletMoveToLeft = true;
            }

            //activate tablet slide
        }
    }
    void TabletMove()
    {
        if(tabletMoveToLeft)
        {
            if(tablet.transform.position.x > -1000)
            {
                tablet.transform.position -= new Vector3(4000, 0,0) * Time.deltaTime;
            }
            else
            {
                playerUI.SetActive(false);
                
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                LockPlayer(false);

                tabletMoveToLeft = false;
            }
        }
        if(tabletMoveToRight)
        {
            if(tablet.transform.position.x < 1000)
            {
                tablet.transform.position += new Vector3(4000, 0,0) * Time.deltaTime;
            }
            else 
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                LockPlayer(true);

                tabletMoveToRight = false;
            }
        }
    }

    void OnPause(InputValue value)
    {
        if(value.isPressed)
        {
            if(!pauseScreen.activeInHierarchy)
            {
                pauseScreen.SetActive(true);
                Time.timeScale = 0;
                LockPlayer(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
    void OnQuota(InputValue value)
    {
        if (value.isPressed)
        {
            if(quotaScreen.activeInHierarchy)
            {
                quotaScreen.SetActive(false);
            }
            else
            {
                quotaScreen.SetActive(true);
            }
        }
    }

    void OnLoss(InputValue value)
    {
        if (value.isPressed)
        {
            if(lossScreen.activeInHierarchy)
            {
                lossScreen.SetActive(false);
            }
            else
            {
                lossScreen.SetActive(true);
            }
        }
    }

    void OnWin(InputValue value)
    {
        if (value.isPressed)
        {
            if(winScreen.activeInHierarchy)
            {
                winScreen.SetActive(false);
            }
            else
            {
                winScreen.SetActive(true);
            }
        }
    }

    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
        LockPlayer(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
