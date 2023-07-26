using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script for bullet object
public class Bullet : MonoBehaviour
{
    // bullet death time
    const float LifeSeconds = 2;
    Timer deathTimer;

    // Start is called before the first frame update
    void Start()
    {
        // start bullet death timer
        deathTimer = gameObject.AddComponent<Timer>();
        deathTimer.Duration = LifeSeconds;
        deathTimer.Run();
    }

    // Update is called once per frame
    void Update()
    {
        //destroy bullet when the timer is finished
        if (deathTimer.Finished)
        {
            Destroy(gameObject);
        }
    }

    // applies force to the bullet in the direction
    public void ApplyForce(Vector2 forceDirection)
    {
        const float ForceMagnitude = 5;
        GetComponent<Rigidbody2D>().AddForce(ForceMagnitude * forceDirection, ForceMode2D.Impulse);
    }
}
