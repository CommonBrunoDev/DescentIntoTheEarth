using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform xRotation;
    [SerializeField] Transform yRotation;
    [SerializeField] Transform zRotation;
    [SerializeField] GameObject PlayerBody;

    [SerializeField] float MovementSpeed;
    [SerializeField] float MaxSpeed;

    [SerializeField] float MouseSensitivityX;
    [SerializeField] float MouseSensitivityY;
    private float rotX;
    private float rotY;
    private float rotZ;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
        HandleMovement();
    }

    void HandleRotation()
    {

        if (Input.GetKey(KeyCode.Mouse1))
        {
            rotZ += Input.GetAxis("Mouse X") * MouseSensitivityX;
            zRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, zRotation.localRotation.z + rotZ));
        }
        else
        {
            rotX += Input.GetAxis("Mouse X") * MouseSensitivityX;
            rotY += Input.GetAxis("Mouse Y") * MouseSensitivityY * -1;
            xRotation.localRotation = Quaternion.Euler(new Vector3(0, xRotation.localRotation.x + rotX, 0));
            yRotation.localRotation = Quaternion.Euler(new Vector3(yRotation.localRotation.y + rotY, 0, 0));
        }

        Debug.Log(xRotation.localRotation.y + " / " + yRotation.localRotation.x + " / " + zRotation.localRotation.z);
    }

    void HandleMovement()
    {
        var hInput = Input.GetAxis("Horizontal");
        var vInput = Input.GetAxis("Vertical");
        var dInput = Input.GetAxis("Distal");
        PlayerBody.transform.Translate(new Vector3(hInput, vInput, dInput).normalized * MovementSpeed * Time.deltaTime,Space.Self);
    }
}
