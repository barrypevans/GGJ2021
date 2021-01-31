using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    private SpriteRenderer m_renderer;

    private int health = 3;

    private void Awake()
    {
        m_renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 mousePositionWs = GameManager.Get().GetMouseWorldPos();
        m_renderer.flipX = mousePositionWs.x - transform.position.x > 0;
    }

    public void HitPlayer()
    {
        health -= 1;
        if (health == 0)
        {
            GameManager.Get().GameOver();
            Debug.Log("Game Over");
            //Game over
        }
        else
        {
            Debug.Log("Health" + health);
            //Remove heart
        }
    }
}
