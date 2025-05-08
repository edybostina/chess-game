using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using DG.Tweening;
using UnityEngine.UIElements;

public class StreetViewCamera : MonoBehaviour
{
    public float speed = 5f;
    public float maxMovementDistance = 20f;
    public float movementDistance = 7f;

    private Quaternion originalRotation;
    private Vector3 originalPosition;

    public Transform target;
    public Vector3 targetV3 = new Vector3(0,0,0);

    private bool isMoving;
    public BlurBackground blur;
    private void Start()
    {
        blur.Unblur();
        originalRotation = transform.rotation;
        originalPosition = new Vector3(0f, 12f, -15f) ;
    }

    private void Update()
    {
        if (!PauseMenu.quitGame)
        {
            HandleRotationInput();
            HandleMovementInput();
            HandleDragInput();
        }
    }
  
    private void HandleRotationInput()
    {

        
        
            transform.DOLookAt(targetV3,0.5f);

        
    }

    private void HandleMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        
        
        //if (horizontal == 0 && vertical == 0)
        //{
        //    transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * speed);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * speed);
        //}


        if (vertical > 0)
        {
            if (vertical > 0 && horizontal > 0)
            {
                Vector3 newPosition = originalPosition + Vector3.right * movementDistance + Vector3.forward * movementDistance * 1.5f + Vector3.up * 2f;
                transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
            }
            else if (vertical > 0 && horizontal < 0)
            {
                Vector3 newPosition = originalPosition + Vector3.left * movementDistance + Vector3.forward * movementDistance * 1.5f + Vector3.up * 2f;
                transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    Vector3 newPosition = originalPosition + Vector3.forward * movementDistance * 2.142f + Vector3.up * 2f;
                    transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
                } 
                else 
                {
                    Vector3 newPosition = originalPosition + Vector3.up * 2f + Vector3.forward * movementDistance * 1.5f;
                    transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
                }
               
            }


        }



        if (horizontal > 0 && vertical == 0)
        {
            Vector3 newPosition = originalPosition + Vector3.right * movementDistance + Vector3.forward * movementDistance;
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);

        }
        if (horizontal < 0 && vertical == 0)
        {
            Vector3 newPosition = originalPosition + Vector3.left * movementDistance + Vector3.forward * movementDistance;
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);

        }


        if (vertical < 0)
        {

            if (horizontal > 0)
            {


                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    Vector3 newPosition = originalPosition + Vector3.back * movementDistance * 2f + Vector3.right * movementDistance * 3f + Vector3.forward * movementDistance * 3f;
                    transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
                }
                else
                {
                    Vector3 newPosition = originalPosition + Vector3.down * 7f + Vector3.right * movementDistance + Vector3.forward * movementDistance;
                    transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
                }
            }
            else if (horizontal < 0)
            {

                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    Vector3 newPosition = originalPosition + Vector3.back * movementDistance * 2f + Vector3.left * movementDistance * 3f + Vector3.forward * movementDistance * 3f;
                    transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
                }
                else
                {
                    Vector3 newPosition = originalPosition + Vector3.down * 7f + Vector3.left * movementDistance + Vector3.forward * movementDistance;
                    transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
                }
            }

            else
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    Vector3 newPosition = originalPosition + Vector3.back * movementDistance * 2f;
                    transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
                }
                else
                {
                    Vector3 newPosition = originalPosition + Vector3.down * 7f;
                    transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
                }
            }

        }

        
    }

    Vector3 clickPosition;
    Vector3 releasePosition;
    bool down = false;
    bool up = false;
    bool left = false;
    bool right = false;

  

    private void HandleDragInput()
    {
        if (Input.GetMouseButtonDown(0))
        {

            clickPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {

            releasePosition = Input.mousePosition;



            if (Mathf.Abs(clickPosition.y - releasePosition.y) > 300 && Mathf.Abs(clickPosition.x - releasePosition.x) < 300)
            {
                //Debug.Log("Mouse Click Position: " + clickPosition);
                //Debug.Log("Mouse Release Position: " + releasePosition);


                if (clickPosition.y > releasePosition.y)
                {
                    if (down)
                    {
                        ResetData();
                    }
                    else
                    {
                        Vector3 newPosition = originalPosition + Vector3.up * 2f + Vector3.forward * movementDistance * 1.5f;
                        transform.DOMove(newPosition, 0.5f);
                        up = true;
                    }
                }
                else if (clickPosition.y < releasePosition.y)
                {
                    if (up)
                    {
                        ResetData();
                    }
                    else
                    {
                        Vector3 newPosition = originalPosition + Vector3.down * 7f;
                        transform.DOMove(newPosition, 0.5f);
                        down = true;
                    }
                }
            }

            if (Mathf.Abs(clickPosition.x - releasePosition.x) > 300 && Mathf.Abs(clickPosition.y - releasePosition.y) < 300)
            {
                if (clickPosition.x > releasePosition.x)
                {
                    if (left)
                    {
                        ResetData();
                    }
                    else
                    {
                        Vector3 newPosition = originalPosition + Vector3.right * movementDistance + Vector3.forward * movementDistance;
                        transform.DOMove(newPosition, 0.5f);
                        right = true;
                    }
                }
                else if (clickPosition.x < releasePosition.x)
                {
                    if (right)
                    {
                        ResetData();
                    }
                    else
                    {
                        Vector3 newPosition = originalPosition + Vector3.left * movementDistance + Vector3.forward * movementDistance;
                        transform.DOMove(newPosition, 0.5f);
                        left = true;
                    }
                }
            }

            if (Mathf.Abs(clickPosition.x - releasePosition.x) > 300 && Mathf.Abs(clickPosition.y - releasePosition.y) > 300)
            {
                if (clickPosition.y > releasePosition.y)
                {
                    if (clickPosition.x < releasePosition.x)
                    {
                        if (down || right)
                        {
                            ResetData();
                        }
                        else
                        {
                            Vector3 newPosition = originalPosition + Vector3.left * movementDistance + Vector3.forward * movementDistance * 1.5f + Vector3.up * 2f;
                            transform.DOMove(newPosition, 0.5f);
                            left = true;
                            up = true;

                        }
                    }
                    else if (clickPosition.x > releasePosition.x)
                    {
                        if (down || left)
                        {
                            ResetData();
                        }
                        else
                        {
                            Vector3 newPosition = originalPosition + Vector3.right * movementDistance + Vector3.forward * movementDistance * 1.5f + Vector3.up * 2f;
                            transform.DOMove(newPosition, 0.5f);
                            right = true;
                            up = true;

                        }
                    }
                }
                else if (clickPosition.y < releasePosition.y)
                {
                    if (clickPosition.x < releasePosition.x)
                    {
                        if (up || right)
                        {
                            ResetData();
                        }
                        else
                        {
                            Vector3 newPosition = originalPosition + Vector3.down * 7f + Vector3.left * movementDistance + Vector3.forward * movementDistance;
                            transform.DOMove(newPosition, 0.5f);
                            down = true;
                            left = true;
                        }
                    }
                    else if (clickPosition.x > releasePosition.x)
                    {
                        if (up || left)
                        {
                            ResetData();
                        }
                        else
                        {
                            Vector3 newPosition = originalPosition + Vector3.down * 7f + Vector3.right * movementDistance + Vector3.forward * movementDistance;
                            transform.DOMove(newPosition, 0.5f);
                            down = true;
                            right = true;
                        }
                    }
                }



            }




        }
    }


    public void ResetData()
    {
        transform.DOMove(originalPosition, 0.5f);
        down = false;
        up = false;
        left = false;
        right = false;
    }



    public void UpdateOriginalRotation()
    {
        originalRotation = transform.rotation * Quaternion.Euler(70, 180, 0);


        originalPosition = new Vector3(
            originalPosition.x,
            originalPosition.y,
            -originalPosition.z
        );

        movementDistance = -movementDistance;
    }

}

