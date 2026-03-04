using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the data for the water, which is passed into the shader graph for the visuals, and the player ocean for the movement, to enable matching. 
/// </summary>
[CreateAssetMenu(fileName = "WaterSettings", order = 1)]
public class WaterSettings : ScriptableObject
{
    public float amplitude;
    public float frequency;
    public float speed;

}

