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
    private int flashCooldown = 0;
    private float flashPause = 0;
    private bool flashing = false;

    private Vector2 _startPosition;

    protected virtual void Start()
    {
        _startPosition = transform.position;
        SetSkin(EnemyPower);
    }

    public void DoFlash()
    {
        flashing = true;
    }
        
    protected override void Update()
    {
        base.Update();
        if (flashPause > .05f)
        {
            if (flashing)
            {
                flashCooldown++;
            }
            if (flashCooldown >= 10)
            {
                flashing = false;
                flashCooldown = 0;
            }
            m_spriteRenderer.color = flashCooldown % 2 == 0 ? Color.white : Color.red;
            flashPause = 0;
        }
        flashPause += Time.deltaTime;
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
