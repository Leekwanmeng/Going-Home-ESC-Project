using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : PhysicsObject {

    public float maxSpeed = 7f;
    public float jumpTakeOffSpeed = 10f;
    public float fallMultiplier = 1.1f;
	[HideInInspector] public bool facingRight = true;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Use this for initialization
    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        animator = GetComponent<Animator>();
    }


    protected override void ComputeVelocity() {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis ("Horizontal");

        if (Input.GetButtonDown ("Jump") && grounded) {
            velocity.y = jumpTakeOffSpeed;
        } else if (Input.GetButtonUp ("Jump") && velocity.y > 0) {
        	// slows down jump when released
            velocity.y = velocity.y * 0.5f;
        }

		if (move.x > 0 && !facingRight) {
			flip();
		} else if (move.x < 0 && facingRight) {
			flip();
		}

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

		playerVelocity.x = move.x * maxSpeed;
    }

    public void MoveRight() {
    	print("working right");
    	playerVelocity.x = maxSpeed;
    }

    public void MoveLeft(){
		print("working left");
    	playerVelocity.x = -maxSpeed;
    }

    public void stopMovement() {
		playerVelocity.x = 0;
    }

	void flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


}