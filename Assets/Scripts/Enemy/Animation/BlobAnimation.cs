using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobAnimation
{
    Animator animator;

    public BlobAnimation(Animator animator)
    {
        this.animator = animator;
    }

    public enum Animation_States
    {
        StartJump,
        EndJump,
        VelocityY,
    }

    public void SetTrigger(Animation_States state)
    {
        animator.SetTrigger(state.ToString());
    }

    public void SetFloat(Animation_States state, float value)
    {
        animator.SetFloat(state.ToString(), value);
    }

}
