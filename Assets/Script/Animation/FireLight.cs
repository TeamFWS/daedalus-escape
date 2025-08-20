using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireLight : MonoBehaviour
{
    private const float ColorChangeInterval = 0.1f;
    private const float MinColorG = 0.42f;
    private const float MaxColorG = 0.52f;
    private const float MinIntensity = 1.2f;
    private const float MaxIntensity = 1.4f;
    private Light2D _fireLight;
    private float _timer;

    private void Start()
    {
        _fireLight = GetComponent<Light2D>();
        _timer = ColorChangeInterval;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer < 0f)
        {
            _timer = ColorChangeInterval;
            SetRandomFireLight();
        }
    }

    private void SetRandomFireLight()
    {
        _fireLight.color = new Color(1f, Random.Range(MinColorG, MaxColorG), 0.4f);
        _fireLight.intensity = Random.Range(MinIntensity, MaxIntensity);
    }
}