using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    enum LateralDirection
    {
        kLeft,
        kRight,
        kNone
    }

    private Rigidbody2D m_rigidbody;

    public float jumpForce;
    public float lateralSpeed = 10;

    private LateralDirection lateralDirection = LateralDirection.kNone;
    private bool isJump = false;


    [SerializeField]
    private float kRisingGravity  = .1f;
    [SerializeField]
    private float kFallingGravity = .1f;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Keyboard input for left/right movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            lateralDirection = LateralDirection.kLeft;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            lateralDirection = LateralDirection.kRight;
        }
        else
        {
            lateralDirection = LateralDirection.kNone;
        }

        //Keyboard input for jumping
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            isJump = true;
        }

        m_rigidbody.gravityScale = m_rigidbody.velocity.y > 0 ? kRisingGravity : kFallingGravity;
    }

    void FixedUpdate()
    {
        //Sets players lateral velocity
        switch (lateralDirection)
        {
            case LateralDirection.kLeft:
                m_rigidbody.velocity = new Vector2(-lateralSpeed, m_rigidbody.velocity.y);
                break;
            case LateralDirection.kRight:
                m_rigidbody.velocity = new Vector2(lateralSpeed, m_rigidbody.velocity.y);
                break;
            default:
                m_rigidbody.velocity = new Vector2(0, m_rigidbody.velocity.y);
                break;
        }

        //Jumps if the jump key was pressed
        if (isJump)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.55F);
                
            if (hit.collider != null)
            {
                m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, jumpForce);
            }
            isJump = false;
        }
    }
}

