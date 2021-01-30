using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SystemSingleton<CameraManager>
{
    private Camera m_camera;
    private Vector3 m_cameraPosition;

    // Screen Shake Vars
    private const float kShakeDecay          = .9f;
    private const float kShakeIntensityMax   = 1.0f;
    private const float kShakeSpeed          = 15.0f;
    private Vector2 kShakeTimePhaseShift = new Vector2(0,10);

    private Vector3 m_cameraShakeOffset;
    private float m_cameraShakeIntensity;

    public void DoShake(float intensity = .01f)
    {
        m_cameraShakeIntensity += intensity;
        m_cameraShakeIntensity = Mathf.Min(m_cameraShakeIntensity, kShakeIntensityMax);
    }

    protected override void Awake()
    {
        base.Awake();
        m_camera = Camera.main;
    }

    private void FixedUpdate()
    {

        Vector2 shakeSamplePos = Vector2.one * Time.time * kShakeSpeed + kShakeTimePhaseShift;
        float value = Mathf.PerlinNoise(shakeSamplePos.x, shakeSamplePos.y);
        float value2 = Mathf.PerlinNoise(shakeSamplePos.y, shakeSamplePos.x);

        // decrease shake intensity
        m_cameraShakeIntensity *= kShakeDecay;
        m_cameraShakeIntensity = Mathf.Max(m_cameraShakeIntensity, 0);
        m_cameraShakeIntensity = Mathf.Min(m_cameraShakeIntensity, kShakeIntensityMax);

        m_camera.transform.position = new Vector3(value, value2, 0) * m_cameraShakeIntensity;
        m_camera.transform.position = new Vector3(m_camera.transform.position.x, m_camera.transform.position.y, -10);
    }
}
