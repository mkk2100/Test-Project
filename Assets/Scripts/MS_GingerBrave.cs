using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MS_GingerBrave : CommonSkill
{
    #region AnimationState
    private const string GINGER_IDLE = "Ginger_Idle";
    private const string GINGER_WALK = "Ginger_Walk";
    private const string GINGER_BLAST = "Ginger_Blast";
    private const string GINGER_JUMP = "Ginger_Jump";
    private const string GINGER_DOUBLEJUMP = "Ginger_DoubleJump";
    private const string GINGER_LAND = "Ginger_Land";
    private const string GINGER_SLIDE = "Ginger_Slide";
    private const string GINGER_HURT = "Ginger_Hurt";
    private const string GINGER_KILLED = "Ginger_Killed";
    private const string GINGER_BONUSTIME_ENTERED1 = "Ginger_BonusTime_Entered1";
    private const string GINGER_BONUSTIME_ENTERED2 = "Ginger_BonusTime_Entered2";
    private const string GINGER_BONUSTIME_ENTERED3 = "Ginger_BonusTime_Entered3";        
    private const string GINGER_BONUSTIME_IDLE = "Ginger_BonusTime_Idle";
    private const string GINGER_BONUSTIME_UPWARD = "Ginger_BonusTime_Upward";
    private const string GINGER_BONUSTIME_EXITED = "Ginger_BonusTimeExited";
    private const string GINGER_FALL = "Ginger_Fall";
    private const string GINGER_SKILL_ON = "Ginger_Skill_On";
    private const string GINGER_SKILLACTIVATED = "Ginger_Skill_Activated";
    private const string GINGER_SKILL1 = "Ginger_Skill1";
    private const string GINGER_SKILL2 = "Ginger_Skill2";    
    private const string GINGER_SKILL3 = "Ginger_Skill3";
    private const string GINGER_SKILL4 = "Ginger_Skill4";
    #endregion
    
    public AudioClip[] gingerSFX;
    private Vector3 playerPosition;
    private bool hasSetPlayerPosition;
    private bool hasItemGet;

/*
======[START]======
*/
    private void Start()
    {
        blastSpeed = maxSpeed * 2;
        groundMask = 7 << LayerMask.NameToLayer("Ground");        
        extraJump = maxExtraJump;
    }
/*
======[FIXEDUPDATE]======
*/
    private void FixedUpdate()
    {
        if(gameMode == GameMode.DebugMode) return;
        CheckGround(); 
        SetPlayerPosition();
        Walk();
        Jump();
        DoubleJump();
        LandingJump();
        Land();
    }
/*
======[UPDATE]======
*/
    private void Update()
    {
        // Jump
        if(Input.GetKeyDown(KeyCode.F) && parentAnimator.GetCurrentAnimatorStateInfo(0).IsName(GINGER_WALK)) 
        {
            Debug.Log("Input Detected : Jump");            
            isJumpPressed = true;
            animationState = AnimationState.Jump;
        }

        // DoubleJump
        if(Input.GetKeyDown(KeyCode.F) && parentAnimator.GetCurrentAnimatorStateInfo(0).IsName(GINGER_JUMP))
        {
            Debug.Log("Input Detected : Double Jump");
            isJumpPressed = false;
            isDoubleJumpPressed = true;
            animationState = AnimationState.DoubleJump;

        }

        // MultipleJump
        if(Input.GetKeyDown(KeyCode.F) && parentAnimator.GetCurrentAnimatorStateInfo(0).IsName(GINGER_DOUBLEJUMP) && (maxExtraJump - 1 >= extraJump && extraJump > 0))
        {
            Debug.Log("Input Detected : MultipleJump");
            isJumpPressed = false;
        }

        // LandingJump
        if(Input.GetKeyDown(KeyCode.F) && parentAnimator.GetCurrentAnimatorStateInfo(0).IsName(GINGER_LAND))
        {
            Debug.Log("Input Detected : Landing Jump");
        }
    }

    private void SetPlayerPosition()
    {
        if(!hasSetPlayerPosition)
        {
            hasSetPlayerPosition = true;
            playerPosition = this.transform.position;
        }
    }

    public override void Walk()
    {
        base.Walk();
        if(!isJumpPressed && !isDoubleJumpPressed && extraJump == maxExtraJump) animationState = AnimationState.Walk;
        if(animationState == AnimationState.Walk) ChangeAnimationState(GINGER_WALK);
    }

    public override void Jump()
    {
        if(!isJumpPressed) return;
        bool key = true;
        if(extraJump == maxExtraJump)
        {
            base.Jump();
            if(key)
            {
                key = false;
                ChangeAnimationState(GINGER_JUMP);
            }
            SoundManager.instance.SFXPlay("JumpSFX", gingerSFX[1]); // Play SFX[1]
        }        
    }

    public override void DoubleJump()
    {
        if(!isDoubleJumpPressed) return;
        if(maxExtraJump - 1 >= extraJump && extraJump > 0)
        {         
            if(!isJumpPressed)
            {
                base.Jump();
                isJumpPressed = true;
                currentAnimationState = GINGER_DOUBLEJUMP;
                parentAnimator.Play(GINGER_DOUBLEJUMP, 0, 0f);
                SoundManager.instance.SFXPlay("JumpSFX", gingerSFX[1]); // Play SFX[1]
            }
        }
    }

    public override void LandingJump()
    {
        // Nothing special
    }

    public override void Land()
    {
        if(parentAnimator.GetCurrentAnimatorStateInfo(0).IsName(GINGER_JUMP) && animationState == AnimationState.Jump) StartCoroutine(SetLandAnimation(0.04f, animationState));
        if(parentAnimator.GetCurrentAnimatorStateInfo(0).IsName(GINGER_DOUBLEJUMP) && animationState == AnimationState.DoubleJump) StartCoroutine(SetLandAnimation(0.04f, animationState));
    }
    
    IEnumerator SetLandAnimation(float delay, AnimationState currentState)
    {
        yield return new WaitForSeconds(delay);
        yield return new WaitUntil(() => isGround);
        while(animationState == currentState)
        {
            extraJump = maxExtraJump;
            animationState = AnimationState.Land;
            ChangeAnimationState(GINGER_LAND);
            isDoubleJumpPressed = false;
            isJumpPressed = false;
        }                
    }
}
