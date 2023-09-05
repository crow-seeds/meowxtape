using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSprite : MonoBehaviour
{
    [SerializeField] List<Sprite> yarnBalls;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = yarnBalls[Random.Range(0, 4)];
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.back * Time.deltaTime * 360);
    }
}
