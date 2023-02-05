using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobAnimation
{
    Animator animator;
    Animation_States currentState;

    public BlobAnimation(Animator animator)
    {
        this.animator = animator;
    }

    public enum Animation_States
    {
        StartJump,
        EndJump,
        Squat,
    }

    public void SetTrigger(Animation_States state)
    {
        if (state == currentState) return;
        if (animator == null) return;
        animator.SetTrigger(state.ToString());
        currentState = state;
    }
}
