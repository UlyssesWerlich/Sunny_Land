using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BackGroundScript : MonoBehaviour
{

    public Transform cameraView;
    public GameObject ground1;
    public GameObject ground2;
    //public GameObject ground3;
    //public GameObject ground4;

    public float rotate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void FixedUpdate()
    {
        //Rotate();
        MoveWithCamera();
    }

    void Rotate()
    {
        /*if (ground1.transform.position.x <= -17.7)
            ground1.transform.position = new Vector3(29.5f, ground1.transform.position.y);
        else
            ground1.transform.position = new Vector3(ground1.transform.position.x - rotate, ground1.transform.position.y);

        if (ground2.transform.position.x <= -17.7)
            ground2.transform.position = new Vector3(29.5f, ground2.transform.position.y);
        else
            ground2.transform.position = new Vector3(ground2.transform.position.x - rotate, ground2.transform.position.y);

        if (ground3.transform.position.x <= -17.7)
            ground3.transform.position = new Vector3(29.5f, ground3.transform.position.y);
        else
            ground3.transform.position = new Vector3(ground3.transform.position.x - rotate, ground3.transform.position.y);

        if (ground4.transform.position.x <= -17.7)
            ground4.transform.position = new Vector3(29.5f, ground4.transform.position.y);
        else
            ground4.transform.position = new Vector3(ground4.transform.position.x - rotate, ground4.transform.position.y);*/
    }

    void MoveWithCamera()
    {
        Vector2 position = cameraView.position;
        transform.position = new Vector2(position.x, position.y + 2);
    }
}
