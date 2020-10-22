using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EagleScript : MonoBehaviour
{

    [Header("Trajeto")]
    public GameObject point1;
    public GameObject point2;
    public float canMoveDistance;
    public float velocity;
    public float velocityAttack;

    [Header("Localização do Jogador")]
    public Transform player;

    [Header("Dano")]
    public int damage;
    public int health;

    [Header("Animação")]
    public GameObject death;
    public Animator eagleAnim;

    private bool canMove;
    private bool canAttack;
    private bool isAttacking;
    private int moveRight;
    private bool facingRight;

    private Vector2 route1;
    private Vector2 route2;
    private Vector2 target;

    private float testPlayerTime = .5f;
    private float testPlayerCounter = 0;


    // Start is called before the first frame update
    void Start()
    {
        route1 = point1.transform.position;
        route2 = point2.transform.position;
        canAttack = true;
        moveRight = -1;
        facingRight = false;
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
            Animate();
            Move();
            WatchPlayer();
        }

        if (isAttacking)
        {
            MoveAttack();
            VerifyTarget();
        }

    }

    private void CanMove()
    {
        if (testPlayerCounter >= testPlayerTime)
        {
            if (TestPlayer())
            {
                if (Vector2.Distance(transform.position, player.position) < canMoveDistance && !isAttacking)
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

    bool TestPlayer()
    {
        return GameObject.FindGameObjectWithTag("player");
    }

    private void Move()
    {
        if (transform.position.x < route1.x || transform.position.x > route2.x)
        {
            if (transform.position.x < route1.x ) moveRight = 1;
            if (transform.position.x > route2.x ) moveRight = -1;
            if ((moveRight < 0 && facingRight) || (moveRight > 0 && !facingRight)) Flip();
        }

        if (transform.position.y < route2.y)
            transform.position = new Vector2(transform.position.x + velocity * moveRight * Time.deltaTime, transform.position.y + velocity * Time.deltaTime);
        else
            transform.position = new Vector2(transform.position.x + velocity * moveRight * Time.deltaTime, transform.position.y);
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void WatchPlayer()
    {
        if (TestPlayer()){
            if (route1.x < player.position.x && route1.y < player.position.y && route2.x > player.position.x && route2.y > player.position.y && canAttack)
            {
                if (transform.position.x > player.position.x && !facingRight || transform.position.x < player.position.x && facingRight)
                {
                    canAttack = false;
                    StartCoroutine("PrepareAttack");
                }
            }
        }
    }

    IEnumerator PrepareAttack()
    {
        moveRight = 0;
        yield return new WaitForSeconds(0.6f);
        target = player.position;
        if ((target.x < transform.position.x && facingRight) || (target.x > transform.position.x && !facingRight)) Flip();
        isAttacking = true ;
        canMove = false;
    }

    private void MoveAttack()
    {
        Animate();
        transform.position = Vector2.MoveTowards(transform.position, target, velocityAttack * Time.deltaTime);
    }

    private void VerifyTarget()
    {
        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            ReturnRoute();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            ReturnRoute();
            collision.gameObject.GetComponent<PlayerScript>().SetHealth(-damage);
        }
    }

    private void ReturnRoute()
    {
        canMove = true;
        isAttacking = false;

        if (facingRight)
            moveRight = 1;
        else
            moveRight = -1;
        StartCoroutine("AttackCoolDown");
    }

    IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(3);
        canAttack = true;
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

    private void Animate()
    {
        eagleAnim.SetBool("flapWings", canMove);
    }

}
