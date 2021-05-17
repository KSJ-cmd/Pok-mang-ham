using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;

    public float maxSpeed;
    public float jumpPower;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsulecollider;
    AudioSource audioSource;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsulecollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();

    }


    void Update() {
        // 점프
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);

            //Sound
            PlayerSound("JUMP");
            audioSource.Play();
        }

        // Stop Speed
        if (Input.GetButtonUp("Horizontal")) {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        // 방향 전완
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        // Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);


    }
    void FixedUpdate()
    {
        // Move Speed
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //Max Speed
        if (rigid.velocity.x > maxSpeed) // 오른쪽 스피드
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed*(-1)) // 왼쪽 스피드
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        // Landing Platform
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            if (rayHit.collider != null) {
                if (rayHit.distance < 0.5f)
                    anim.SetBool("isJumping", false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ememy") {

            //Attack
            if(rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y) {

                OnAttack(collision.transform);
            }else //Damage
                OnDamaged(collision.transform.position);
        }   
    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Item") {
            //Point
            bool inBronze = collision.gameObject.name.Contains("Bronze");
            bool inSilver = collision.gameObject.name.Contains("inSilver");
            bool inGold = collision.gameObject.name.Contains("Gold");

            if(inBronze)
                gameManager.stagePoint += 50;
            else if(inSilver)
                gameManager.stagePoint += 100;
            else if(inGold)
                gameManager.stagePoint += 300;

            //Deactive Item
            collision.gameObject.SetActive(false);

            //Sound
            PlayerSound("ITEM");
            audioSource.Play();

        }
        else if(collision.gameObject.tag == "Finish") {
            //Next Stage
            gameManager.NextStage();
            //Sound
            PlayerSound("FINISH");
            audioSource.Play();

        }
    }

    void OnAttack(Transform enemy)
    {
        // Point
        gameManager.stagePoint += 100;

        // Reaction Force
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        //Enemy Die
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
        //Sound
        PlayerSound("ATTACK");
        audioSource.Play();
    }
    
    void OnDamaged(Vector2 targetPos)
    {
        //Health Down
        gameManager.HealthDown();

        // Change Layer
        gameObject.layer = 11;

        //View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7 , ForceMode2D.Impulse);

        //Animation
        anim.SetTrigger("doDamaged");

        Invoke("OffDamaged", 1.5f);
        //Sound
        PlayerSound("DAMAGED");
        audioSource.Play();

    }

    void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        //Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //Sprite Flip Y
        spriteRenderer.flipY = true;

        //Collider Disable
        capsulecollider.enabled = false;

        //Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        //Sound
        PlayerSound("DIE");
        audioSource.Play();
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }

    void PlayerSound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                break;
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                break;
        }
    }
}
