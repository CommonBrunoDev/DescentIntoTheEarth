// Mouse movement (these are captured in an earlier function within Update)
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
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
    public float energyAmount = 100;
    public float rocketAmount = 100;
    [SerializeField] Transform[] ShootPoints;
    [SerializeField] Rocket RocketRef;
    [SerializeField] LaserBullet LaserRef;
    [SerializeField] float laserDelay;
    private float laserTimer = 0;
    [Space(20)]

    [Header("UI")]
    [SerializeField] Image slimeScreen;
    [SerializeField] TextMeshProUGUI missilesLeft;
    [SerializeField] TextMeshProUGUI lasersLeft;
    [SerializeField] TextMeshProUGUI scoreDisplay;
    [SerializeField] RawImage shipIcon;
    [SerializeField] TextMeshProUGUI shipHp;
    [SerializeField] TextMeshProUGUI headlightsOnOff;
    [SerializeField] TextMeshProUGUI enemiesLeft;
    [SerializeField] TextMeshProUGUI shipStatus;
    [SerializeField] TextMeshProUGUI timePassed;
    [SerializeField] TextMeshProUGUI energyLeft;

    [SerializeField] GameObject bombManager;
    [SerializeField] TextMeshProUGUI bombTimer;

    public GameObject enemyEscaped;
    public float enemyVisTime;
    public float enemyVisTimer = 0;

    public GameObject cantEscape;
    public float escapeVisTime;
    public float escapeVisTimer = 0;

    public TextMeshProUGUI bombPopup;
    [Space(20)]

    [Header("Other")] //HL = HeadLights
    [SerializeField] Light HLOn;
    [SerializeField] float HLEnergyUsage = 40;
    private float HLEnergyTimer = 0;

    public InputMode playerControls;

    

    Rigidbody rb;
    private static Player instance;
    public static Player Instance
    { get{ return instance; } }

    public float health { get; private set; }
    public float HP { set { health = value; } }

    private void Awake()
    {
        cantEscape.SetActive(false);
        enemyEscaped.SetActive(false);
        bombPopup.gameObject.SetActive(false);
        bombManager.SetActive(false);

        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        HLEnergyTimer = HLEnergyUsage;
        HP = 100;
    }

    private void OnEnable()
    {
        InputManager.OnInputModeChanged += SetInput;
    }
    private void OnDisable()
    {
        InputManager.OnInputModeChanged -= SetInput;
    }
    private void SetInput(InputMode action)
    {
        playerControls = action;
    }

    private void Update()
    {
        ProcessLook();
        ProcessMovement();
        ProcessShooting();
        ProcessHeadLights();
        ProcessUI();
        ProcessTimers();
    }

    #region Processes
    void ProcessLook()
    {
        Vector2 inputLook = Vector2.zero;
        if (playerControls == InputMode.Controller)
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

        float rotationX = accMouseX * MouseSens * 100f * Time.deltaTime;
        float rotationY = accMouseY * MouseSens * 100f * Time.deltaTime;

        verticalRotation = -rotationY;
        if (playerControls == InputMode.Controller && Input.GetAxis("Joystick Rot ZL") + Input.GetAxis("Joystick Rot ZR") != 0)
        {
            rollRotation = (Input.GetAxis("Joystick Rot ZR") - Input.GetAxis("Joystick Rot ZL")) * 100f * Time.deltaTime;
            horizontalRotation = rotationX;
        }
        else if (playerControls == InputMode.Keyboard && Input.GetKey(KeyCode.Mouse1))
        {
            rollRotation = -rotationX;
            horizontalRotation = 0;
        }
        else
        {
            horizontalRotation = rotationX;
            rollRotation = 0;
        }
        
        rb.transform.Rotate(verticalRotation, horizontalRotation, rollRotation, Space.Self);
    }
    void ProcessMovement()
    {
        if (playerControls == InputMode.Controller)
        {
            CurrentSpeed.x = CalculateSpeed(Input.GetAxis("Joystick Horizontal"), CurrentSpeed.x);
            CurrentSpeed.y = CalculateSpeed(Input.GetAxis("Joystick Vertical"), CurrentSpeed.y);
            CurrentSpeed.z = CalculateSpeed((Input.GetKey("joystick button 1") ? 1 : 0) - (Input.GetKey("joystick button 0") ? 1 : 0), CurrentSpeed.z);
        }
        else
        { 
            CurrentSpeed.x = CalculateSpeed(Input.GetAxis("Horizontal"), CurrentSpeed.x);
            CurrentSpeed.y = CalculateSpeed(Input.GetAxis("Vertical"), CurrentSpeed.y);
            CurrentSpeed.z = CalculateSpeed(Input.GetAxis("Distal"), CurrentSpeed.z);
        }

        Vector3 newPosition = rb.transform.TransformDirection(CurrentSpeed * 20);
        rb.linearVelocity = newPosition; 
            
    }
    void ProcessShooting()
    {
        if ((playerControls == InputMode.Keyboard && Input.GetKeyDown(KeyCode.Q)) || (playerControls == InputMode.Controller && (Input.GetKeyDown("joystick button 4"))))
        {
            if (rocketAmount > 0)
            {
                var r = Instantiate(RocketRef, ShootPoints[2].position, transform.rotation);
                r.SetDirection(transform.forward, gameObject);
                rocketAmount--;
            }
        }

        if ((playerControls == InputMode.Keyboard && Input.GetKey(KeyCode.E)) 
            || (playerControls == InputMode.Controller && (Input.GetKey("joystick button 5"))) 
            && laserTimer <= 0)
        {
            if (energyAmount > 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    var l = Instantiate(LaserRef, ShootPoints[i].position, transform.rotation);
                    l.SetDirection(transform.forward, gameObject);

                }
                laserTimer = laserDelay;
                energyAmount--;
                if (energyAmount <= 0)
                { HLOn.enabled = false; }
            }
        }

        if (laserTimer > 0)
            laserTimer -= Time.deltaTime;
    }

    private void ProcessHeadLights()
    {
        if ((playerControls == InputMode.Keyboard && Input.GetKeyDown(KeyCode.F))
            || (playerControls == InputMode.Controller && Input.GetKeyDown("joystick button 2")))
        {
            HandleActivation();
        } 

        if (HLOn.enabled)
        {
            if (HLEnergyTimer > 0)
                HLEnergyTimer -= Time.deltaTime;
            else
            {
                energyAmount--;
                HLEnergyTimer = HLEnergyUsage;
                if (energyAmount <= 0)
                    HLOn.enabled = false;
            }
        }
    }

    public void HandleActivation()
    {
        if (HLOn.enabled)
        { 
            HLOn.enabled = false; 
            headlightsOnOff.text = "OFF";
        }
        else if (energyAmount > 0)
        { 
            HLOn.enabled = true;
            headlightsOnOff.text = "ON";
        }
    }

    private void ProcessUI()
    {
        if (slimeScreen != null)
        { slimeScreen.color = Color.Lerp(slimeScreen.color, new Color(slimeScreen.color.r, slimeScreen.color.g, slimeScreen.color.b, 0), Time.deltaTime); }

        missilesLeft.text = rocketAmount.ToString();
        lasersLeft.text = (energyAmount * 2).ToString();

        var numAmount = 5 - GameManager.Instance.totalPoints.ToString().Length;
        var fullNum = "";
        while (numAmount > 0)
        { fullNum += "0"; numAmount--; }
        scoreDisplay.text = fullNum + GameManager.Instance.totalPoints.ToString();

        shipHp.text = health.ToString();
        if (health > 75)
        {
            shipIcon.color = new Color(0, 1, 0, 1);
            shipStatus.text = "GOOD";
        }
        else if (health > 50)
        {
            shipIcon.color = new Color(1, 1, 0, 1);
            shipStatus.text = "OK";
        }
        else if (health > 25)
        {
            shipIcon.color = new Color(1, 0.5f, 0, 1);
            shipStatus.text = "BAD";
        }
        else
        {
            shipIcon.color = new Color(1, 0, 0, 1);
            shipStatus.text = "DANGER";
        }
        
        energyLeft.text = energyAmount.ToString();
        enemiesLeft.text = (GameManager.Instance.totalEnemies - GameManager.Instance.enemiesEscaped - GameManager.Instance.enemiesKilled).ToString();

        timePassed.text = GameManager.Instance.playerTime.ToString("F2");

    }

    private void ProcessTimers()
    {
        if (GameManager.Instance.bombPlanted)
        { bombTimer.text = GameManager.Instance.bombTimer.ToString("F2"); }

        if (enemyVisTimer > 0)
        {
            enemyVisTimer -= Time.deltaTime;
            enemyEscaped.SetActive(true);
        }
        else
        { enemyEscaped.SetActive(false); }

        if (escapeVisTimer > 0)
        {
            escapeVisTimer -= Time.deltaTime;
            cantEscape.SetActive(true);
        }
        else
        { cantEscape.SetActive(false); }
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
        health -= damage;
        if (health <= 0)
        { Death(); }
    }

    public void Death()
    {
        GameManager.Instance.GameOver();
    }

    public void Slimed()
    {
        if (slimeScreen != null)
        { slimeScreen.color = new Color(0, 0, 0, 1f); }
        else 
        { Debug.Log("SLIMED! Slimescreen not set");  }
    }
    #endregion
}