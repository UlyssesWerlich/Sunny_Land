using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaScript : MonoBehaviour
{
    private bool moveRight = true;
    private bool moveUp = true;

    public Rigidbody2D rb;
    public float velocidade = 3f;
    public Transform pontoA;
    public Transform pontoB;
    public bool moveVertical;
    public bool moveHorizontal;
    public bool canFall;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moveVertical)
        {
            if (transform.position.y < pontoA.position.y)
                moveUp = true;
            if (transform.position.y > pontoB.position.y)
                moveUp = false;

            if (moveUp)
                transform.position = new Vector2(transform.position.x, transform.position.y + velocidade * Time.deltaTime);
            else
                transform.position = new Vector2(transform.position.x, transform.position.y - velocidade * Time.deltaTime);
        }

        if (moveHorizontal)
        {
            if (transform.position.x < pontoA.position.x)
                moveRight = true;
            if (transform.position.x > pontoB.position.x)
                moveRight = false;

            if (moveRight)
                transform.position = new Vector2(transform.position.x + velocidade * Time.deltaTime, transform.position.y);
            else
                transform.position = new Vector2(transform.position.x - velocidade * Time.deltaTime, transform.position.y);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name.Equals("Player") && canFall)
        {
            moveHorizontal = false;
            moveVertical = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.mass = 50f;
            rb.gravityScale = 0.5f;
        }
    }
}
