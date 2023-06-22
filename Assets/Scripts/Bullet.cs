using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector2 direction;
    float speed;
    float downSpeed = 4f;
    float timer = 0;

    bool roadGrav = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 10)
        {
            Destroy(gameObject);
        }

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
        Debug.Log(speed * direction + Vector2.down * downSpeed);
    }
}
