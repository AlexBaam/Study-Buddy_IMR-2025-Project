using UnityEngine;
using System;

public class AdaptiveDirectionalLight : MonoBehaviour
{
    public Light directionalLight;

    void Start()
    {
        if (directionalLight == null)
            directionalLight = GetComponent<Light>();

        UpdateLight();
        InvokeRepeating(nameof(UpdateLight), 0f, 60f);
    }

    void UpdateLight()
    {
        int hour = DateTime.Now.Hour;

        if (hour >= 8 && hour < 12)
            directionalLight.color = new Color(0.9f, 0.95f, 1f);
        else if (hour >= 12 && hour < 18)
            directionalLight.color = new Color(1f, 0.96f, 0.88f);
        else if (hour >= 18 && hour < 22)
            directionalLight.color = new Color(1f, 0.82f, 0.64f);
        else
            directionalLight.color = new Color(1f, 0.7f, 0.5f);
    }
}
