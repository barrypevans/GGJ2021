using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerController))]
public class SquashAndStretcher : MonoBehaviour
{
    enum SquashState
    {
        kAir,
        kIdle,
        kRunning
    };
    SquashState state;

    private Rigidbody2D m_rigidbody;
    private PlayerController m_playerController;
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_playerController = GetComponent<PlayerController>();
    }
    void Update()
    {
        float absVelocityX = Mathf.Abs(m_rigidbody.velocity.x);
        float absVelocityY = Mathf.Abs(m_rigidbody.velocity.y);


        state = m_playerController.IsGrounded() ? SquashState.kRunning : SquashState.kAir;
        if (state == SquashState.kRunning && absVelocityX <= .01)
            state = SquashState.kIdle;

        if (state == SquashState.kAir)
        {

            float t = Mathf.Min(absVelocityY, 10.0f) / 10.0f;
            float squashAmount = Mathf.Lerp(1, .9f, t);
            squashAmount = m_rigidbody.velocity.y > 0 ? squashAmount : 1 / squashAmount;
            transform.localScale = new Vector3(squashAmount, 1 / squashAmount, 1);
        }
        else if (state == SquashState.kRunning)
        {

            float squashAmountX = 1.0f + Mathf.Cos(Time.time * 20.0f) * .1f;
            float squashAmountY = 1.0f - Mathf.Cos(Time.time * 20.0f) * .1f;
            transform.localScale = new Vector3(squashAmountX, squashAmountY, 1);
        }
        else if (state == SquashState.kIdle)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
