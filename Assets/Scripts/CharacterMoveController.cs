﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    private Animator anim;
    private CharacterSoundController sound;

    [Header("Movement")]
    public float moveAccel;
    public float maxSpeed;

    private Rigidbody2D rig;

    [Header("Jump")]
    public float jumpAccel;
    private bool isJumping;
    private bool isOnGround;

    [Header("Ground Raycast")]
    public float groundRaycastDistance;
    public LayerMask groundLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sound = GetComponent<CharacterSoundController>();
    }

    // Update is called once per frame
    void Update()
    {
        //read input
        if(Input.GetMouseButtonDown(0)){
            if(isOnGround){ //if the character is standing on ground then it can jump
                isJumping = true;
                sound.PlayJump();
            }
        }
        //change animation 
        anim.SetBool("isOnGround", isOnGround);
    }

    private void FixedUpdate()
    {
        Vector2 velocityVector = rig.velocity; //speed of object (character)
       

        //raycast ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);
        if(hit)
        {
            if(!isOnGround && rig.velocity.y <= 0) //velocity upwards is less than or equal to 0 and not on ground
            {
                isOnGround = true;
            }
        }
            else 
            {
                isOnGround = false;
            }

        //calculate velocity vector
        if(isJumping)
        {
            velocityVector.y += jumpAccel;
            isJumping = false;
        }

        velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);
        rig.velocity = velocityVector;
        
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);
    }
}
