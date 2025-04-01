using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;

    private void Start()
    {

        Vector3 newRot = transform.eulerAngles;
        newRot.y = Random.Range(0, 360);
        transform.rotation = Quaternion.Euler(newRot);
    }
    void Update()
    {
        Vector3 newRot = transform.eulerAngles;
        newRot.y += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(newRot);
    }
}
