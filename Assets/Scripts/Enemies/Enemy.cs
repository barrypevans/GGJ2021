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

    protected virtual void Start()
    {
        _startPosition = transform.position;
        SetSkin(EnemyPower);
    }

    public virtual void SetSkin(Power powerlevel)
    {
        if(powerlevel == Power.Easy)
        {
            m_variantId = "";
        }
        else
        {
            m_variantId = "Var";
        }
    }
}
