using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    [SerializeField] GameObject bulletGameObject;
    [SerializeField] Transform gun;

    [SerializeField] float runSpeed = 7f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(20f,20f);

    [SerializeField] AudioClip bulletSFX;
    [SerializeField] AudioClip deathSFX;
    bool isAlive = true;

    float gravityScaleAtStart;

    bool isGrounded = true;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }


    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        PlayerDeath();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
        // will give a vector 2 which consist of values for right, left, up and down
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (value.isPressed && isGrounded)
        {
            myRigidbody.velocity = new Vector2(0f, jumpSpeed);
            // isGrounded = false;
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        Instantiate(bulletGameObject, gun.position, transform.rotation);
        // firing bullet there is an empty game object in player which is gun so from gun's location bullet is fired
        AudioSource.PlayClipAtPoint(bulletSFX, Camera.main.transform.position);
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        //The Rigidbody is part of the physics simulation and takes care of the framerate-independency.
        //In other game projects, we directly manipulated the transform.position circumventing the physics simulation.
        // No time.DeltaTime used

        //And what my logic here is, is saying that whatever your current velocity is on y, just keep that,

        //just do the same because we're adding a new velocity each frame.

        //We only need to be adding the x, we don't want to be touching the Y.

        //So instead of saying it's zero, we say just keep it as it currently is
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;


        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }


    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
            bulletGameObject.transform.localScale = transform.localScale;
        }


        //transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        // Mathf.Sign Return value is 1 when f is positive or zero, -1 when f is negative.
        // Mathf.Abs Returns the absolute value of f.
        //   prints 10.5
        //Debug.Log(Mathf.Abs(-10.5f));

    }

    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            return;
        }
        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;

        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
        myRigidbody.gravityScale = 0f;
    }

    void PlayerDeath()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {

            isAlive = false;
            myAnimator.SetTrigger("Dying");
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
            myRigidbody.velocity = deathKick;
            FindObjectOfType<GameSession>().PlayerProcessDeath();
        }
    }

    void OnQuitApplication(InputValue value)
    {
        Application.Quit();
    }
}
