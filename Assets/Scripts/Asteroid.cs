using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for Asteroid object
public class Asteroid : MonoBehaviour
{
    [SerializeField]
    Sprite asteroidSpriteGreen;
    [SerializeField]
    Sprite asteroidSpriteRed;
    [SerializeField]
    Sprite asteroidSpriteWhite;

    // Start is called before the first frame update
    void Start()
    {       
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        int spriteNumber = Random.Range(0, 3);
        switch (spriteNumber)
        {
            case 0:
                spriteRenderer.sprite = asteroidSpriteGreen;
                break;
            case 1:
                spriteRenderer.sprite = asteroidSpriteRed;
                break;
            case 2:
                spriteRenderer.sprite = asteroidSpriteWhite;
                break;
            default:
                break;
        }
    }

    public void Initialize(Direction direction, Vector3 position)
    {
        // set asteroid position
        transform.position = position;

        // set random angle based on direction
        float angle;
        float randomAngle = Random.value * 30f * Mathf.Deg2Rad;
        switch (direction)
        {
            case Direction.Up:
                angle = 75 * Mathf.Deg2Rad + randomAngle;
                break;
            case Direction.Down:
                angle = 255 * Mathf.Deg2Rad + randomAngle;
                break;
            case Direction.Left:
                angle = 165 * Mathf.Deg2Rad + randomAngle;
                break;
            case Direction.Right:
                angle = -15 * Mathf.Deg2Rad + randomAngle;
                break;
            default:
                angle = 0f;
                break;
        }

        // get game object moving
        StartMoving(angle);
    }

    // starts the asteroid moving at the given angle
    public void StartMoving(float angle)
    {
        // apply impulse force to get asteroid moving
        const float MinImpulseForce = 1f;
        const float MaxImpulseForce = 3f;
        Vector2 moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        float magnitude = Random.Range(MinImpulseForce, MaxImpulseForce);
        GetComponent<Rigidbody2D>().AddForce(moveDirection * magnitude, ForceMode2D.Impulse);
    }

    // destroy asteroid on collision with bullet
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Bullet"))
        {
            Destroy(coll.gameObject);

            // destroy or split as appropriate
            if (transform.localScale.x < 0.5f)
            {
                Destroy(gameObject);
            }
            else
            {
                // shrink asteroid to half size
                Vector3 scale = transform.localScale;
                scale.x /= 2;
                scale.y /= 2;
                transform.localScale = scale;

                // cut collider radius in half
                CircleCollider2D collider = GetComponent<CircleCollider2D>();
                collider.radius /= 2;

                // clone twice and destroy original
                GameObject newAsteroid = Instantiate<GameObject>(gameObject, transform.position, Quaternion.identity);
                newAsteroid.GetComponent<Asteroid>().StartMoving(Random.Range(0, 2 * Mathf.PI));
                newAsteroid = Instantiate<GameObject>(gameObject, transform.position, Quaternion.identity);
                newAsteroid.GetComponent<Asteroid>().StartMoving(Random.Range(0, 2 * Mathf.PI));
                Destroy(gameObject);
            }
        }
    }
}
