using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector2 direction;
    float speed;
    float downSpeed = 4f;

    bool roadGrav = false;
    [SerializeField] float angularVelocity = 0;
    public SpriteRenderer sprite;


    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void Update()
    {


        direction = rotate(direction, angularVelocity * Time.deltaTime);
        transform.Translate((speed * direction + Vector2.down * downSpeed) * Time.deltaTime);
    }

    public void init(float sp, Vector2 dir)
    {
        speed = sp;
        direction = dir.normalized;
    }

    public void init(float sp, Vector2 dir, bool g)
    {
        speed = sp;
        direction = dir.normalized;
        roadGrav = g;
        
        if (!g)
        {
            downSpeed = 0;
        }
       // Debug.Log(speed * direction + Vector2.down * downSpeed);
    }

    public void init(float sp, Vector2 dir, bool g, float angVel)
    {
        speed = sp;
        direction = dir.normalized;
        roadGrav = g;
        angularVelocity = angVel;

        if (!g)
        {
            downSpeed = 0;
        }
        // Debug.Log(speed * direction + Vector2.down * downSpeed);
    }

    public static Vector2 rotate(Vector2 v, float degrees)
    {
        return new Vector2(
            v.x * Mathf.Cos(Mathf.Deg2Rad * degrees) - v.y * Mathf.Sin(Mathf.Deg2Rad * degrees),
            v.x * Mathf.Sin(Mathf.Deg2Rad * degrees) + v.y * Mathf.Cos(Mathf.Deg2Rad * degrees)
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "bulletStopper")
        {
            Debug.Log("pee");
            Destroy(gameObject);
        }
    }
}
