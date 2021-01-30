using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;

    public float jumpForce;
    public float lateralSpeed;

    private string lateralDirection = "none";
    private bool isJump = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Keyboard input for left/right movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            lateralDirection = "left";
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            lateralDirection = "right";
        }
        else
        {
            lateralDirection = "none";
        }

        //Keyboard input for jumping
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            isJump = true;
        }
    }

    void FixedUpdate()
    {
        //Sets players lateral velocity
        switch (lateralDirection)
        {
            case "left":
                rb.velocity = lateralSpeed * Vector2.left;//new Vector2(-1, 0);
                break;
            case "right":
                rb.velocity = lateralSpeed * Vector2.right;//new Vector2(1, 0);
                break;
            default:
                rb.velocity = Vector2.zero;//new Vector2(0, 0);
                break;
        }

        //Jumps if the jump key was pressed
        if (isJump)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.55F);

            if (hit.collider != null)
            {
                rb.AddForce(Vector2.up * jumpForce);
            }
            isJump = false;
        }
    }
}

