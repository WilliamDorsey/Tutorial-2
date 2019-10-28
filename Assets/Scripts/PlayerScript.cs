using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rb2d;
    private GameObject[] enemies;
    Animator anim;
    public AudioSource musicSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;

    public float speed;
    public Text score;
    public Text winLose;
    public Text lives;
    private int scoreValue = 0;
    private int lifeValue = 3;
    private int jumpTimer = 0;
    public int jumpDelay = 0;
    public float jumpForce = 5;
    private bool leftDirection = false;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        score.text = $"Score: {scoreValue}";
        winLose.text = "";
        musicSource.clip = musicClipOne;
        musicSource.Play();
    }

    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        float vel = rb2d.velocity.magnitude;

        rb2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        
        if (vel == 0)
        {
            anim.SetInteger("State", 0);
        }
        else
        {
            anim.SetInteger("State", 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue++;
            score.text = $"Score: {scoreValue}";

            if (scoreValue == 4)
            {
                transform.position = new Vector3(4, -1, 0);
                rb2d.velocity = Vector3.zero;
                lifeValue = 3;
                lives.text = "Lives: 3";
            }

            if (scoreValue == 8)
            {
                winLose.text = "You win!\nGame created by William Dorsey";
                enemies = GameObject.FindGameObjectsWithTag("Enemy");
                musicSource.clip = musicClipTwo;
                musicSource.Play();
                for (var i = 0; i < enemies.Length; i++)
                    Destroy(enemies[i]);
            }

            if (scoreValue == 11)
                winLose.text = "Special Win!\nYou collected all coins!";

            Destroy(collision.collider.gameObject);
        }

        if(collision.collider.tag == "Enemy")
        {
            lifeValue--;
            lives.text = $"Lives: {lifeValue}";

            if(lifeValue == 0)
            {
                winLose.text = "You Lose";
                winLose.color = Color.red;

                rb2d.velocity = Vector3.zero;
                rb2d.isKinematic = true;
            }

            Destroy(collision.collider.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && (jumpTimer == 0))
            {
                rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                jumpTimer = jumpDelay;
                anim.SetInteger("State", 2);
            }
        }
    }

    void OnCollisionExit()
    {
        anim.SetInteger("State", 2);
    }

    private void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (jumpTimer > 0)
        {
            jumpTimer--;
        }
    }

    private void LateUpdate()
    {
        if((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && leftDirection == false)
        {
            Flip();
        }

        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && leftDirection == true)
        {
            Flip();
        }
    }

    void Flip()
    {
        leftDirection = !leftDirection;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
