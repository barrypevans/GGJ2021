using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - Time.deltaTime * 2f);
    }
}