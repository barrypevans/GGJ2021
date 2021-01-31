using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Werewolf : Enemy
{
    public EnemyState State = EnemyState.Leaping;
    private Rigidbody2D m_rigidbody;
    private float _kAccel = 3f;
    private float _kMaxVelocity = 10f;
    private float _kLeapVelocity = 20f;
    
    private Transform m_player;
    private Transform m_currentTarget;

    private SpriteRenderer m_renderer;


    IEnumerator _stateCycler;

    // Start is called before the first frame update
    void Start()
    {
        m_renderer = GetComponent<SpriteRenderer>();
        m_player = GameManager.Get().GetPlayer().transform;
        m_rigidbody = GetComponent<Rigidbody2D>();
        _kAccel += Random.Range(0, 4.5f);

        m_currentTarget = m_player;
        _stateCycler = CycleState();
        StartCoroutine(_stateCycler);
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
                    yield return new WaitForSeconds(2.5f);
                    State = EnemyState.InPosition;
                } else
                {
                    yield return new WaitForSeconds(5f);
                }
            }
        }
    }

    public void Attack()
    {
        StopAllCoroutines();
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
        m_renderer.flipX = m_rigidbody.velocity.x < 0;
        //DampenAccel(toTarget);
    }

private void GetOutOfPosition(Vector3 toTarget)
    {
        m_rigidbody.velocity += new Vector2(-toTarget.normalized.x * _kMaxVelocity, transform.position.y) * Time.deltaTime * _kMaxVelocity / 4;
    }

    private void MoveToPosition(Vector3 toTarget)
    {
        m_rigidbody.velocity += new Vector2(toTarget.normalized.x * _kMaxVelocity, transform.position.y) * Time.deltaTime * _kMaxVelocity /4 ;
    }

    private void MoveToAttack(Vector3 toTarget)
    {
        m_rigidbody.velocity = Vector2.zero;
        m_rigidbody.AddForce(new Vector2(toTarget.normalized.x, Mathf.Max(Mathf.Sqrt(toTarget.y), 1f)) 
            * _kLeapVelocity * (toTarget.magnitude + 5));
        State = EnemyState.Leaping;
        _stateCycler = CycleState();
        StartCoroutine(_stateCycler);
    }
    public void Kill()
    {
        EnemyManager.Get().GetWerewolves().Remove(gameObject);
        FXManager.Get().PlaySFX("sfx/wolf howl", Random.Range(0, 3), 0.1F);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == GameManager.Get().GetPlayer())
        {
            GameManager.Get().GetPlayer().GetComponent<Player>().HitPlayer();
            Kill();
        }
    }
}
