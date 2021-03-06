using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Animateable
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
    private bool m_isGrounded = true;

    [SerializeField]
    private float kRisingGravity = .1f;
    [SerializeField]
    private float kFallingGravity = .1f;

    private bool m_isTeleporting;
    private Vector3 m_teleportTarget;
    private const float kTeleportDist = 5.0f;

    private bool isPaused = false;


    AnimData m_walkAnimData;
    AnimData m_reverseWalkAnimData;
    AnimData m_idleAnimData;
    AnimData m_riseAnimData;
    AnimData m_fallAnimData;

    protected override void Awake()
    {
        base.Awake();
        m_isTeleporting = false;
        m_rigidbody = GetComponent<Rigidbody2D>();

        m_walkAnimData = InitAnimData("sprites/player/playerWalk", 4);
        m_reverseWalkAnimData = InitAnimData("sprites/player/playerWalk", 4, 1, true);
        m_idleAnimData = InitAnimData("sprites/player/playerIdle", 4, 0.8f);
        m_riseAnimData = InitAnimData("sprites/player/playerRise", 1);
        m_fallAnimData = InitAnimData("sprites/player/playerFall", 1);
        m_activeAnimData = m_idleAnimData;
    }

    private void DoTeleport()
    {
        if (m_isTeleporting) return;

        m_isTeleporting = true;
        FXManager.Get().PlaySFX("sfx/Dash", Random.Range(1, 5), 0.4F);

        Vector3 mousePosWs = GameManager.Get().GetMouseWorldPos();
        Vector3 teleportDir = (mousePosWs - transform.position).normalized;
        Vector3 teleportPos = teleportDir * kTeleportDist + transform.position;

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, teleportDir, kTeleportDist);
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject != gameObject &&
                hit.collider.gameObject.tag != "Werewolf" &&
                hit.collider.gameObject.tag != "Bat")
            {
                teleportPos = (Vector3)hit.point - teleportDir * 1.0f;
            }
        }

        m_teleportTarget = teleportPos;
        m_teleportSnapAnim = 0;
        m_teleportMaterializeAnim = 0;
        m_teleportPause = 0;
        GetComponent<SquashAndStretcher>().enabled = false;
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        m_rigidbody.simulated = false;
        m_rigidbody.velocity = Vector3.zero;
    }

    private float m_teleportSnapAnim = 0;
    private float m_teleportMaterializeAnim = 0;
    private float m_teleportPause = 0;
    private void UpdateTeleport()
    {
        if (!m_isTeleporting) return;

        if (m_teleportSnapAnim < 1.0f)
        {
            float cosTheta = Mathf.Cos(m_teleportSnapAnim * 2 * 3.14159f);
            float squashAmountX = 1.0f + cosTheta * .5f;
            float squashAmountY = 1.0f - cosTheta * .5f;
            transform.localScale = new Vector3(squashAmountX, squashAmountY, 1);
            m_teleportSnapAnim += Time.deltaTime * 10;
        }
        else if (m_teleportPause < 1.0f)
        {
            //GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
            m_teleportPause += Time.deltaTime * 30;
        }
        else if (m_teleportMaterializeAnim < 1.0f)
        {
            transform.position = m_teleportTarget;
            float cosTheta = Mathf.Cos(m_teleportSnapAnim * 2 * 3.14159f);
            float invNormTime = 1.0f - m_teleportMaterializeAnim;
            float squashAmountX = 1.0f + cosTheta * invNormTime;
            float squashAmountY = 1.0f - cosTheta * invNormTime;
            transform.localScale = new Vector3(squashAmountX, squashAmountY, 1);
            m_teleportMaterializeAnim += Time.deltaTime * 10;
            CameraManager.Get().DoShake();
        }
        else
        {
            m_teleportSnapAnim = 0;
            m_teleportMaterializeAnim = 0;
            GetComponent<SquashAndStretcher>().enabled = true;
            GetComponent<SpriteRenderer>().color = Color.white;
            m_rigidbody.simulated = true;
            m_isTeleporting = false;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.P))
            PauseResumeGame();

        if (!isPaused)
        {
            UpdateTeleport();
            GroundCheck();
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJump = true;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                DoTeleport();
            }

            m_rigidbody.gravityScale = m_rigidbody.velocity.y > 0 ? kRisingGravity : kFallingGravity;
            Debug.DrawRay(transform.position, Vector3.down * .55f, Color.red);
        }
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
        if (isJump && m_isGrounded)
        {
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, jumpForce);
            isJump = false;
        }
    }

    protected override void UpdateAnims()
    {
        if (IsGrounded())
        {
            if (lateralDirection != LateralDirection.kNone)
            {
                float MouseXWs = GameManager.Get().GetMouseWorldPos().x;
                bool invert = MouseXWs < transform.position.x && lateralDirection == LateralDirection.kRight ||
                    MouseXWs >= transform.position.x && lateralDirection == LateralDirection.kLeft;
                if (invert)
                    m_activeAnimData = m_reverseWalkAnimData;
                else
                    m_activeAnimData = m_walkAnimData;
            }
            else if (lateralDirection == LateralDirection.kNone)
            {
                m_activeAnimData = m_idleAnimData;
            }
        }
        else
        {
            if (m_rigidbody.velocity.y > 0)
            {
                m_activeAnimData = m_riseAnimData;
            }
            else
            {
                m_activeAnimData = m_fallAnimData;
            }
        }
        base.UpdateAnims();
    }

    public bool IsGrounded()
    {
        return m_isGrounded;
    }

    private void GroundCheck()
    {
        m_isGrounded = false;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, Vector2.one, 0, Vector2.down, 0.55F);

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject != gameObject)
            {
                m_isGrounded = true;
                return;
            }
        }
    }

    public void PauseResumeGame()
    {
        //Maybe put up panel to show game is paused?
        //Debug.Log(isPaused);
        //Debug.Log(Time.timeScale);

        if (isPaused)
        {
            isPaused = false;
            FXManager.Get().PlaySFX("sfx/Unpause", 0F, 0.5F);
            UiManager.Get().ShowPausePanel(false);
            UiManager.Get().PauseResumeDialog(true);
            Time.timeScale = 1;
        }
        else
        {
            isPaused = true;
            FXManager.Get().PlaySFX("sfx/Pause", 0F, 0.5F);
            Time.timeScale = 0;
            UiManager.Get().PauseResumeDialog(false);
            UiManager.Get().ShowPausePanel(true);
        }


    }
}

