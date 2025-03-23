using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    public float force;
    public float forceTime;

    private bool isPushing;
    private float startTime;
    private Transform theTransform;

    // Update is called once per frame
    void Update()
    {
        if (isPushing && Time.fixedTime < startTime + forceTime)
        {
            Push();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isPushing = true;
        theTransform = collision.transform;
        startTime = Time.fixedTime;
    }

    void Push()
    {
        theTransform.GetComponent<Rigidbody2D>().AddForce(-theTransform.position * force);
    }
}
