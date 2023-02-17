using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IASlug : MonoBehaviour
{

public  Transform       enemy;
public  SpriteRenderer  enemySprite;
public  Transform[]     position;
public  float           speed;
public  float           maxSpeed;
public  bool            isRight;
private int             idTarget;

public  int             lifeIA;
public  int             rage = 1;
public  bool            invencibleIA = false;
public  Color           rageColor;

private ControllerGame  _ControllerGame;


    
    void Start()
    {
        enemySprite = enemy.gameObject.GetComponent<SpriteRenderer>();
        enemy.position = position[0].position;
        idTarget = 1;
        
        _ControllerGame = FindObjectOfType(typeof(ControllerGame)) as ControllerGame;
    }

    void Update()
    {
       

        if (enemy != null)
        {
            enemy.position = Vector3.MoveTowards(enemy.position, position[idTarget].position, (speed*rage) * Time.deltaTime);

            if (enemy.position == position[idTarget].position)
                {
                    idTarget += 1;
                    if (idTarget == position.Length)
                    {
                        idTarget = 0;
                    }
                }

            if (position[idTarget].position.x < enemy.position.x && isRight == true)
                {
                    Flip();
                }
            else if (position[idTarget].position.x > enemy.position.x && isRight == false)
                {
                    Flip();
                } 

        }
    }

    void OnTriggerEnter2D (Collider2D collision)
    {
         switch (collision.gameObject.tag)
        {
            case "Player":
                DamageIA();
                break;
        }
    }


    void Flip ()
    {
        isRight = !isRight;
        enemySprite.flipX = !enemySprite.flipX;
    }

    public void DamageIA()
    {
        lifeIA -= 1;
        

        if (lifeIA == 1)
        {
            StartCoroutine ("InRage");
            
                        
        }

        if (lifeIA ==0)
        {
        GameObject explosionTemp = Instantiate (_ControllerGame.hitPrefab, enemy.position, transform.localRotation);
        Destroy(explosionTemp, 0.5f);

                
        _ControllerGame.ExplosionSound();
            Destroy(this.gameObject);
        }
    }

    IEnumerator InRage ()
    {
        enemySprite.color = rageColor;
        speed = 0;
        _ControllerGame.AngrySound();
        
        

        for (float i=0; i < 0.3; i+= 0.1f)
        {
            enemySprite.enabled = false;
            yield return new WaitForSeconds (0.1f);
            enemySprite.enabled = true;
            yield return new WaitForSeconds (0.1f);
        }
        
        invencibleIA = false;
        enemySprite.color = rageColor;
        speed = maxSpeed;
        rage +=2;
    }

}
