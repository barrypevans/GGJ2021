using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Werewolf : Enemy
{
    public EnemyState State = EnemyState.MovingToPosition;
    private Rigidbody2D m_rigidbody;
    private float _kAccel = 40f;
    private float _kMaxVelocity = 10f;
    
    private Transform m_player;
    private Transform m_currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        m_player = GameManager.Get().GetPlayer().transform;
        m_rigidbody = GetComponent<Rigidbody2D>();
        _kAccel += Random.Range(0, 4.5f);

        m_currentTarget = m_player;
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
            case EnemyState.Attacking:
                MoveToAttack(toTarget);
                break;
        }
    }
    private void MoveToAttack(Vector3 toTarget)
    {
        if (toTarget.magnitude < 2)
        {
            // Face player
            // Play attack animation
        }
        m_rigidbody.AddForce(toTarget.normalized * _kAccel);
    }
    public void Kill()
    {
        EnemyManager.Get().GetWerewolves().Remove(gameObject);
        Destroy(gameObject);
    }
}
