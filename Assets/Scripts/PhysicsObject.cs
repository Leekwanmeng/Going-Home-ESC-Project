using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PhysicsObject : NetworkBehaviour {
	// Public Fields
    public float gravityModifier = 1f;

    // Protected constant fields (to inherit)
	protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

	protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
	protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

	protected Rigidbody2D rb2d;
	protected SpriteRenderer spriteRenderer;
    protected Animator animator;

    void Awake() {
        rb2d = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer>(); 
        animator = GetComponent<Animator>();
    }

    void Start () {
    // Collision rules (not trigger, 
    //get and set settings from layerCollisionMatrix to check what layers interact with what)
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask (gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    void FixedUpdate() {
		velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
    }
    


}
