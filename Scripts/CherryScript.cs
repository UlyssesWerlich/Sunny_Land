using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryScript : MonoBehaviour
{
    public int lifeUp;
    public Animator cherryAnim;
    public GameObject feedBack;

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
        if (collision.CompareTag("player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().SetHealth(lifeUp);
            IsTaked();
        }
    }
    private void IsTaked()
    {
        Instantiate(feedBack, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
