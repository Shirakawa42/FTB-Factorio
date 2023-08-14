using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    private float _DayLight = 1.0f;
    private bool _DayLightIncreasing = false;

    void Update()
    {
        UpdateDayLight();
    }

    void UpdateDayLight()
    {
        if (_DayLightIncreasing)
            SetDaylight(_DayLight + Time.deltaTime * 0.3f);
        else
            SetDaylight(_DayLight - Time.deltaTime * 0.3f);
    }

    void SetDaylight(float daylight)
    {
        Globals.ChunkMaterialFloor.SetFloat("_Daylight", daylight);
        Globals.ChunkMaterialSolid.SetFloat("_Daylight", daylight);
        Globals.SpritesMaterial.SetFloat("_Daylight", daylight);
        
        _DayLight = daylight;

        if (_DayLight <= 0.0f)
        {
            _DayLight = 0.0f;
            _DayLightIncreasing = true;
        }
        if (_DayLight >= 1.0f)
        {
            _DayLight = 1.0f;
            _DayLightIncreasing = false;
        }
    }
}
