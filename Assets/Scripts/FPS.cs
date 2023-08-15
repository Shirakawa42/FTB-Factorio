using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private float updateTime = 0.5f;
    private float timer = 0f;
    private int frameCount = 0;
    private float fps = 0f;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        frameCount++;
        if (timer >= updateTime)
        {
            fps = frameCount / timer;
            textMeshPro.text = fps.ToString("F2");
            timer = 0f;
            frameCount = 0;
        }
    }
}
