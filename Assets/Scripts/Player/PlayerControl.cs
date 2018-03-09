using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : PhysicsObject {

    public float maxSpeed = 7f;
    public float jumpTakeOffSpeed = 9f;
    public float fallMultiplier = 1.1f;
	public float minGroundNormalY = .65f;
	[HideInInspector] public bool facingRight = true;

	protected bool grounded;
	protected Vector2 groundNormal;

	private Vector2 playerVelocity;	// Player Input

	// Initialisation for local player
	// TODO: Configure cameras and input
	public override void OnStartLocalPlayer() {
		
	}

	void Update () {
		if (!isLocalPlayer) {
			return;
		}
		playerVelocity = Vector2.zero;
        ComputeVelocity(); 
        CheckDuck();
    }

    // For every frame update, forces on RigidBody2D
    void FixedUpdate() {
		grounded = false;

        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
		velocity.x = playerVelocity.x;
        Vector2 distMoved = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2 (groundNormal.y, -groundNormal.x);	// For along line of slope
		Vector2 xMove = moveAlongGround * distMoved.x;
		Vector2 yMove = Vector2.up * distMoved.y;
        horizontalMovement(xMove);
		verticalMovement(yMove);
    }

	void movement(Vector2 move, bool yMovement) {
        float distance = move.magnitude;

        // Ignores if idle
        if (distance > minMoveDistance) {
            checkCollision(move, distance);

            for (int i = 0; i < hitBufferList.Count; i++) {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY) {
                    grounded = true;
                    if (yMovement) {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0) {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                if (modifiedDistance < distance) {
                	distance = modifiedDistance;
                }
            }

        }

        rb2d.position = rb2d.position + move.normalized * distance;
    }

    void horizontalMovement(Vector2 xMove) {
    	movement(xMove, false);
    }

    void verticalMovement(Vector2 yMove) {
		movement(yMove, true);
    }

    void checkCollision(Vector2 move, float distance) {
    	// shell as padding to prevent colliders from being stuck
		int count = rb2d.Cast (move, contactFilter, hitBuffer, distance + shellRadius);
        hitBufferList.Clear();
        for (int i = 0; i < count; i++) {
            hitBufferList.Add(hitBuffer[i]);
        }
    }


	protected void ComputeVelocity() {
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

    void CheckDuck() {
    	if (Input.GetButtonDown("Fire1")) {
    		animator.SetBool("interact", true);
		} else {
			animator.SetBool("interact", false);
    	}
    }

    /*
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
    */

	void flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


}