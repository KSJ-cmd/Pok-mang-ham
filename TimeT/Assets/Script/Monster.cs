using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float speed = 10f;
    private BoxCollider2D MonsterCollider;
    private Rigidbody2D MonsterRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        MonsterCollider = GetComponent<BoxCollider2D>();
        MonsterRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MonsterRigidbody.velocity=new Vector2(speed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag=="Platform")
        {
            Debug.Log("Trigger");
            transform.position += new Vector3(0,0.1f,0);
        }
    }
}

