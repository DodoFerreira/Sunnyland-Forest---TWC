using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Secrets : MonoBehaviour
{
    public TilemapRenderer tilemap;

    void Start()
    {
        tilemap = GetComponent<TilemapRenderer>();
    }

    void OnTriggerEnter2D (Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Secrets":
            break;
        }

    }

    void OnTriggerExit2D (Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Secrets":
            tilemap.enabled = true;
            Debug.Log ("Secret true");
            break;
        }
    }

    public void EnterSecrets ()
    {
        tilemap.enabled = false;
    }

    public void ExitSecrets ()
    {
        tilemap.enabled = true;
    }
}
