using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    [Header("Mostradores")]
    public GameScript manager;

    [Header("Parametros do Personagem")]
    public GameObject playerDead;
    public Rigidbody2D playerRb;
    public Animator playerAnim;
    public int health;
    public float recoveryTime;

    [Header("Parametros Colisores")]
    public Transform feetPos;
    public Transform wallCheck;
    public float checkRadius;
    public float wallCheckDistance;
    public LayerMask whatIsGround;

    [Header("Movimentação e Velocidade")]
    public float speed;
    public float jumpForce;
    public float wallSlideSpeed;
    public float wallJumpForce;
    public Vector2 wallJumpDirection;
    public float hurtJumpForce;
    public Vector2 hurtJumpDirection;
    public float durationDash; //Dash pode ser utilizado numa determinada duracao
    public float dashSpeed; // Velocidade que o personagem se move durante o Dash 

    private float moveInput;
    private bool isGrounded;
    private bool isRunning;
    private int jumps = 2;
    private bool facingRight = true;

    private float dashAtual; //Duracao do Dash atual
    private bool canDash; //Variavel de controle para sabermos se ja pode executar novo dash
    private bool isDashing; //Variavel para sabermos se esta no meio de uma execucao de dash (controle de animacoes)
    private int facingDirection = 1;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool canMove = true;
    private bool isHurt = false;

    private bool recovering;

    // Start is called before the first frame update
    void Start()
    {

        isGrounded = true;
        dashAtual = durationDash;
        canDash = true;
        manager.UpdateHealthUI(health);

    }

    // Update is called once per frame
    void Update()
    {
        CheckSurroundings();
        Animate();
    }

    void FixedUpdate()
    {
        CheckInputs();
        CheckActions();
    }

    void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround); //Vai criar um circulo, na posicao do pe, com o tamanho 0.3f, whatIsGround
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetAxisRaw("Horizontal") != moveInput || Input.GetAxisRaw("Horizontal") != 0)
        {
            Walk();
        } 
        else
        {
            isRunning = false;
        }

        if (Input.GetKeyDown(KeyCode.Z) && isGrounded && canDash && !isDashing)
        {
            Dash();
        }

        if (Input.GetKeyUp(KeyCode.Z)) canDash = true;
       
    }

    void CheckActions()
    {
        CheckWallSliding();

        if (isWallSliding) playerRb.velocity = new Vector2(playerRb.velocity.x, -wallSlideSpeed);

        if (isDashing)
        {
            dashAtual -= Time.deltaTime;
            if (dashAtual <= 0) StopDash();
        }

        if (isGrounded) jumps = 2;
    }

    private void CheckWallSliding()
    {
        if (isTouchingWall && !isGrounded && playerRb.velocity.y <= 0 && moveInput != 0)
        {
            isWallSliding = true;
            
        } else
        {
            isWallSliding = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (facingRight)
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        else
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x - wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }

    void Jump() {
        
        if (jumps > 0 && !isWallSliding)
        {
            StopDash();
            jumps--;
            //playerRb.velocity = Vector2.zero;
            playerRb.velocity = new Vector2 (playerRb.velocity.x,  jumpForce); //alterar a speed no eixo y (pra cima) multiplicado pela força do pulo

        } else if (isWallSliding)
        {
            // X = forca * x * (-1 ou 1 - Esquerda ou direita)
            // Y = forca * y (sempre para cima)
            Vector2 force = new Vector2(wallJumpForce * wallJumpDirection.x * -facingDirection, wallJumpForce * wallJumpDirection.y);

            // Para a velocidade antes de atribuir, para nao ocorrer acumulo de velocidades
            playerRb.velocity = Vector2.zero;

            // Adiciona a forca do wall jump
            playerRb.AddForce(force, ForceMode2D.Impulse);

            // Retira temporariamente o controle do personagem
            StartCoroutine("StopMove");
        }
    }

    IEnumerator StopMove()
    {
        canMove = false; // Retira o controle do Personagem
        //transform.localScale = transform.localScale.x == 1 ? new Vector2(-1, 1) : Vector2.one; //Inverte o lado do transform
        Flip();
        yield return new WaitForSeconds(.3f);
        // Normaliza do lado do transform
        //transform.localScale = new Vector2(4, 4);
        //Devolve o controle do personagem
        canMove = true;
    }

    private void Walk() {

        if (!isDashing && canMove )
        {
            moveInput = Input.GetAxisRaw("Horizontal"); // <- = -1   -> = 1
            playerRb.velocity = new Vector2(moveInput * speed, playerRb.velocity.y);
            isRunning = true;

            if ((moveInput < 0 && facingRight) || (moveInput > 0 && !facingRight)) Flip();
        }
    }

    void Flip() {
        if (isDashing) StopDash();

        facingDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    void Dash() {
        if (isGrounded && canDash && !isDashing) {
            canDash = false;
            isDashing = true;
            if (facingRight) 
                playerRb.velocity = Vector2.right * dashSpeed;
            else 
                playerRb.velocity = Vector2.left * dashSpeed;
        }
    }

    private void StopDash()
    {
        playerRb.velocity = Vector2.zero;
        dashAtual = durationDash;
        isDashing = false;
    }

    public void JumpOnEnemy()
    {
        jumps = 2;
        playerRb.velocity = Vector2.zero;
        playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
    }

    public void SetHealth(int value)
    {
        if (value > 0)
        {
            health += value;
            manager.UpdateHealthUI(health);
        }
        else
        {
            if (!recovering)
            {
                health += value;
                manager.UpdateHealthUI(health);
                
                if (health <= 0)
                {
                    StartCoroutine("IsDead");
                } else
                {
                    StartCoroutine("Recovering");
                }
            }
        }        
    }

    IEnumerator Recovering()
    {
        canMove = false;
        recovering = true;
        playerRb.velocity = Vector2.zero;
        playerRb.velocity = new Vector2(hurtJumpDirection.x * hurtJumpForce * -facingDirection, hurtJumpDirection.y * hurtJumpForce);
        isHurt = true;
        yield return new WaitForSeconds(.3f);
        canMove = true;
        isHurt = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.7f);
        yield return new WaitForSeconds(recoveryTime - .3f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        recovering = false;
    }

    IEnumerator IsDead()
    {
        canMove = false;
        playerRb.velocity = Vector2.zero;
        playerAnim.SetTrigger("Dead");
        yield return new WaitForSeconds(1.2f);
        Instantiate(playerDead, transform.position, Quaternion.identity);
        manager.PlayerDead();
        Destroy(gameObject);
    }

    public void SetGem()
    {
        manager.UpdateGemUI(1);
    }

    void Animate()
    {
        playerAnim.SetBool("IsGrounded", isGrounded);
        playerAnim.SetBool("IsRunning", isRunning);
        playerAnim.SetBool("Hurt", isHurt);
        playerAnim.SetFloat("SpeedY", playerRb.velocity.y);
        playerAnim.SetFloat("Speed", Mathf.Abs(moveInput));
    }

    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.name.Equals("Plataforma"))
        {
            this.transform.parent = col.transform;
            isGrounded = true;
        }    
    }

    void OnCollisionExit2D(Collision2D col){
        if (col.gameObject.name.Equals("Plataforma"))
        //if (col.gameObject.tag.Equals("Plataforma"))
            this.transform.parent = null;
    }


    /*void DetectEnemy()
    {
        if (!isGrounded)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, -transform.up, wallCheckDistance, layerEnemy);
 
            if (hitInfo.collider != null)
            {
                if (hitInfo.collider.CompareTag("eagle"))
                {
                    hitInfo.collider.GetComponent<EagleScript>().TakeDamage(1);
                    playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
                }
            }
        }
    }*/
}
