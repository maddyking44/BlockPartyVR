using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    void Start()
    {

    }

    public float Speed = 10;
    public Camera Camera;

    bool movingForward = false;
    bool movingBackward = false;
    bool movingLeft = false;
    bool movingRight = false;
    bool movingUp = false;
    bool movingDown = false;

    void CheckInputState()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            movingForward = true;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            movingForward = false;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            movingBackward = true;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            movingBackward = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movingLeft = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            movingLeft = false;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            movingRight = true;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            movingRight = false;
        }

        if (Input.GetKeyDown("s"))
        {
            movingDown = true;
        }

        if (Input.GetKeyUp("s"))
        {
            movingDown = false;
        }

        if (Input.GetKeyDown("w"))
        {
            movingUp = true;
        }

        if (Input.GetKeyUp("w"))
        {
            movingUp = false;
        }
    }

    void Update()
    {
        CheckInputState();
        Vector3 forwardVector = Camera.transform.forward;
        Vector3 rightVector = Camera.transform.right;
        Vector3 upVector = Camera.transform.up;

        if (movingForward)
        {
            transform.position += forwardVector * Speed * Time.deltaTime;
        }
		else if (movingBackward)
        {
            transform.position += forwardVector * Speed * Time.deltaTime * -1.0f;
        }

        if (movingLeft)
        {
            transform.Translate(rightVector * Speed * Time.deltaTime * -1.0f, Space.Self);
        }
		else if (movingRight)
        {
            transform.Translate(rightVector * Speed * Time.deltaTime, Space.Self);
        }

        if (movingUp)
        {
            transform.Translate(upVector * Speed * Time.deltaTime, Space.Self);
        }
		else if (movingDown)
        {
            transform.Translate(upVector * Speed * Time.deltaTime * -1.0f, Space.Self);
        }
    }
}
