using TMPro;
using UnityEngine;

public class TransparencyPulsation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float transparencySpeed = 1f;
    [SerializeField] private float maxTransparency = 15f;

    // Update is called once per frame
    void Update()
    {
        Color c = text.color;
        c.a = maxTransparency * 3 / 5 + Mathf.Sin(Mathf.PI * (Mathf.PI * + Time.time * transparencySpeed)) * (maxTransparency * 2 / 5);
        text.color = c;
    }
}
