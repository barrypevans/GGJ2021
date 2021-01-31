using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Werewolf : Enemy
{
    public EnemyState State = EnemyState.Leaping;
    private Rigidbody2D m_rigidbody;
    private float _kAccel = 3f;
    private float _kMaxVelocity = 3f;
    private float _kLeapVelocity = 150f;
    
    private Transform m_player;
    private Transform m_currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        m_player = GameManager.Get().GetPlayer().transform;
        m_rigidbody = GetComponent<Rigidbody2D>();
        _kAccel += Random.Range(0, 4.5f);

        m_currentTarget = m_player;
        StartCoroutine(CycleState());
    }

    IEnumerator CycleState()
    {
        while(true)
        {
            {
                if (State == EnemyState.MovingToPosition)
                {
                    yield return new WaitForSeconds(.5f);
                    State = EnemyState.InPosition;
                }
                else if (State == EnemyState.InPosition) 
                {
                    yield return new WaitForSeconds(.2f);
                    State = EnemyState.MovingToPosition;
                } else if (State == EnemyState.Leaping)
                {
                    yield return new WaitForSeconds(20f);
                    State = EnemyState.InPosition;
                }
            }
        }
    }

    public void Attack()
    {
        State = EnemyState.Attacking;
        SetCurrentTarget(m_player);
    }
    private void SetCurrentTarget(Transform currentTarget)
    {
        m_currentTarget = currentTarget;
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        var toTarget = m_currentTarget.position - transform.position;

        switch (State)
        {
            case EnemyState.MovingToPosition:
                MoveToPosition(toTarget);
                break;
            case EnemyState.InPosition:
                GetOutOfPosition(toTarget);
                break;
            case EnemyState.Attacking:
                MoveToAttack(toTarget);
                break;
            default:
                break;
        }
        //DampenAccel(toTarget);
    }

    private void GetOutOfPosition(Vector3 toTarget)
    {
        m_rigidbody.velocity = new Vector2(-toTarget.normalized.x * _kMaxVelocity, transform.position.y);
    }

    private void MoveToPosition(Vector3 toTarget)
    {
        m_rigidbody.velocity = new Vector2(toTarget.normalized.x * _kMaxVelocity, transform.position.y);
    }

    private void MoveToAttack(Vector3 toTarget)
    {
        m_rigidbody.AddForce(new Vector2(0, 500f));
        State = EnemyState.Leaping;
    }
    public void Kill()
    {
        EnemyManager.Get().GetWerewolves().Remove(gameObject);
        Destroy(gameObject);
    }

    private void DampenAccel(Vector3 toTarget)
    {
        if (toTarget.magnitude < 5)
        {
            m_rigidbody.velocity *= Mathf.Lerp(.99f, 1f, Mathf.Clamp01(toTarget.magnitude / 5.0f));
        }
        if (m_rigidbody.velocity.magnitude > _kMaxVelocity)
        {
            m_rigidbody.velocity = m_rigidbody.velocity.normalized * _kMaxVelocity;
        }
    }
}
