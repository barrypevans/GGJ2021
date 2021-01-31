using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bat")
        {
            collision.gameObject.GetComponent<Bat>().Kill();
            Destroy(gameObject);
        } else // make sure bullet can't collide with player/gun?
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bat")
        {
            collision.gameObject.GetComponent<Bat>().Kill();
            Destroy(gameObject);
        } else if (collision.gameObject.tag == "Werewolf")
        {
            collision.gameObject.GetComponent<Werewolf>().Kill();
            Destroy(gameObject);
        }
        else // make sure bullet can't collide with player/gun?
        {
            Destroy(gameObject);
        }
    }
}