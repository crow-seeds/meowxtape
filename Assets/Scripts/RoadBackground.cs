using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBackground : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if(transform.position.y < -12.37)
        {
            transform.position = new Vector3(transform.position.x, 0, 20);
        }
    }
}
