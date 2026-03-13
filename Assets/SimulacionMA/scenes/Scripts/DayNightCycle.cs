using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] public Light sun;

    public float dayDuration = 120f;

    float time;

    void Update()
    {
        if (sun == null) return;

// avanza el tiempo
        time += Time.deltaTime / dayDuration;

        if (time > 1f)
            time = 0f;

// rotación del sol
        float angle = time * 360f;
        sun.transform.rotation = Quaternion.Euler(angle - 90f, 170f, 0);

// intensidad de luz
        float dot = Vector3.Dot(sun.transform.forward, Vector3.down);

        sun.intensity = Mathf.Clamp01(dot) * 1.2f;

// luz ambiental
        RenderSettings.ambientIntensity = Mathf.Clamp01(dot);
    }
}