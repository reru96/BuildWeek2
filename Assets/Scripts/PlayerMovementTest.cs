using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTest : MonoBehaviour
{
    public float speed = 5f;
    public float doubleTapTime = 0.3f;

    private float lastTapTime = 0f;
    private bool moveForward = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Time.time - lastTapTime < doubleTapTime)
            {
                moveForward = true;
            }

            lastTapTime = Time.time;
        }

        if (moveForward)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
