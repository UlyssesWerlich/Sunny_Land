using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour
{

    public Animator gemAnim;
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
            collision.gameObject.GetComponent<PlayerScript>().SetGem();
            IsTaked();
        }
    }

    private void IsTaked()
    {
        Instantiate(feedBack, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
