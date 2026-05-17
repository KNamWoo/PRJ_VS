/*
    최초 작성일:26/05/17
    최종 변경일:26/05/17
    
    수정자
    - 김남우
    -
    
    목적
    - Boss와의 전투시 횡스크롤/탑다운으로 전환하는 로직을 개발하기 위해
      임시 Code 작성
*/

using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public enum GameMode
    {
        TopDown,
        SideScroll
    }

    [Header("Move")]
    [SerializeField] private float speed;

    [Header("SideScroll")]
    [SerializeField] private float sideScrollGravity;
    [SerializeField] private float jumpPower;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;

    private GameMode currentMode = GameMode.TopDown;

    private Vector2 inputVec;
    private Rigidbody2D rbody;

    public bool jumpRequested;
    public bool isGrounded;

    private bool previousGrounded;

    private void Awake()
    {
        rbody             = GetComponent<Rigidbody2D>();
        speed             = 3f;
        sideScrollGravity = 1f;
        jumpPower         = 8f;
        groundCheckRadius = 0.15f;
    }

    private void Start()
    {
        ChangeToTopDown();
    }

    private void FixedUpdate()
    {
        UpdateGroundedState();

        switch (currentMode)
        {
            case GameMode.TopDown:
                MoveTopDown();
                break;

            case GameMode.SideScroll:
                MoveSideScroll();
                HandleJump();
                break;
        }
    }

    private void MoveTopDown()
    {
        rbody.linearVelocity = inputVec * speed;
    }

    private void MoveSideScroll()
    {
        rbody.linearVelocity = new Vector2(
            inputVec.x * speed,
            rbody.linearVelocity.y
        );
    }

    private void HandleJump()
    {
        if (!jumpRequested)
            return;

        jumpRequested = false;

        if (!isGrounded)
            return;

        rbody.linearVelocity = new Vector2(
            rbody.linearVelocity.x,
            jumpPower
        );
    }

    private void UpdateGroundedState()
    {
        isGrounded = CheckGrounded();

        // 상태가 바뀔 때만 로그 출력
        if (isGrounded != previousGrounded)
        {
            Debug.Log("Grounded: " + isGrounded);
            previousGrounded = isGrounded;
        }
    }

    private bool CheckGrounded()
    {
        if (groundCheck == null)
            return false;

        return Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    private void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (!value.isPressed)
            return;

        if (currentMode != GameMode.SideScroll)
            return;

        jumpRequested = true;
    }

    public void ChangeToTopDown()
    {
        currentMode = GameMode.TopDown;

        rbody.gravityScale = 0f;
        rbody.linearVelocity = Vector2.zero;
    }

    public void ChangeToSideScroll()
    {
        currentMode = GameMode.SideScroll;

        rbody.gravityScale = sideScrollGravity;
        rbody.linearVelocity = Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        if (groundCheck == null)
            return;

        if (Application.isPlaying)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
        }
        else
        {
            Gizmos.color = Color.yellow;
        }

        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}