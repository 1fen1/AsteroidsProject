using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script for the Ship object
public class Ship : MonoBehaviour
{
    [SerializeField]
    GameObject prefabBullet;

    Rigidbody2D rb2D;
    Vector2 thrustDirection = new Vector2(1, 0);
    const float ThrustForce = 15;
    const float RotateDegreesPerSecond = 180;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //check for rotation input
        float rotationInput = Input.GetAxis("Rotate");
        if (rotationInput != 0)
        {
            float rotationAmount = RotateDegreesPerSecond * Time.deltaTime;
            if (rotationInput < 0)
            {
                rotationAmount *= -1;
            }
            transform.Rotate(Vector3.forward, rotationAmount);

            // change thrust direction to match ship rotation
            float zRotation = transform.eulerAngles.z * Mathf.Deg2Rad;
            thrustDirection.x = Mathf.Cos(zRotation);
            thrustDirection.y = Mathf.Sin(zRotation);
        }

        // shooting bullet
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            GameObject bullet = Instantiate(prefabBullet, transform.position, Quaternion.identity);
            Bullet script = bullet.GetComponent<Bullet>();
            script.ApplyForce(thrustDirection);
        }
    }

    // FixedUpdate is called 50 times per second
    void FixedUpdate()
    {
        if (Input.GetAxis("Thrust") != 0)
        {
            rb2D.AddForce(ThrustForce * thrustDirection, ForceMode2D.Force);
        }
    }

    // Destroys Ship object on collision with Asteroid object
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Asteroid"))
        {
            Destroy(gameObject);
        }
    }
}
