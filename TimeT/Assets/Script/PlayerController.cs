using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D Rigidbody2D;
    public float playerJumpSpeed = 500;
    public float playerMoveSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (Input.GetButtonDown("Horizontal"))
        {
            Move();
        }
    }
    void Jump()
    {
        Vector2 vector2 = new Vector2(0, playerJumpSpeed);
        Rigidbody2D.transform.Translate(vector2);
        Debug.Log("Jump");
    }
    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        Vector2 vector2 = new Vector2(h*playerMoveSpeed,0);
        transform.Translate(vector2); 
        Debug.Log("Move");
    }
}
