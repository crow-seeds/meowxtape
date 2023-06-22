using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(transform, new Vector3(3, 3, 0), 3);
        GameObject[] soundFx = GameObject.FindGameObjectsWithTag("soundFx");
        sfx = soundFx[0].GetComponent<AudioSource>();
    }

    float timer = 0;
    float bMod = 1;
    AudioSource sfx;


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 3f)
        {
            Instantiate(Resources.Load<GameObject>("Enemies/Ring Cluster"), transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<RingCluster>().init(3f * bMod, Random.Range(8,16), false);
            timer = 0;
            sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/gun"), 0.3f);
        }
        

    }

    public void init(float i)
    {
        bMod = i;
    }
}
