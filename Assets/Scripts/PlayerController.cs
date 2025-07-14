using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerIndex = 1;      // 1이면 1P, 2면 2P
    public float moveSpeed = 5f;     // 이동 속도

    public bool isGrounded;         // 착지 여부
    private bool isCrouching;        // 숙이기 상태

    private Rigidbody2D rb;          // Rigidbody2D 컴포넌트
    private Animator animator;       // Animator 컴포넌트

    private bool isAttacking = false;

public float jumpForce = 7f;
public float fallMultiplier = 2.5f;
public float lowJumpMultiplier = 2f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleCrouch();
        HandleAttack();
    }

    // 이동 처리
    private void HandleMovement()
{
    float moveInput = Input.GetAxis(playerIndex == 1 ? "Horizontal" : "Horizontal2");

    float currentSpeed = isAttacking ? moveSpeed * 0.5f : moveSpeed;
    Vector2 moveVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
    rb.linearVelocity = moveVelocity;

    // 방향 반전
    if (Mathf.Abs(moveInput) > 0.01f)
    {
        Vector3 scale = transform.localScale;
        scale.x = moveInput > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    if (animator != null)
    {
        animator.SetBool("isRunning", Mathf.Abs(moveInput) > 0.1f);
    }
}



    // 점프 처리
private void HandleJump()
{
    if (isGrounded && Input.GetButtonDown(playerIndex == 1 ? "Jump" : "Jump2"))
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    // 공중에서 중력 보정
    if (rb.linearVelocity.y < 0) // 내려올 때
    {
        rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
    }
    else if (rb.linearVelocity.y > 0 && !Input.GetButton(playerIndex == 1 ? "Jump" : "Jump2")) // 올라가다 키 뗐을 때
    {
        rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }
}

    // 숙이기 처리
    private void HandleCrouch()
    {
        if (Input.GetKeyDown(playerIndex == 1 ? KeyCode.C : KeyCode.K))
        {
            isCrouching = !isCrouching;

            if (animator != null)
            {
                animator.SetBool("isCrouching", isCrouching);
            }
        }
    }


private void HandleAttack()
{
    if (!isAttacking && Input.GetKeyDown(playerIndex == 1 ? KeyCode.Z : KeyCode.M))
    {
        animator.SetTrigger("Attack");
        isAttacking = true;
    }
}

    // 착지 여부 체크
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            if (animator != null)
            {
                animator.SetBool("isJumping", false);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;

            if (animator != null)
            {
                animator.SetBool("isJumping", true);
            }
        }
    }
    
    // 애니메이션 끝나는 시점에 호출할 함수 (애니메이션 이벤트 사용)
public void AttackFinished()
{
    isAttacking = false;
}
}
