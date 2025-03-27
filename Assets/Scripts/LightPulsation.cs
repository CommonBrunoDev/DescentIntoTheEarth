using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightPulsation : MonoBehaviour
{
    private Light light;
    [SerializeField] private float pulsationSpeed = 1f;
    [SerializeField] private float maxIntensity = 15f;

    private void Awake()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        light.intensity = maxIntensity * 3/5 + Mathf.Sin(Mathf.PI * (Mathf.PI * + Time.time * pulsationSpeed)) * (maxIntensity * 2/5); ;
    }
    //Sin(PI* (u + 0.5f * t));
}
