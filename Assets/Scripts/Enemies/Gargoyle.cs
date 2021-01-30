using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : Enemy
{
    public enum EnemyState
    {
        MovingToPosition,
        InPosition,
        Attacking,
        Dying
    }

    public EnemyState State = EnemyState.MovingToPosition;
    private Vector2 _targetPosition;
    private float _speed = 10;
    private float _time = 0;
    private Vector2 _hoverPosition;
    public int Direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Attack()
    {
        State = EnemyState.Attacking;
    }

    public void SetTarget(Transform targetPosition)
    {
        _targetPosition = targetPosition.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _time += Time.deltaTime * Direction;
        _hoverPosition = new Vector2(
            _targetPosition.x + Mathf.Cos(_time * _speed / 10) * 3,
            _targetPosition.y + Mathf.Sin(_time * _speed / 10) * 3);
        switch (State)
        {
            case EnemyState.MovingToPosition:
                MoveToPosition();
                break;
            case EnemyState.InPosition:
                Hover();
                break;
            case EnemyState.Attacking:
                MoveToAttack();
                break;
        }
    }

    void MoveToPosition()
    {
        if(Vector2.Distance(_targetPosition, transform.position) < 3)
        {
            State = EnemyState.InPosition;
        }
        transform.position = Vector2.MoveTowards(transform.position, _hoverPosition ,_speed * Time.deltaTime);
    }
    void Hover()
    {
        transform.position = Vector2.MoveTowards(transform.position, _hoverPosition, _speed * Time.deltaTime);
    }

    void MoveToAttack()
    {
        if (Vector2.Distance(GameManager.Get().GetPlayer().transform.position, transform.position) < 2)
        {
            State = EnemyState.MovingToPosition;
        }
        transform.position = Vector2.MoveTowards(transform.position, 
            GameManager.Get().GetPlayer().transform.position, _speed * Time.deltaTime);
    }
}
