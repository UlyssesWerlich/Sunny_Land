using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpossumScript : MonoBehaviour
{
    [Header("Localização do Jogador")]
    public Transform player;

    [Header("Trajeto")]
    public GameObject point1;
    public GameObject point2;
    public float canMoveDistance;

    [Header("Corpo Físico")]
    public Animator opossumAnim;
    public Rigidbody2D opossumRb;
    public float velocity;
    public int health;

    [Header("Dano")]
    public int damage;
    public GameObject death;

    private bool canMove;
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
            VerifyDirection();
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
        transform.position = new Vector2(transform.position.x + velocity * moveRight * Time.deltaTime, transform.position.y);
    }

    private void VerifyDirection()
    {

        if (transform.position.x < route1.x) moveRight = 1;
        if (transform.position.x > route2.x) moveRight = -1;
        if ((moveRight < 0 && facingRight) || (moveRight > 0 && !facingRight))
        {
            Flip();
        }
    }

    void Flip()
    {
        opossumRb.velocity = Vector2.zero;
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void Animate()
    {
        opossumAnim.SetBool("walk", canMove);
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
        float move = velocity;
        opossumRb.velocity = Vector2.zero;
        velocity = 0;
        yield return new WaitForSeconds(1.5f);
        velocity = move;
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
