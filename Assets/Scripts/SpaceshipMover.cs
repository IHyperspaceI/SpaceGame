using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipMover : MonoBehaviour
{
    public float speed;
    public float power;
    public float inputDeadzone;

    public GameObject trailPrefab;
    public float trailSpeed;
    public float trailTime;
    public float trailCooldownTime;

    private Rigidbody2D rigidbody;
    private bool trailCooldown;

    private float trailIntensity;

    public void Start()
    {
        rigidbody = transform.GetComponent<Rigidbody2D>();
    }

    public void Move(Vector3 target)
    {
        Vector3 direction = (transform.position - target);
        float magnitude = Mathf.Clamp((transform.position - target).magnitude, 0, 10) / 10;
        float currentSpeed = Mathf.Clamp(transform.GetComponent<Rigidbody2D>().velocity.magnitude, 0, speed) / speed;

        trailIntensity = direction.magnitude / 10;

        if (direction.magnitude > inputDeadzone)
        {
            rigidbody.AddForce(magnitude * -power * direction);
        }


        // Calculate the angle in radians then convert to degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the object (Quaternion for 2D rotation on the Z axis)
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));

        if (rigidbody.velocity.magnitude > speed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * speed;
        }

        StartCoroutine(Trail());
    }

    IEnumerator Trail()
    {
        if (!trailCooldown)
        {
            trailCooldown = true;
            GameObject trail = Instantiate(trailPrefab);

            trail.transform.position = transform.position;
            trail.GetComponent<Rigidbody2D>().velocity = transform.GetComponent<Rigidbody2D>().velocity;
            trail.transform.parent = transform.parent;
            trail.transform.rotation = transform.rotation;
            trail.transform.GetComponent<Rigidbody2D>().AddForce(transform.up * -trailSpeed * trailIntensity);

            yield return new WaitForSeconds(trailCooldownTime);

            trailCooldown = false;
            StartCoroutine(FadeOutTrail(trail.GetComponent<SpriteRenderer>()));
        }
    }

    IEnumerator FadeOutTrail(SpriteRenderer trailRenderer)
    {
        float elapsedTime = 0f;
        Color trailColor = trailRenderer.color;

        // Gradually increase the alpha value from 0 to 1
        while (elapsedTime < trailTime)
        {
            elapsedTime += Time.deltaTime;
            trailColor.a = 1 - Mathf.Clamp01(elapsedTime / trailTime);
            trailRenderer.color = trailColor;

            yield return null;
        }

        // Ensure the alpha is fully set to 1 at the end
        Destroy(trailRenderer.transform.gameObject);
    }

}
