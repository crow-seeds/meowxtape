using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    // Start is called before the first frame update
    bool hardMode = false;
    [SerializeField] Transform turret;
    [SerializeField] Transform player;
    bool hasStartedAttack2 = false;

    void Start()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(transform, new Vector3(3, 3, 0), 3);
        GameObject[] soundFx = GameObject.FindGameObjectsWithTag("soundFx");
        sfx = soundFx[0].GetComponent<AudioSource>();

        GameObject[] playerObj = GameObject.FindGameObjectsWithTag("player");
        player = playerObj[0].transform;
    }

    float timer = 0;
    float bMod = 1;
    AudioSource sfx;
    float lastAngle = 0;


    // Update is called once per frame
    void Update()
    {
        Vector3 dir = player.position - transform.position;
        float currentAngle = Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x) + 90;
        //Debug.Log(player.position);
        //Debug.Log(dir);
        turret.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    IEnumerator attack1()
    {
        if (!hardMode)
        {
            yield return new WaitForSeconds(3);
        }
        else
        {
            yield return new WaitForSeconds(1.2f);
        }

        if (!hardMode)
        {
            Instantiate(Resources.Load<GameObject>("Enemies/Ring Cluster"), transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<RingCluster>().init(3f * bMod, Random.Range(8, 16), false, 0);
        }
        else
        {
            Instantiate(Resources.Load<GameObject>("Enemies/Ring Cluster"), transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<RingCluster>().init(3f * bMod, Random.Range(8, 16), false, Random.Range(-25,25));
        }


        //Instantiate(Resources.Load<GameObject>("Prefabs/Rotator")).GetComponent<Rotator>().set(turret, 360, 0.5f);
        sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/gun"), 0.3f);
        StartCoroutine(attack1());
    }

    IEnumerator attack2()
    {
        if (!hasStartedAttack2)
        {
            if (!hardMode)
            {
                yield return new WaitForSeconds(1.5f);
            }
            else
            {
                yield return new WaitForSeconds(0.6f);
            }
        }

        if (!hardMode)
        {
            yield return new WaitForSeconds(2.7f);
        }
        else
        {
            yield return new WaitForSeconds(0.9f);
        }



        //Instantiate(Resources.Load<GameObject>("Prefabs/Rotator")).GetComponent<Rotator>().set(turret, angle - transform.rotation.eulerAngles.z, 0.5f);
        Vector2 dir = Vector3.Normalize(player.position - transform.position);
        Bullet b = Instantiate(Resources.Load<GameObject>("Enemies/Bullet"), new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(0, 0, 0)).GetComponent<Bullet>();
        b.init(6 * bMod, dir, false);
        b.sprite.color = Color.red;

        Bullet b2 = Instantiate(Resources.Load<GameObject>("Enemies/Bullet"), new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(0, 0, 0)).GetComponent<Bullet>();
        b2.init(5.5f * bMod, dir, false);
        b2.sprite.color = Color.red;

        Bullet b3 = Instantiate(Resources.Load<GameObject>("Enemies/Bullet"), new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(0, 0, 0)).GetComponent<Bullet>();
        b3.init(5f * bMod, dir, false);
        b3.sprite.color = Color.red;

        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(turret, turret.localPosition + new Vector3(-dir.normalized.x * .5f, -dir.normalized.y * .5f, 0), 0.25f);
        yield return new WaitForSeconds(.3f);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(turret, turret.localPosition + new Vector3(dir.normalized.x * .5f, dir.normalized.y * .5f, 0), 0.3f);
        sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/gun"), 0.3f);
        StartCoroutine(attack2());
    }

    public void init(float i, bool h)
    {
        bMod = i;
        hardMode = h;
        StartCoroutine(attack1());
        StartCoroutine(attack2());
        if (hardMode)
        {
            //
        }
        
    }
}
