using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SwarmProto : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;
    private Transform m_target;
    private float kAcceleration = 30.0f;
    private const float kMaxVelcity = 30.0f;
    private Vector3 m_targetOffset;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        //Never use GameObject.Find lol this is just a proto
        m_target = GameObject.Find("target").transform;
        kAcceleration += Random.Range(0,4.5f);
        m_targetOffset = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);
    }
    private void FixedUpdate()
    {
        Vector3 toTarget = m_target.position+ m_targetOffset - transform.position;
        float accScale = Mathf.Clamp01(toTarget.magnitude / 20.0f);
        accScale = accScale * accScale * accScale;
        float acc = kAcceleration * accScale;
        m_rigidbody.AddForce(toTarget.normalized * kAcceleration);

        if(toTarget.magnitude < 5)
        { 
            m_rigidbody.velocity *= Mathf.Lerp(.99f, 1.0f, Mathf.Clamp01(toTarget.magnitude / 5.0f));
        }

        if (m_rigidbody.velocity.magnitude > kMaxVelcity)
            m_rigidbody.velocity = m_rigidbody.velocity.normalized * kMaxVelcity;
    }
}
