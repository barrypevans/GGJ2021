using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    public EnemyState State = EnemyState.MovingToPosition;
    private Transform m_currentTarget;
    private Transform m_hoverTarget;
    private Rigidbody2D m_rigidbody;
    private Vector3 m_targetOffset;
    private float _kAccel = 40f;
    private float _kMaxVelocity = 10f;

    private Transform m_player;

    private const float MaxTargetOffset = 1.5f;

    private void Awake()
    {
        m_player = GameManager.Get().GetPlayer().transform;
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_targetOffset = new Vector2(
            Random.Range(-MaxTargetOffset, MaxTargetOffset), 
            Random.Range(-MaxTargetOffset, MaxTargetOffset));
        _kAccel += Random.Range(0, 4.5f);
    }

    public void Attack()
    {
        State = EnemyState.Attacking;
        SetCurrentTarget(m_player);
    }

    public void SetTarget(Transform hoverTarget)
    {
        m_hoverTarget = hoverTarget;
        SetCurrentTarget(hoverTarget);
    }

    private void SetCurrentTarget(Transform currentTarget)
    {
        m_currentTarget = currentTarget;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        var toTarget = m_currentTarget.position - transform.position +
            (m_currentTarget == m_hoverTarget ? m_targetOffset : Vector3.zero);
        float accScale = Mathf.Clamp01(toTarget.magnitude / 20.0f);
        accScale = accScale * accScale * accScale;
        float acc = _kAccel * accScale;

        switch (State)
        {
            case EnemyState.MovingToPosition:
                MoveToPosition(toTarget);
                break;
            case EnemyState.InPosition:
                Hover(toTarget);
                break;
            case EnemyState.Attacking:
                MoveToAttack(toTarget);
                break;
        }
        DampenAccel(toTarget);
    }

    private void MoveToPosition(Vector3 toTarget)
    {
        if(Vector2.Distance(m_currentTarget.position, transform.position) < 3)
        {
            State = EnemyState.InPosition;
        }
        m_rigidbody.AddForce(toTarget.normalized * _kAccel);
    }

    private void DampenAccel(Vector3 toTarget)
    {
        if(toTarget.magnitude < 5)
        {
            m_rigidbody.velocity *= Mathf.Lerp(.99f, 1f, Mathf.Clamp01(toTarget.magnitude / 5.0f));
        }
        if(m_rigidbody.velocity.magnitude > _kMaxVelocity)
        {
            m_rigidbody.velocity = m_rigidbody.velocity.normalized * _kMaxVelocity;
        }
    }

    private void Hover(Vector3 toTarget)
    {
        m_rigidbody.AddForce(toTarget.normalized * _kAccel);
    }

    private void MoveToAttack(Vector3 toTarget)
    {
        if (toTarget.magnitude < 2)
        {
            State = EnemyState.MovingToPosition;
            SetCurrentTarget(m_hoverTarget);
        }
        m_rigidbody.AddForce(toTarget.normalized * _kAccel);
    }

    // Create Bat Destroyed method
    public void Kill()
    {
        EnemyManager.Get().GetBats().Remove(gameObject);
        FXManager.Get().PlaySFX("sfx/Splat 2", Random.Range(0, 5));
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name.Contains("player"))
        {
            GameManager.Get().GetPlayer().GetComponent<Player>().HitPlayer();
            FXManager.Get().PlaySFX("sfx/Splat 1", Random.Range(0, 3));
            Kill();
        }
    }
}
