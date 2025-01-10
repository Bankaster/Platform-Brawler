using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackgroundAnimation : MonoBehaviour
{
    public GameObject image1;
    public GameObject image2;
    public float speed = 50f;
    public float resetPosition = -1496f;
    public float startPosition = 3900f;

    void Update()
    {
        //Move the images to the left 
        image1.transform.Translate(Vector3.left * speed * Time.deltaTime);
        image2.transform.Translate(Vector3.left * speed * Time.deltaTime);

        //Reset the image position if reaches the end position
        if (image1.transform.position.x <= resetPosition)
        {
            image1.transform.position = new Vector3(startPosition, image1.transform.position.y, image1.transform.position.z);
        }

        if (image2.transform.position.x <= resetPosition)
        {
            image2.transform.position = new Vector3(startPosition, image2.transform.position.y, image2.transform.position.z);
        }
    }
}
