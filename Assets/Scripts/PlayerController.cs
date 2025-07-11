using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerIndex = 1; // 1이면 1P, 2면 2P
    public float moveSpeed = 5f;  // 이동 속도
    public float jumpForce = 7f;  // 점프 힘
    private bool isGrounded;      // 착지 여부 체크
    private Rigidbody2D rb;       // Rigidbody2D 컴포넌트
    private bool isCrouching;     // 숙이기 상태

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleCrouch();
    }

    // 이동 처리
    private void HandleMovement()
    {
        if (rb == null) return;
        float moveInput = Input.GetAxis(playerIndex == 1 ? "Horizontal" : "Horizontal2");
        Vector2 moveVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = moveVelocity;
    }

    // 점프 처리
    private void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown(playerIndex == 1 ? "Jump" : "Jump2"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // 숙이기 처리
    private void HandleCrouch()
    {
        if (Input.GetKeyDown(playerIndex == 1 ? KeyCode.C : KeyCode.K))
        {
            isCrouching = !isCrouching;
            // 애니메이션 및 크기 조정 등을 추가할 수 있음
        }
    }

    // 착지 여부 확인
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}