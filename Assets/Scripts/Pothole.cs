using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pothole : MonoBehaviour
{
    float speed;
    [SerializeField] SpriteRenderer sp;

    // Start is called before the first frame update
    void Start()
    {
        sp.sprite = Resources.Load<Sprite>("Sprites/pothole" + Random.Range(0,3).ToString());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if(transform.position.y < -6)
        {
            Destroy(gameObject);
        }
    }

    public void init(float sp)
    {
        speed = sp;
    }
}
