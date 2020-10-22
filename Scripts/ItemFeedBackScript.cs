using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFeedBackScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("FeedBack");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FeedBack()
    {
        yield return new WaitForSeconds(.4f);
        Destroy(gameObject);
    }
}
