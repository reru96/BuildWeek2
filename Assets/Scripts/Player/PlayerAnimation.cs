using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private RenatoPlayerController player;
    private CharacterController controller;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<RenatoPlayerController>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (player == null) return;

        animator.SetFloat("Speed", player.ForwardSpeed);

      
        animator.SetBool("isJumping", !controller.isGrounded);

        animator.SetBool("isSliding", player.IsSliding);
    }
}
