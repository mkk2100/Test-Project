using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class CommonSkill : MonoBehaviour
{   
    #region Field
    // Game Mode
    public Action playerAction;
    public enum GameMode
    {
        DefaultMode,
        DebugMode
    }
    public GameMode gameMode;  

    // Miscellaneous
    public Rigidbody2D parentRigid;
    //public BoxCollider2D parentBoxCollider;
    public CapsuleCollider2D parentCapsuleCollider;
    public Animator parentAnimator;    
    public AudioClip[] baseSoundFX;

    // Animation
    protected AnimationState animationState;
    protected enum AnimationState
    {
        Walk,
        Jump,
        DoubleJump,
        Land
    };    
    protected string currentAnimationState;

    [Header("Cookie Settings")]
    public int maxExtraJump = 2;
    public int jumpForce;
    public float maxSpeed = 16.0f;
    public float minSpeed = 0.0f;
    public float blastSpeed;     

    // Ground Check
    protected int groundMask;
    protected bool isGround;

    // Statue Check
    protected bool isJumpPressed;
    protected bool isDoubleJumpPressed;

    // Jump Initialize
    protected int extraJump;   
    #endregion

    private void Awake()
    {
        SetDefaultStatue();
    }

    protected void SetDefaultStatue()
    {
        animationState = AnimationState.Walk;
    }

    public virtual void Walk()
    {
        parentRigid.velocity = new Vector2(maxSpeed / 2, parentRigid.velocity.y);
    }

    public virtual void Jump()
    {
        extraJump -= 1;        
        parentRigid.velocity = new Vector2(parentRigid.velocity.x, jumpForce);
    }

    public virtual void DoubleJump()
    {
        // Nothing special
    }

    public virtual void LandingJump()
    {
        // Nothing special
    }

    public virtual void Slide()
    {
        // Nothing special
    }

    public virtual void Land()
    {
        // Nothing special
    }

    protected void CheckGround()
    {
        isGround = Physics2D.IsTouchingLayers(parentCapsuleCollider, groundMask);
    }

    protected void ChangeAnimationState(string newAnimationState)
    {
        if(currentAnimationState == newAnimationState) return;
        parentAnimator.Play(newAnimationState);
        currentAnimationState = newAnimationState;
    }   
}