using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetPositionScript : MonoBehaviour
{

    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("eagle"))
        {
            collision.gameObject.GetComponent<EagleScript>().TakeDamage(damage);
            gameObject.GetComponentInParent<PlayerScript>().JumpOnEnemy();
        }
        if (collision.CompareTag("opossum"))
        {
            collision.gameObject.GetComponent<OpossumScript>().TakeDamage(damage);
            gameObject.GetComponentInParent<PlayerScript>().JumpOnEnemy();
        }
        if (collision.CompareTag("frog"))
        {
            collision.gameObject.GetComponent<FrogScript>().TakeDamage(damage);
            gameObject.GetComponentInParent<PlayerScript>().JumpOnEnemy();
        }
    }
}
