using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;  // 이동 속도
    public float jumpForce = 7f;  // 점프 힘
    private bool isGrounded;      // 착지 여부 체크
    private Rigidbody2D rb;       // Rigidbody2D 컴포넌트
    private bool isCrouching;     // 숙이기 상태

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Rigidbody2D 컴포넌트 할당
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
        float moveInput = Input.GetAxis("Horizontal");  // A/D 또는 Left/Right 키로 이동
        Vector2 moveVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y); // Y값은 점프만 처리
        rb.linearVelocity = moveVelocity;
    }

    // 점프 처리
    private void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))  // Space 키로 점프
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // 숙이기 처리
    private void HandleCrouch()
    {
        if (Input.GetButtonDown("Crouch"))  // C 키로 숙이기
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

