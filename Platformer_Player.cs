using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Platformer_Player : MonoBehaviour
{
    public float speed = 3.0f;
    public float jumpForce = 600.0f;
    Vector2 velocity;
    [SerializeField] TextMeshProUGUI textJumpState;
    [SerializeField] LayerMask groundLayer;     // 바닥으로 판단할 레이어
    [SerializeField] Transform groundCheck;     // 바닥 체크 위치
    public float groundCheckDistance = 0.2f;    // 바닥 체크 거리

    public bool isJumping = false;
    private enum JumpState { Grounded, Rising, Falling }
    private JumpState jumpState = JumpState.Grounded;

    new Rigidbody2D rigidbody;
    Animator animator;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float _hozInput = Input.GetAxisRaw("Horizontal");
        velocity = new Vector2(_hozInput * speed, 0);

        if (velocity.x != 0)
            animator.SetBool("isWalk", true);
        else
            animator.SetBool("isWalk", false);

        if (Input.GetKeyDown(KeyCode.Space) && jumpState == JumpState.Grounded)
        {
            animator.SetTrigger("jump");
        }

        if (_hozInput > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_hozInput < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        // 바닥 체크
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        bool isGrounded = hit.collider != null;

        // 점프 상태
        if (isGrounded)
        {
            isJumping = false;
            jumpState = JumpState.Grounded;
        }
        else
        {
            if (rigidbody.velocity.y > 0.1f)
            {
                jumpState = JumpState.Rising;
            }
            else if (rigidbody.velocity.y < -0.1f)
            {
                jumpState = JumpState.Falling;
            }
        }

        // 상태에 따른 애니메이션 설정
        switch (jumpState)
        {
            case JumpState.Rising:
                animator.SetBool("isRising", true);
                animator.SetBool("isFalling", false);
                break;
            case JumpState.Falling:
                animator.SetBool("isRising", false);
                animator.SetBool("isFalling", true);
                break;
            case JumpState.Grounded:
                animator.SetBool("isRising", false);
                animator.SetBool("isFalling", false);
                break;
        }
        textJumpState.text = jumpState.ToString();
    }

    void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(velocity.x, rigidbody.velocity.y);
    }

    public void PlayerJump()
    {
        rigidbody.AddForce(new Vector2(0, jumpForce));
        isJumping = true;
    }

    // Gizmos를 사용하여 바닥 체크 영역을 시각적으로 표시
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }
    }
}
