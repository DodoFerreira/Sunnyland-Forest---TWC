using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

using UnityEngine.Tilemaps;


public class ControllerGame : MonoBehaviour
{
    private int             score;
    public  int             maxScore;
    public  Text            txtScore;
    public  GameObject      hitPrefab;

    public  Sprite[]        imgLife;
    public  Image           Life;

    public  AudioSource     fxGame;
    public  AudioClip       fxPoint;
    public  AudioClip       fxExplosion;
    public  AudioClip       fxJump;
    public  AudioClip       fxDeath;
    public  AudioClip       fxFall;
    public  AudioClip       fxMush;
    public  AudioClip       fxAngry;

    public  TilemapRenderer tileSecrets;

    public  Canvas          message;

    public  Text            messageNextLevel;


    void Awake() 
    {
        tileSecrets.enabled = true;
    }


    public void Point ( int carrot )
    {
        score += carrot;
        txtScore.text = score.ToString();
        fxGame.PlayOneShot(fxPoint);

    
    }

    public void LifeBar ( int health )
    {
        Life.sprite = imgLife [health];
    }

    public void JumpSound ()
    {
        fxGame.PlayOneShot(fxJump);
    }

    public void DeathSound ()
    {
        fxGame.PlayOneShot(fxDeath);
    }
    
    public void ExplosionSound ()
    {
        fxGame.PlayOneShot(fxExplosion);
    }

    public void FallSound()
    {
        fxGame.PlayOneShot(fxFall);
    }

    public void MushSound()
    {
        fxGame.PlayOneShot(fxMush);
    }

    public void AngrySound()
    {
        fxGame.PlayOneShot(fxAngry);
    }


    public void EnterSecrets ()
    {
        tileSecrets.enabled = false;
    }

    public void ExitSecrets ()
    {
        tileSecrets.enabled = true;
    }

    public void ExitLevel ()
    {
        if (score == maxScore)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

        else
            {
                messageNextLevel.enabled = true;
                StartCoroutine ("Delay5s");
            }

    }

    public void MessageEnter ()
    {
        message.enabled = true;
    }

    public void MessageExit ()
    {
        message.enabled = false;
    }

    IEnumerator Delay5s ()
    {
        yield return new WaitForSeconds (5f);
        messageNextLevel.enabled = false;
    }
    
}
