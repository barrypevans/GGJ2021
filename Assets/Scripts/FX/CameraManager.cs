using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SystemSingleton<CameraManager>
{
    private Camera m_camera;
    private Vector3 m_cameraPosition;

    // Screen Shake Vars
    private const float kShakeDecay          = .01f;
    private const float kShakeIntensityMax   = 3.0f;
    private Vector2 kShakeTimePhaseShift = new Vector2(0,10);

    private Vector3 m_cameraShakeOffset;
    private float m_cameraShakeIntensity;

    public void DoShake(float intensity)
    {
        m_cameraShakeIntensity += intensity;
        m_cameraShakeIntensity = Mathf.Min(m_cameraShakeIntensity, kShakeIntensityMax);
    }

    protected override void Awake()
    {
        m_camera = Camera.main;
    }

    private void FixedUpdate()
    {

        Vector2 shakeSamplePos = Vector2.one * Time.time + kShakeTimePhaseShift;
        float value = Mathf.PerlinNoise(shakeSamplePos.x, shakeSamplePos.y);

        // decrease shake intensity
        m_cameraShakeIntensity -= kShakeDecay;
        m_cameraShakeIntensity = Mathf.Max(m_cameraShakeIntensity, 0);
    }
}
