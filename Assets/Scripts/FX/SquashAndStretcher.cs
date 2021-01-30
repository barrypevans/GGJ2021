using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SquashAndStretcher : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        float absVelocity = Mathf.Abs(m_rigidbody.velocity.y);

        float t = Mathf.Min(absVelocity, 10.0f) / 10.0f;
        float squashAmount = Mathf.Lerp(1, .9f, t);
        squashAmount = m_rigidbody.velocity.y > 0 ? squashAmount : 1 / squashAmount;
        transform.localScale = new Vector3(squashAmount, 1 / squashAmount, 1);

    }
}
