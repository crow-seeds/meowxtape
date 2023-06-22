using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingCluster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init(float sp, int amt, bool dG)
    {
        float ang = 0;
        for(int i = 0; i < amt; i++)
        {
            Instantiate(Resources.Load<GameObject>("Enemies/Bullet"), new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(0, 0, 0)).GetComponent<Bullet>().init(sp, new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)), dG);
            ang += 2 * 3.1415f / amt;
        }
        Destroy(gameObject);
    }
}
