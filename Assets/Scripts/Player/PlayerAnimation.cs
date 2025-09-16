using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        // Aggiorna l'Animator in base allo stato
        animator.SetBool("IsJumping", playerController.CurrentState == AnimationState.JUMP);
        animator.SetBool("IsSliding", playerController.CurrentState == AnimationState.SLIDE);
        animator.SetFloat("LaneSpeed", playerController.CurrentState == AnimationState.MOVELEFT ? -1f :
                                   playerController.CurrentState == AnimationState.MOVERIGHT ? 1f : 0f);

        // Sempre in Run se non saltando/scivolando
        animator.SetBool("IsRunning", playerController.CurrentState == AnimationState.RUN ||
                                       playerController.CurrentState == AnimationState.MOVELEFT ||
                                       playerController.CurrentState == AnimationState.MOVERIGHT);
    }
}
