using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera camera;

    private SpaceshipMover mover;


    private void Start()
    {
        mover = transform.GetComponent<SpaceshipMover>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 screenPosition = camera.ScreenToWorldPoint(Input.mousePosition);
        mover.Move(screenPosition);
    }
}
