using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    [SerializeField] float playerSpeed = 4;
    Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float movement = Input.GetAxis("Horizontal") * playerSpeed ;
        Debug.Log(movement);
        var playerMovement = new Vector2(movement, 0);
        rb.velocity = playerMovement;

    }
}
