using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float InputX;
    
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private float jumpForce;
    [SerializeField] public float speed;

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    private int jumpCount;

    [Header("Dash info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;  //一次冲刺持续时间
    private float dashTime;
    [SerializeField] private float dashCooldown;  //冲刺冷却时间
    private float dashCooldownTime;

    [Header("Attack info")]
    [SerializeField] private bool isAttacking;
    [SerializeField] private int comboCount;
    [SerializeField]private float max_combo_delay;              //最大的连击间隔
    private float comboTime;

    private int facingDir = 1;
    private bool facingRight = true;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        CheckInput();
        Movement();

        dashTime-= Time.deltaTime;
        dashCooldownTime -= Time.deltaTime;
        comboTime-= Time.deltaTime;

        FlipController();
        AnimatorControllers();
    }

    private void GroundCheck()
    {
        //用射线检测地面，可防止将墙壁误认为是地面
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        //实现2段跳，这里设置为1是因为跳起来后，射线任有一段极短的时间接触地面重置计数
        if (isGrounded)
        {
            jumpCount = 1;
        }
    }

    private void CheckInput()
    {
        InputX = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
        if (Input.GetButtonDown("Jump") && jumpCount>0)
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Dash();
        }
    }

    private void Dash()
    {
        if (dashCooldownTime < 0 && !isAttacking)
        {
            dashTime = dashDuration;
            dashCooldownTime = dashCooldown;
        }
    }

    private void Attack()
    {
        isAttacking = true;
        if (comboTime >= 0)
        {
            comboCount = (comboCount + 1) % 3;
        }
        else
        {
            comboCount = 0;
        }
        comboTime = max_combo_delay;
    }

    private void Movement()
    {
        if (isAttacking)
        {
            rb.velocity = new Vector2(0, 0);
        }
        else if (dashTime > 0)
        {
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(InputX * speed, rb.velocity.y);
        }
    }

    private void Jump()
    {
        jumpCount -= 1;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void AnimatorControllers()
    {
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isMoving", rb.velocity.x != 0);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", dashTime > 0);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCount", comboCount);
    }
    
    private void Flip()
    {
        facingDir = -1 * facingDir;
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
    }

    private void FlipController()
    {
        if (facingRight && rb.velocity.x < 0 )
        {
            Flip();
        }
        if(!facingRight && rb.velocity.x > 0 )
        {
            Flip();
        }
    }
    private void OnDrawGizmos()
    {
        //绘制两点时间的线段
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x,transform.position.y-groundCheckDistance));
    }
    public void AttackOver()
    {
        isAttacking = false;
    }
}
