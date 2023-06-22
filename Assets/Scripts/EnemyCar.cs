using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCar : MonoBehaviour
{
    float timer = 0;
    float speed = 0;
    float timeToStop = 1;
    string type = "regular";

    [SerializeField] float acceleration;
    [SerializeField] float deceleration;

    [SerializeField] Rigidbody2D rig;
    [SerializeField] SpriteRenderer sprite;
    BulletManager bullm;

    Transform playerPos;
    AudioSource sfx;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("player").transform;
        GameObject[] soundFx = GameObject.FindGameObjectsWithTag("soundFx");
        sfx = soundFx[0].GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    bool hasStopped = false;
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if((timer > timeToStop || detectCar(0.25f) || detectCar(-0.25f) || (transform.position.y > 2.5f && type == "aim")) && !hasStopped && transform.position.y > 0)
        {
            speed = Random.Range(-7f, -3f);
            if(type == "aim")
            {
                StartCoroutine(aim());
            }

            hasStopped = true;
        }

        float targetSpeedY = speed;
        targetSpeedY = Mathf.Lerp(rig.velocity.y, targetSpeedY, 1);
        float accelRateY = (Mathf.Abs(targetSpeedY) > 0.01f) ? acceleration : deceleration;
        float speedDifY = targetSpeedY - rig.velocity.y;
        float movementY = speedDifY * accelRateY;
        rig.AddForce(movementY * Vector2.up, ForceMode2D.Force);

        if(transform.position.y > 7 || transform.position.y < -7)
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] LayerMask carLayer;

    bool detectCar(float p)
    {
        RaycastHit2D hit;

        hit = Physics2D.Raycast(transform.position + Vector3.up*0.8f - Vector3.right*p, Vector2.up, 3f, carLayer);
        Debug.DrawRay(transform.position + Vector3.up * 0.8f - Vector3.right * p, Vector3.up * 3f, Color.green);

        if(hit.collider == true)
        {
            Debug.Log("car!");
        }
        return (hit.collider == true);
    }


    public void init(float sp, float tS, string ty, float bSpeed, BulletManager bm)
    {
        speed = sp;
        timeToStop = tS;
        type = ty;
        bulletSpeedModifier = bSpeed;
        bullm = bm;
        switch (type)
        {
            case "aim":
                GetComponent<SpriteRenderer>().color = new Color(1, 0.6f, 0.6f);
                sprite.sprite = Resources.Load<Sprite>("Sprites/enemycarcop");
                break;
            default:
                sprite.sprite = Resources.Load<Sprite>("Sprites/enemycar" + Random.Range(0,4).ToString());
                break;
        }
    }

    float bulletSpeedModifier = 1;
    IEnumerator aim()
    {
        yield return new WaitForSeconds(1f);
        //Debug.Log("shooting!!");
        Vector2 dist = playerPos.position - transform.position;
        sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/gun"), 0.3f);
        Instantiate(Resources.Load<GameObject>("Enemies/Bullet"), new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(0, 0, 0)).GetComponent<Bullet>().init(4 * bulletSpeedModifier, dist, false);
        StartCoroutine(aim());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "carkill")
        {
            bullm.playScream();
        }
    }
}
