using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public float speed;
    public Transform startPos;


    private void Update()
    {
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime,
            transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.position = new Vector3(startPos.position.x, transform.position.y);
    }
}
