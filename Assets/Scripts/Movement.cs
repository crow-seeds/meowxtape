using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Rigidbody2D rig;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] float velPower;

    bool canDash = true;
    [SerializeField] float dashTime = 0.2f;
    [SerializeField] public float dashCooldownTime = 2f;

    [SerializeField] TrailRenderer tr;

    [SerializeField] SpriteRenderer sr;
    [SerializeField] SpriteRenderer srOfPlayer;
    [SerializeField] GameObject playerHitbox;
    [SerializeField] GameObject playerHurtbox;

    [SerializeField] AudioSource sfx;

    float dashTimer = 0;
    public bool canMove = false;

    float allTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool inDash = false;
    [SerializeField] AudioClip tire;

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Gamepad.current.buttonSouth.wasPressedThisFrame || Gamepad.current.buttonEast.wasPressedThisFrame) && canDash && canMove)
        {
            Debug.Log("dashing!");
            speed *= 3;
            sfx.PlayOneShot(tire, 1);
            canDash = false;
            dashTimer = 0;
            inDash = true;
            sr.material.SetFloat("_Arc1", 0f);
            tr.emitting = true;
            playerHitbox.SetActive(false);
            playerHurtbox.SetActive(true);
            StartCoroutine(dashCooldown());
        }

        allTimer += Time.deltaTime * 6;

        if (!playerHitbox.activeSelf)
        {
            if ((int)allTimer % 2 == 0)
            {
                srOfPlayer.color = new Color(srOfPlayer.color.r, srOfPlayer.color.g, srOfPlayer.color.b, 0.5f);
            }
            else
            {
                srOfPlayer.color = new Color(srOfPlayer.color.r, srOfPlayer.color.g, srOfPlayer.color.b, 1f);
            }

        }



        if (!canDash)
        {
            dashTimer += Time.deltaTime;
            sr.material.SetFloat("_Arc1", (dashTimer/dashCooldownTime) * 360f);
            if (dashTimer >= dashCooldownTime)
            {
                canDash = true;
                sr.material.SetFloat("_Arc1", 360f);
            }
        }

        mov = Vector2.zero;
        if (canMove)
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Gamepad.current.dpad.down.isPressed)
            {
                mov += Vector2.down;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Gamepad.current.dpad.up.isPressed)
            {
                mov += Vector2.up;
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Gamepad.current.dpad.left.isPressed)
            {
                mov += Vector2.left;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Gamepad.current.dpad.right.isPressed)
            {
                mov += Vector2.right;
            }

            if(Gamepad.current.leftStick.ReadValue().magnitude > 0.1f)
            {
                mov = Gamepad.current.leftStick.ReadValue();
            }
        }


    }

    Vector2 mov = Vector2.zero;

    // Update is called once per frame
    void FixedUpdate()
    {
        mov = Vector2.ClampMagnitude(mov, 1) * speed;

        float targetSpeedX = mov.x;
        targetSpeedX = Mathf.Lerp(rig.velocity.x, targetSpeedX, 1);
        float accelRateX = (Mathf.Abs(targetSpeedX) > 0.01f) ? acceleration : deceleration;
        float speedDifX = targetSpeedX - rig.velocity.x;
        float movementX = speedDifX * accelRateX;
        rig.AddForce(movementX * Vector2.right, ForceMode2D.Force);

        float targetSpeedY = mov.y;
        targetSpeedY = Mathf.Lerp(rig.velocity.y, targetSpeedY, 1);
        float accelRateY = (Mathf.Abs(targetSpeedY) > 0.01f) ? acceleration : deceleration;
        float speedDifY = targetSpeedY - rig.velocity.y;
        float movementY = speedDifY * accelRateY;
        rig.AddForce(movementY * Vector2.up, ForceMode2D.Force);


        if (canMove)
        {
            if (transform.position.x < -1.5f)
            {
                transform.position = new Vector3(-1.5f, transform.position.y, transform.position.z);
            }

            if (transform.position.x > 7)
            {
                transform.position = new Vector3(7f, transform.position.y, transform.position.z);
            }

            if (transform.position.y > 4.5f)
            {
                transform.position = new Vector3(transform.position.x, 4.5f, transform.position.z);
            }

            if (transform.position.y < -4.5f)
            {
                transform.position = new Vector3(transform.position.x, -4.5f, transform.position.z);
            }
        }
        
    }

    IEnumerator dashCooldown()
    {
        yield return new WaitForSeconds(dashTime);
        speed /= 3;
        inDash = false;
        tr.emitting = false;
        yield return new WaitForSeconds(dashTime);
        playerHitbox.SetActive(true);
        playerHurtbox.SetActive(false);
        srOfPlayer.color = new Color(srOfPlayer.color.r, srOfPlayer.color.g, srOfPlayer.color.b, 1f);

    }

    public void toggleHitbox(bool on)
    {
        playerHitbox.SetActive(on);
        srOfPlayer.color = new Color(srOfPlayer.color.r, srOfPlayer.color.g, srOfPlayer.color.b, 1f);
    }

    public float getSpeed()
    {
        return speed;
    }

    public void setSpeed(float s)
    {
        speed = s;
    }

    float iFrameTime = 1f;
    public void takeDamage()
    {
        playerHitbox.SetActive(false);
        playerHurtbox.SetActive(true);
        StartCoroutine(takeDamageCoroutine());
    }

    IEnumerator takeDamageCoroutine()
    {
        yield return new WaitForSeconds(iFrameTime);
        if (!inDash)
        {
            playerHitbox.SetActive(true);
            playerHurtbox.SetActive(false);
            srOfPlayer.color = new Color(srOfPlayer.color.r, srOfPlayer.color.g, srOfPlayer.color.b, 1f);
        }      
    }
}
