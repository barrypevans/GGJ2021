using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Animateable
{
    public enum EnemyState
    {
        MovingToPosition,
        InPosition,
        Attacking,
        Dying,
        Leaping
    }

    public enum Power { 
        Easy,
        Hard
    }

    public int HitPoints = 1;
    public Power EnemyPower = Power.Easy;

    private Vector2 _startPosition;
    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
    }
}
