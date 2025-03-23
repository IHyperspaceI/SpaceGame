using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public GameObject moveableObjects;
    public float maxRange;
    public float mass;


    // Update is called once per frame
    void Update()
    {
        foreach (Transform gravityObject in moveableObjects.transform)
        {
            Vector2 direction = (gravityObject.position - transform.position);
            float magnitude = Mathf.Clamp((gravityObject.position - transform.position).magnitude, 0, maxRange) / maxRange;

            gravityObject.GetComponent<Rigidbody2D>().AddForce((1 - magnitude) * -mass * direction);
        }
    }
}
