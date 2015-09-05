﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ActorMovement))]
public class BirdController : MonoBehaviour {

	// Setup
	Animator anim;
	public Transform mainCam;

	// Movement
	public float accel			= 70f;			//acceleration/deceleration in air or on the ground
	public float decel			= 7.6f;
	[Range(0f, 5f)]
	public float rotateSpeed	= 0.7f;			//how fast to rotate
	public float maxSpeed		= 20;			//maximum speed of movement in X/Z axis

	//Privates
	private ActorMovement actorMovement;
	private Rigidbody rigidBody;

	private Quaternion screenSpace;
	private Vector3 direction, moveDirection, screenSpaceForward, screenSpaceRight, movingObjSpeed;

	/**
	 * Initialize
	 */
	void Awake () {
		// Assign references
		anim			= GetComponent<Animator> ();
		actorMovement	= GetComponent<ActorMovement> ();
		mainCam			= GameObject.FindGameObjectWithTag("MainCamera").transform;
	}

	/**
	 * Update player once per frame
	 */
	void Update () {
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		//get movement axis relative to camera
		screenSpace			= Quaternion.Euler (0, mainCam.eulerAngles.y, 0);
		screenSpaceForward	= screenSpace * Vector3.forward;
		screenSpaceRight	= screenSpace * Vector3.right;

		//animation
		if (h == 0 && v == 0)
			anim.SetBool ("running", false);
		else
			anim.SetBool ("running", true);

		//OLD: transform.Translate(new Vector3(-xPos, 0, -yPos));

		direction = (screenSpaceForward * v) + (screenSpaceRight * h);
		moveDirection = transform.position + direction;
	}

	/**
	 * Apply player movement with physics calculations
	 */
	void FixedUpdate ()
	{
		actorMovement.MoveTo (moveDirection, accel, 0.7f);

		if (rotateSpeed != 0 && direction.magnitude != 0)
			actorMovement.RotateToDirection (moveDirection , rotateSpeed * 5);

		actorMovement.ManageSpeed (decel, maxSpeed + movingObjSpeed.magnitude);
	}
}
