using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;

    private Vector2 movement;
    private float xAxis, yAxis;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Movement(float xAxis, float speed) {
        movement = new Vector2(xAxis, 0f);
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
