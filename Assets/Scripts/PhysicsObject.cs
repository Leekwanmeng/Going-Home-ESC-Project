using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

    public float minGroundNormalY = .65f;
    public float gravityModifier = 1f;

    public Vector2 playerVelocity;	// Player Input
    protected bool grounded;
    protected Vector2 groundNormal;

    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    void OnEnable() {
        rb2d = GetComponent<Rigidbody2D> ();
    }

    void Start () {
    // Collision rules (not trigger, 
    //get and set settings from layerCollisionMatrix to check what layers interact with what)
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask (gameObject.layer));
        contactFilter.useLayerMask = true;
    }
    
    void Update () {
		playerVelocity = Vector2.zero;
        ComputeVelocity(); 
    }

    // To override
    protected virtual void ComputeVelocity() {
    
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

}
