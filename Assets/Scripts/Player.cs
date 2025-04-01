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
    [SerializeField] float cameraSens = 5f;
    [SerializeField] float cameraSnap = 20f;
    [SerializeField] Camera cameraObj;
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
    [SerializeField] GameObject UI;
    [SerializeField] GameObject pauseUI;
    [SerializeField] Image slimeScreen;
    [SerializeField] TextMeshProUGUI missilesLeft;
    [SerializeField] TextMeshProUGUI lasersLeft;
    [SerializeField] TextMeshProUGUI scoreDisplay;
    [SerializeField] RawImage shipIcon;
    [SerializeField] Sprite[] iconImages;
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
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }
    void Start()
    {
        pauseUI.SetActive(false);
        cantEscape.SetActive(false);
        enemyEscaped.SetActive(false);
        bombPopup.gameObject.SetActive(false);
        bombManager.SetActive(false);

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
        if (GameManager.Instance.gameState == GameStates.Paused && (Input.GetKeyDown("joystick button 6") || Input.GetKeyDown("joystick button 7")))
        { PauseGame(false); }
        else if (GameManager.Instance.gameState == GameStates.Playing)
        {
            ProcessLook();
            ProcessMovement();
            ProcessShooting();
            ProcessHeadLights();
            ProcessUI();
            ProcessTimers();
        }
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

        accMouseX = Mathf.Lerp(accMouseX, inputLook.x, cameraSnap * Time.deltaTime);
        accMouseY = Mathf.Lerp(accMouseY, inputLook.y, cameraSnap * Time.deltaTime);

        float rotationX = accMouseX * cameraSens * 100f * Time.deltaTime;
        float rotationY = accMouseY * cameraSens * 100f * Time.deltaTime;

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
        if ((playerControls == InputMode.Keyboard && Input.GetKeyDown(KeyCode.P))
            || (playerControls == InputMode.Controller && (Input.GetKeyDown("joystick button 6") || Input.GetKeyDown("joystick button 7"))))
        { PauseGame(true); return; }

        if ((playerControls == InputMode.Keyboard && Input.GetKey(KeyCode.R))
            || (playerControls == InputMode.Controller && Input.GetKey("joystick button 3")))
        { UI.SetActive(false); cameraObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0)); }
        else
        { UI.SetActive(true); cameraObj.transform.localRotation = Quaternion.Euler(Vector3.zero); }

        if (slimeScreen != null)
        { slimeScreen.color = Color.Lerp(slimeScreen.color, new Color(slimeScreen.color.r, slimeScreen.color.g, slimeScreen.color.b, 0), Time.deltaTime); }

        missilesLeft.text = rocketAmount.ToString();
        lasersLeft.text = (energyAmount * 2).ToString();

        var num = GameManager.Instance.totalPoints;
        var stringNum = "";

        if (num < 0) stringNum += "-";
        num = Mathf.Abs(num);
        var numAmount = 5 - num.ToString().Length;
        while (numAmount > 0)
        { stringNum += "0"; numAmount--; }
        scoreDisplay.text = stringNum + num.ToString();

        shipHp.text = health.ToString();
        if (health > 75)
        {
            shipIcon.texture = iconImages[0].texture;
            shipStatus.text = "GOOD";
        }
        else if (health > 50)
        {
            shipIcon.texture = iconImages[1].texture;
            shipStatus.text = "OK";
        }
        else if (health > 25)
        {
            shipIcon.texture = iconImages[2].texture;
            shipStatus.text = "BAD";
        }
        else
        {
            shipIcon.texture = iconImages[3].texture;
            shipStatus.text = "DANGER";
        }
        
        energyLeft.text = energyAmount.ToString();
        enemiesLeft.text = (GameManager.Instance.totalEnemies - GameManager.Instance.enemiesEscaped - GameManager.Instance.enemiesKilled).ToString();

        timePassed.text = GetTimeString((int)GameManager.Instance.playerTime);

    }

    private void ProcessTimers()
    {
        if (GameManager.Instance.bombPlanted)
        {
            bombTimer.text = GetTimeString((int)GameManager.Instance.bombTimer);
            bombManager.gameObject.SetActive(true);
        }

        if (enemyVisTimer > 0)
        {
            Debug.Log(enemyVisTimer);
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
    public void PauseGame(bool pause)
    {
        GameManager.Instance.PauseGame(pause);
        UI.SetActive(!pause);
        pauseUI.SetActive(pause);
    }
    string GetTimeString(int time)
    {
        int seconds = time;

        int minutes = (int)seconds / 60;
        seconds = seconds - minutes * 60;

        int hours = (int)minutes / 60;
        minutes = minutes - hours * 60;

        string timeString = "";

        if (hours > 0)
        {
            if (hours < 10) timeString += "0";
            timeString += hours.ToString() + ":";
        }

        if (minutes < 10) timeString += "0";
        timeString += minutes.ToString() + ":";

        if (seconds < 10) timeString += "0";
        timeString += seconds.ToString();

        return timeString;
    }
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
        GameManager.Instance.hitless = false;
    }

    public void Death()
    {
        GameManager.Instance.GameOver();
    }

    public void Slimed()
    {
        if (slimeScreen != null)
        { slimeScreen.color = new Color(slimeScreen.color.r, slimeScreen.color.g, slimeScreen.color.b, 1f); }
        else 
        { Debug.Log("SLIMED! Slimescreen not set");  }
    }
    #endregion
}