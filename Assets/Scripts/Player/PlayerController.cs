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

    public float jumpForce      = 30.0f;
    public float lateralSpeed   = 5.0f;

    private LateralDirection lateralDirection = LateralDirection.kNone;
    private bool isJump = false;

    [SerializeField]
    private float kRisingGravity  = 10;
    [SerializeField]
    private float kFallingGravity = 20;

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

        UpdateGravityMultiplier();
    }

    void FixedUpdate()
    {
        //Sets players lateral velocity
        switch (lateralDirection)
        {
            case LateralDirection.kLeft:
                m_rigidbody.velocity = lateralSpeed * Vector2.left;//new Vector2(-1, 0);
                break;
            case LateralDirection.kRight:
                m_rigidbody.velocity = lateralSpeed * Vector2.right;//new Vector2(1, 0);
                break;
            default:
                m_rigidbody.velocity = Vector2.zero;//new Vector2(0, 0);
                break;
        }

        //Jumps if the jump key was pressed
        if (isJump)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.55F);

            if (hit.collider != null)
            {
                m_rigidbody.AddForce(Vector2.up * jumpForce);
            }
            isJump = false;
        }
    }

    void UpdateGravityMultiplier()
    {
        m_rigidbody.gravityScale = m_rigidbody.velocity.y > 0 ? kRisingGravity : kFallingGravity;
    }
}

