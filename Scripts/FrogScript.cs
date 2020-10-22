using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogScript : MonoBehaviour
{

    [Header("Localização do Jogador")]
    public Transform player;

    [Header("Trajeto")]
    public GameObject point1;
    public GameObject point2;
    public Transform feetPosition;
    public float canMoveDistance;
    public LayerMask ground;
    public float checkGroundRadius;

    [Header("Corpo Físico")]
    public Animator frogAnim;
    public Rigidbody2D frogRb;
    public float jumpForce;
    public Vector2 jumpDirection;
    public int health;

    [Header("Dano")]
    public int damage;

    public GameObject death;

    private bool canMove;
    private bool isGrounded;
    private bool canJump;
    private int moveRight;
    private bool facingRight;
    private Vector2 route1;
    private Vector2 route2;

    private float testPlayerTime = .5f;
    private float testPlayerCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        route1 = point1.transform.position;
        route2 = point2.transform.position;
        moveRight = -1;
        facingRight = false;
        canJump = true;
        player = GameObject.FindGameObjectWithTag("player").transform;
        Destroy(point1);
        Destroy(point2);
    }

    // Update is called once per frame
    void Update()
    {
        CanMove();
        if (canMove)
        {
            Move();
            Animate();
        }
    }

    private void CanMove()
    {
        if (testPlayerCounter >= testPlayerTime)
        {
            if (GameObject.FindGameObjectWithTag("player") != null)
            {
                if (Vector2.Distance(transform.position, player.position) < canMoveDistance)
                {
                    canMove = true;
                }
                else
                {
                    canMove = false;
                }
                testPlayerCounter = 0;
            }
        }
        else
        {
            testPlayerCounter += Time.deltaTime;
        }
    }

    private void Move()
    {
        VerifyDirection();

        IsGrounded();

        if (isGrounded && canJump) Jump();
    }

    private void IsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(feetPosition.position, checkGroundRadius, ground);
    }

    private void Jump()
    {
        Vector2 force = new Vector2(jumpForce * jumpDirection.x * moveRight, jumpForce * jumpDirection.y);
        frogRb.velocity = Vector2.zero;
        frogRb.AddForce(force, ForceMode2D.Impulse);
    }

    private void VerifyDirection()
    {
        if (isGrounded)
        {
            if (transform.position.x < route1.x) moveRight = 1;
            if (transform.position.x > route2.x) moveRight = -1;
            if ((moveRight < 0 && facingRight) || (moveRight > 0 && !facingRight))
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        StartCoroutine("WaitFlip");
        frogRb.velocity = Vector2.zero;
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    IEnumerator WaitFlip()
    {
        canJump = false;
        yield return new WaitForSeconds(1);
        canJump = true;

    }

    private void Animate()
    {
        if (!canMove) frogAnim.SetInteger("State", 0);
        if (canMove && isGrounded) frogAnim.SetInteger("State", 1);
        if (canMove && !isGrounded && frogRb.velocity.y > 0.01) frogAnim.SetInteger("State", 2);
        if (canMove && !isGrounded && frogRb.velocity.y < -0.01) frogAnim.SetInteger("State", 3);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().SetHealth(-damage);
            StartCoroutine("WaitAfterAttack");
        }
    }

    IEnumerator WaitAfterAttack()
    {
        float move = jumpForce;
        frogRb.velocity = Vector2.zero;
        jumpForce = 0;
        yield return new WaitForSeconds(1.5f);
        jumpForce = move;
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health >= 0) IsDestroyed();
    }

    private void IsDestroyed()
    {
        Instantiate(death, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
