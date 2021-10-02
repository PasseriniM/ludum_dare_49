using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool dash = false;
	bool jumpHeld = false;

	Gauges gauges; 
	
	private void Awake()
	{
		gauges = GetComponent<Gauges>();
	}

	public void OnJumpButton(InputAction.CallbackContext context)
    {
		if(context.started)
        {
			jump = true;
        }
		if(context.performed)
        {
			jumpHeld = true;
        }
		if(context.canceled)
        {
			jump = false;
			jumpHeld = false;
        }
    }

	public void OnMovement(InputAction.CallbackContext context)
	{
		Vector2 movement = context.ReadValue<Vector2>();
		horizontalMove = movement.x * runSpeed; 
	}

	void Update ()
	{
	}

	public void OnFall()
	{
		animator.SetBool("IsJumping", true);
	}

	public void OnLanding()
	{
		animator.SetBool("IsJumping", false);
	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump, jumpHeld, dash);
		jump = false;
		dash = false;
	}
}
