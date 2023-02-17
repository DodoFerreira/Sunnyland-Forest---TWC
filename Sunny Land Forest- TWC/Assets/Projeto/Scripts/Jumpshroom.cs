using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpshroom : MonoBehaviour
{
    public ParticleSystem _spore;


    void OnCollisionEnter2D (Collision2D collision )
    {
        switch(collision.gameObject.tag)
        {
            case "Player":
            _spore.Play();
            break;
        }
    }
}
