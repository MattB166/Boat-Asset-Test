using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterShaderSync : MonoBehaviour
{
    public WaterSettings waterSettings;
    public Material waterMaterial;

    private void Start()
    {
        ApplySettings();
    }

    private void OnValidate()
    {
        ApplySettings();
    }

    private void ApplySettings()
    {
        if (waterMaterial != null && waterSettings != null)
        {
            waterMaterial.SetFloat("_Amplitude", waterSettings.amplitude);
            waterMaterial.SetFloat("_Frequency", waterSettings.frequency);
            waterMaterial.SetFloat("_Speed", waterSettings.speed);
        }
    }


}
