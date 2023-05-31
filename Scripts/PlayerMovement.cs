using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public WarriorMove _moveP;
    public Animator animator;
    float horMove = 0f;
    public float speed = 70f;
    bool jump = false;
    bool crouch = false;

    void Update()
    {
        horMove = Input.GetAxisRaw("Horizontal") * speed;

        animator.SetFloat("Speed", Mathf.Abs(horMove));

        if(Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        if(Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch"))
        {
           crouch = false;
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void OnCrouching(bool IsCrouching) 
    {
        animator.SetBool("IsCrouching", IsCrouching);
    }

    void FixedUpdate()
    {
        // Move our character
        _moveP.Move(horMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    
}

