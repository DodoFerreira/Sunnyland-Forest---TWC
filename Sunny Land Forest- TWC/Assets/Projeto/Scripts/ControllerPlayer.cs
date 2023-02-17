using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ControllerPlayer : MonoBehaviour
{

        #region         Variaveis
//Player
private Animator        playerAnimator;
private Rigidbody2D     playerRigidbody;
private SpriteRenderer  srPlayer;


//Life
public  int             life = 3;
public  Color           noHitcolor;
public  Color           hitcolor; 
private bool            invenciblePlayer;
public  GameObject      playerDie;

//Movement
public  Transform       groundCheck;
public  bool            isGround = false;
public  float           movSpeed;
public  float           touchRun = 0.0f;
public  float           touchLadder = 0.0f;
public  bool            facingRight = true;
public  bool            ladder = false;
public  bool            isClimb = false;
public  Transform       limitY;
public  bool            buttonW = false;

//Jump
public  bool            jump = false;
public  int             numberJumps = 0;
public  int             maxJumps = 2;
public  float           jumpForce;

//Scripts
private ControllerGame  _ControllerGame;
private IASlug          _IASlug;

//Particles
public  ParticleSystem  _dust;
public  ParticleSystem  _spore;

    #endregion


    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        srPlayer = GetComponent<SpriteRenderer>();

        _ControllerGame = FindObjectOfType(typeof(ControllerGame)) as ControllerGame;
        _IASlug = FindObjectOfType(typeof(IASlug)) as IASlug;
    }

    
    void Update()
    {
        touchRun = Input.GetAxisRaw("Horizontal");
        touchLadder = Input.GetAxisRaw("Vertical");
        

        isGround = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        playerAnimator.SetBool("GroundAnimator", isGround);

        if(Input.GetButtonDown("Jump") && !isClimb)
        {
            jump = true;
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            buttonW = true;
            StartCoroutine ("Delay1s");
        }
        
        SetAnimation();
        LimitUp ();
    }

    void FixedUpdate() 
    {
    
        MovePlayer();
        MoveLadder();

        if (jump) {
            JumpPlayer();
        }

        if(playerRigidbody.velocity.y !=0 && ladder)
        {
            isClimb = true;
        }

        if(isGround)
        {
            numberJumps = 1;
        }

    }

    void SetAnimation () 
    {
        playerAnimator.SetBool ("Run", playerRigidbody.velocity.x !=0 && isGround);
        playerAnimator.SetBool ("Jump", !isGround);
        playerAnimator.SetBool ("Climb", playerRigidbody.velocity.y !=0 && isClimb);
    }  


        #region         MOVEMENTS


    void MovePlayer(float movH = 0f) 
    {
        movH = touchRun;
        playerRigidbody.velocity = new Vector2 (movH * movSpeed, playerRigidbody.velocity.y);

        if (movH < 0 && facingRight || (movH > 0 && !facingRight))
        {
            FlipPlayer();
        }
    }

    void MoveLadder(float movLadder = 0f)
    {
        movLadder = touchLadder;

        if (ladder)
        {
        playerRigidbody.gravityScale = 0;
        playerRigidbody.velocity = new Vector2 (playerRigidbody.velocity.x, movLadder * movSpeed);
        }
    }

    void FlipPlayer () 
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        if(isGround)
        {
            Dust();
        }
    }

  
    void JumpPlayer () 
    {
        if(isGround && !isClimb) 
        {
            numberJumps = 0;
            Dust();
        }

        if (isGround || numberJumps < maxJumps)
        {
            _ControllerGame.JumpSound ();
            playerRigidbody.velocity = new Vector2 (playerRigidbody.velocity.x, 1 * jumpForce);
           
            //playerRigidbody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse); -->Alteracao no sistema de pulo - Troca da vorça por velocidade no eixo y;

            isGround = false;
            numberJumps ++;
        }
            jump = false;
    }

    void LimitUp ()
    {
        float positionY = transform.position.y;

        if (positionY > limitY.position.y)
        {
            transform.position = new Vector2 (transform.position.x, limitY.position.y);
            playerRigidbody.velocity = new Vector2 (playerRigidbody.velocity.x, 0);
        }
    }

        #endregion

        #region         COLLISION  & COLLIDERS

    void OnTriggerEnter2D (Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Carrot":

                _ControllerGame.Point (1);            

                Destroy(collision.gameObject);
                break;

            case "Enemy":
                
                invenciblePlayer = true;
                Rigidbody2D rbJump = GetComponentInParent<Rigidbody2D>();
                rbJump.velocity = new Vector2 (rbJump.velocity.x, 0);
                rbJump.AddForce (new Vector2(0, 600));
                StartCoroutine ("PreventDamage");
                numberJumps = 1;
                break;

            case "Spike":
                Hurt ();
                break;

            case "Ladder":
                ladder = true;
                break;

            case "Secrets":
                
                _ControllerGame.EnterSecrets ();
                break;

            case "Fall":
                _ControllerGame.FallSound ();
                StartCoroutine ("FallDie");
                break;

                
        }
    }

    void OnTriggerStay2D (Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Exit":
                if (buttonW)
                {
                    buttonW = false;
                    _ControllerGame.ExitLevel ();

                }
                break;

            case "Message":
                _ControllerGame.MessageEnter ();
                break;
        }
    }

    void OnTriggerExit2D (Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {   
            case "Message":
                _ControllerGame.MessageExit ();
                break;

            case "Ladder":
                isClimb = false;
                ladder = false;
                playerRigidbody.gravityScale = 3;
                break;

            case "Secrets":
                _ControllerGame.ExitSecrets ();
                break;
        }
    }

    void OnCollisionEnter2D (Collision2D collision )
    {
        switch(collision.gameObject.tag)
        {
            case "Enemy":
                Hurt ();
                break;

            case "Platform":
                this.transform.parent = collision.transform;
                break;

            case "Mushroom":
                _ControllerGame.MushSound();
                Spore();
                numberJumps =1;
                break;
        }

    }

    void OnCollisionExit2D (Collision2D collision )
    {
        switch(collision.gameObject.tag)
        {
            case "Platform":
                this.transform.parent = null;
                break;
        }
    }

        #endregion

        #region         DAMAGE

    void Hurt ()
    {
        if (!invenciblePlayer) 
        {
            invenciblePlayer = true;
            life -= 1;
            StartCoroutine ("Damage");
            _ControllerGame.LifeBar(life);

            if (life <1)
            {
                Die ();
            }
        }
    }

    void Restart ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Die ()
    {
        GameObject playerDieTemp = Instantiate(playerDie, transform.position, Quaternion.identity);
        Rigidbody2D rbDie = playerDieTemp.GetComponent<Rigidbody2D>();
        rbDie.AddForce(new Vector2(150f, 400f));
        _ControllerGame.DeathSound ();
                
        Invoke ("Restart", 3f);
        gameObject.SetActive(false);
    }

        #endregion

        #region         PARTICLE SYSTEM

    void Dust ()
    {
        _dust.Play();
    }

    void Spore ()
    {
        if (!isGround)
        {
        _spore.Play();
        }
    }
        #endregion

        #region         COROUTINES

    IEnumerator Damage ()
    {
        srPlayer.color = noHitcolor;
        yield return new WaitForSeconds (0.1f);

        for (float i=0; i < 1; i+= 0.1f)
        {
            srPlayer.enabled = false;
            yield return new WaitForSeconds (0.1f);
            srPlayer.enabled = true;
            yield return new WaitForSeconds (0.1f);
        }
        
        invenciblePlayer = false;
        srPlayer.color = Color.white;
    
    }

    IEnumerator Delay1s ()
    {
        yield return new WaitForSeconds (1f);
        buttonW = false;
    }

    IEnumerator FallDie ()
    {
                
        yield return new WaitForSeconds (0.3f);            
        GameObject playerDieTemp = Instantiate(playerDie, transform.position, Quaternion.identity);
        Rigidbody2D rbDie = playerDieTemp.GetComponent<Rigidbody2D>();
        rbDie.AddForce(new Vector2(150f, 400f));
        _ControllerGame.DeathSound ();
                
        Invoke ("Restart", 3f);
        gameObject.SetActive(false);
    }

    IEnumerator PreventDamage ()
    {
        yield return new WaitForSeconds (0.05f); 
        invenciblePlayer = false;
    }

    #endregion
}
