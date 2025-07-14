using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerIndex = 1;          // 1P 또는 2P
    public float moveSpeed = 5f;         // 이동 속도
    public float jumpForce = 7f;         // 점프 힘
    public float fallMultiplier = 2.5f;  // 하강 시 중력 가중치
    public float lowJumpMultiplier = 2f; // 점프 중간에 키 뗄 때 중력 가중치

    public bool isGrounded;              // 착지 여부

    private bool isCrouching = false;    // 숙이기 상태
    private bool isAttacking = false;    // 공격 중 여부
    private bool isGuarding = false;     // 방어 중 여부
   private float velocityXSmooth = 0f;
private float decelerationRate = 10f; // 감속 속도 조절

    private Rigidbody2D rb;              // Rigidbody2D 컴포넌트
    private Animator animator;           // Animator 컴포넌트

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
        HandleGuard();
    }

    // 이동 처리

private void HandleMovement()
{
    float moveInput = Input.GetAxis(playerIndex == 1 ? "Horizontal" : "Horizontal2");

    float targetSpeed = isAttacking ? moveSpeed * 0.5f : moveSpeed;

    if (isGuarding)
    {
        // 가드 중일 때는 이동 입력 무시하고 기존 속도를 서서히 0으로 감속
        velocityXSmooth = Mathf.MoveTowards(velocityXSmooth, 0, decelerationRate * Time.deltaTime);
    }
    else
    {
        // 평소는 입력값에 맞게 속도 변경
        velocityXSmooth = moveInput * targetSpeed;
    }

    rb.linearVelocity = new Vector2(velocityXSmooth, rb.linearVelocity.y);

    // 방향 반전 (가드 중에도 움직임이 있으면 회전 유지)
    if (Mathf.Abs(velocityXSmooth) > 0.01f)
    {
        Vector3 scale = transform.localScale;
        scale.x = velocityXSmooth > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    if (animator != null)
    {
        animator.SetBool("isRunning", Mathf.Abs(velocityXSmooth) > 0.1f);
    }
}


    // 점프 처리
    private void HandleJump()
{
    if (isGrounded && Input.GetKeyDown(KeyCode.W))  // 무조건 W키로 점프
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    // 공중에서 중력 보정
    if (rb.linearVelocity.y < 0) // 내려올 때
    {
        rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
    }
    else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.W)) // 올라가다 키 뗐을 때
    {
        rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }
}


    // 숙이기 처리 (S 키)
    private void HandleCrouch()
    {
        if (Input.GetKeyDown(playerIndex == 1 ? KeyCode.S : KeyCode.K))
        {
            isCrouching = !isCrouching;
            animator?.SetBool("isCrouching", isCrouching);
        }
    }

    // 공격 처리 (F 키)
    private void HandleAttack()
    {
        if (!isAttacking && Input.GetKeyDown(playerIndex == 1 ? KeyCode.F : KeyCode.M))
        {
            animator?.SetTrigger("Attack");
            isAttacking = true;
        }
    }

    // 방어 처리 (G 키)
    private void HandleGuard()
{
    // 1P는 G키, 2P는 콤마키(,)를 누르고 있으면 가드 상태 유지
    bool guardKey = playerIndex == 1 ? Input.GetKey(KeyCode.G) : Input.GetKey(KeyCode.Comma);
    isGuarding = guardKey;
    animator?.SetBool("isGuarding", isGuarding);
}

    // 착지 여부 체크
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator?.SetBool("isJumping", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator?.SetBool("isJumping", true);
        }
    }

    // 공격 애니메이션 종료 시 호출 (애니메이션 이벤트로 연결)
    public void AttackFinished()
    {
        isAttacking = false;
    }
}
