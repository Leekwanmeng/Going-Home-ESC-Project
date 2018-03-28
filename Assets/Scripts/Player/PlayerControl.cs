using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : PhysicsObject {

    public float maxSpeed = 7f;
    public float jumpTakeOffSpeed = 9f;
    public float fallMultiplier = 1.1f;
	public float minGroundNormalY = .65f;
	public float test=0f;

	//test

	private Vector2 playerVelocity;
	private bool isJump;
	private bool isLeft;
	private bool isRight;



	[HideInInspector] public bool facingRight = true;

	protected bool grounded;
	protected Vector2 groundNormal;


// Player Input

	void Update () {
		playerVelocity = Vector2.zero;
        ComputeVelocity(); 
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
	//	gameObject.tag.
		print (rb2d.position);



   }

	float movement(Vector2 move, bool yMovement) {
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
		return rb2d.position.magnitude;
	
    }

     float horizontalMovement(Vector2 xMove) {
    	float position = movement(xMove, false);
		return position;
    }
	float teSt(float t){
		t = Mathf.Abs (velocity.x) / maxSpeed;
		return t;
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
		test = Mathf.Abs (velocity.x) / maxSpeed;
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

		playerVelocity.x = move.x * maxSpeed;
    }

    public void MoveRight() {
    	print("working right");
		//Debug.Log("hththt");
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



	//set n get
	public Vector2 getVelocity(){
		return playerVelocity;
	}
	public void moveL(){
		playerVelocity.x = -3;
	}
	public void moveR(){
		playerVelocity.x = 3;
	}
	public void moveH(){
		playerVelocity.y = 3;
	}
	public void setIsJump(){
		if (playerVelocity.y > 0) {

			isJump=true;
		} else {
			isJump= false;
		}
	}
	public bool getIsJump(){
		return isJump;
	}
	public void setIsMove(){
		if (playerVelocity.x > 0) {
			isRight = true;
			isLeft = false;
		} else if (playerVelocity.x < 0) {
			isRight = false;
			isLeft = true;
		} else {
			isRight = false;
			isLeft = false;
		}
	}
	public bool getIsLeft(){
		return isLeft;
	}
	public bool getIsRight(){
		return isRight;
	}



}