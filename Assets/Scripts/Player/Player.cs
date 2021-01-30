using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    private SpriteRenderer m_renderer;

    private void Awake()
    {
        m_renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 mousePositionWs = GameManager.Get().GetMouseWorldPos();
        m_renderer.flipX = mousePositionWs.x - transform.position.x > 0;
    }
}
