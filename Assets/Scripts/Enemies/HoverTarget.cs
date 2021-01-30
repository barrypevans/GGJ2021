using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverTarget : MonoBehaviour
{
    public float Amplitude = 2;
    public float Period = 5;
    public float Offset = Mathf.PI;

    private float m_speed = 0.5f;

    private Vector2 m_origin;
    // Start is called before the first frame update
    private void Awake()
    {
        m_origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(
            transform.position.x,
            m_origin.y + Mathf.Sin(Time.time * Period + Offset) * Amplitude);
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(
            GameManager.Get().GetPlayer().transform.position.x, transform.position.y),
            Time.deltaTime * m_speed);
    }
}
