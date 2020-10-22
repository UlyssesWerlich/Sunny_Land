using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("IsDestroyed");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator IsDestroyed()
    { 
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
}
