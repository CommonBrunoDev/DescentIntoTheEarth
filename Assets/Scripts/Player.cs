// Mouse movement (these are captured in an earlier function within Update)
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class Player : MonoBehaviour, IDamageable
{
    float accMouseX = 0f;
    float accMouseY = 0f;

    float verticalRotation = 0f;
    float horizontalRotation = 0f;
    float rollRotation = 0f;

    Vector3 CurrentSpeed = Vector3.zero;

    [Header("Camera")]
    [SerializeField] float MouseSens = 5f;
    [SerializeField] float MouseSnap = 20f;
    [Space(20)]

    [Header("Movement")]
    [SerializeField] float AccelSpeed = 2f;
    [SerializeField] float MaxSpeed = 20f;
    [SerializeField][Range(0f,1f)] float Friction = 0.9f;
    [Space(20)]

    [Header("Shooting prefabs")]
    [SerializeField] Transform[] ShootPoints;
    [SerializeField] Rocket RocketRef;
    [SerializeField] LaserBullet LaserRef;
    [SerializeField] float laserDelay;
    private float laserTimer = 0;
    private int laserPointInd = 0;

    public InputAction playerControls;

    

    Rigidbody rb;
    private static Player instance;
    public static Player Instance
    { get{ return instance; } }

    public float health { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        InputManager.OnInputModeChanged += SetInput;
    }
    private void OnDisable()
    {
        InputManager.OnInputModeChanged -= SetInput;
    }
    private void SetInput(InputManager action)
    {
        InputMode test = InputMode.Keyboard;
    }

    private void Update()
    {
        ProcessLook();
        ProcessMovement();
        ProcessShooting();

        
    }

    #region Processes
    void ProcessLook()
    {
        Vector2 inputLook = Vector2.zero;
        if (InputManager.Instance.controllerMode)
        {
            inputLook.x = Gamepad.current.leftStick.ReadValue().x;
            inputLook.y = Gamepad.current.leftStick.ReadValue().y;
        }
        else
        {
            inputLook.x = Input.GetAxis("Mouse X"); // Mouse X
            inputLook.y = Input.GetAxis("Mouse Y"); // Mouse Y
        }

        accMouseX = Mathf.Lerp(accMouseX, inputLook.x, MouseSnap * Time.deltaTime);
        accMouseY = Mathf.Lerp(accMouseY, inputLook.y, MouseSnap * Time.deltaTime);

        float mouseX = accMouseX * MouseSens * 100f * Time.deltaTime;
        float mouseY = accMouseY * MouseSens * 100f * Time.deltaTime;

        verticalRotation = -mouseY;
        if (Input.GetKey(KeyCode.Mouse1))
        {
            rollRotation = -mouseX;
            horizontalRotation = 0;
        }
        else
        {
            horizontalRotation = mouseX;
            rollRotation = 0;
        }

        rb.transform.Rotate(verticalRotation, horizontalRotation, rollRotation, Space.Self);
    }
    void ProcessMovement()
    {
        float xAxis = 0;
        if (GameManager.Instance.controllerMode)
        { xAxis = (Input.GetButton("joystick button 1") ? 1 : 0) - (Input.GetKey("joystick button 2") ? 1 : 0); }
        else
        { xAxis = Input.GetAxis("Horizontal"); }
        
        CurrentSpeed.x = CalculateSpeed(xAxis, CurrentSpeed.x);

        CurrentSpeed.y = CalculateSpeed(Input.GetAxis("Vertical"), CurrentSpeed.y);
        CurrentSpeed.z = CalculateSpeed(Input.GetAxis("Distal"), CurrentSpeed.z);

        Vector3 newPosition = rb.transform.TransformDirection(CurrentSpeed * 20);
        rb.linearVelocity = newPosition; 
            
    }
    void ProcessShooting()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var r = Instantiate(RocketRef, ShootPoints[Random.Range(0,ShootPoints.Length)].position, transform.rotation);
            r.SetDirection(transform.forward, gameObject);
        }

        if (Input.GetKey(KeyCode.E) && laserTimer <= 0)
        {
            var l = Instantiate(LaserRef, ShootPoints[laserPointInd].position, transform.rotation);
            l.SetDirection(transform.forward, gameObject);
            laserTimer = laserDelay;
            laserPointInd = (laserPointInd + 1) % ShootPoints.Length;
        }

        if (laserTimer > 0)
            laserTimer -= Time.deltaTime;
    }
    #endregion

    #region Useful Methods
    float CalculateSpeed(float input,float speed)
    {
        if (input == 0)
        {
            if (speed > 0.01f)
                return speed - AccelSpeed * Time.deltaTime * Friction;
            if (speed < -0.01f)
                return speed + AccelSpeed * Time.deltaTime * Friction;
            else
                return 0;
        }
        else
            return Mathf.Clamp(speed + input * Time.deltaTime * AccelSpeed, -MaxSpeed, MaxSpeed);
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Player took damage: " + damage);
    }

    public void Destroy()
    {
        Debug.Log("De de destructionnnn");
    }
    #endregion
}