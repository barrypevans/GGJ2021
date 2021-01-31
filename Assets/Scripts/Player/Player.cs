using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    private SpriteRenderer m_renderer;
    public GameObject m_gun;
    private int health = 3;

    public void ResetGun()
    {
        m_gun.GetComponent<Gun>().ResetAmmo();
    }

    private void Awake()
    {
        m_renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        m_gun = Instantiate(Resources.Load<GameObject>("Gun"), transform.position, Quaternion.identity);   //Spawn the gun
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
            //Respawn player?
            Debug.Log("Health" + health);
            GameManager.Get().ResetPlayer();
            //Destroy(m_gun);
            //Destroy(gameObject);
            //Remove heart
        }
        UiManager.Get().UpdatePlayerHealth(health);
    }
}
