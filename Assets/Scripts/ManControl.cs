using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManControl : MonoBehaviour
{
	public CheckCollider checkColliderLeft, checkColliderRight;
	public CheckEvent checkEvent;
	public WalkingDirection direction;
	public Transform character;
	public SpriteRenderer body, arm1, arm2, leg1, leg2;
	public Animator animator;
	[Range(0.008f, 0.015f)]
	public float speed = 0.01272f;
	public float horizontalSpeed = 0.003f;
	public int horizontalMovementTime = 30;
	public float chanceToWait = 10;

	[HideInInspector]
	public int id;

	int sideMove = 0;
	float scale;

	Vector3 realSpeed = Vector3.zero;
	Vector3 sideSpeed = Vector3.zero;
	float yPosDestination;
	EventType actualEvent;
	EventController eventController;

	private bool sideWalking = false;
	private bool walking = false;
	public bool Walking { get => walking; 
		set
		{
			walking = value;
			animator.SetTrigger(value ? "Walk" : "Idle");
		}
	}

	public void Initialize(Vector2 startPos, Vector2 endPos, WalkingDirection dir, PersonLook personLook)
	{
		direction = dir;
		checkColliderLeft.gameObject.SetActive(direction == WalkingDirection.Left);
		checkColliderRight.gameObject.SetActive(direction == WalkingDirection.Right);
		checkColliderLeft.id = checkColliderRight.id = checkEvent.id = id;
		realSpeed.x = direction == WalkingDirection.Right ? speed : -speed;
		scale = character.localScale.x;
		if (direction == WalkingDirection.Right)
			character.localScale = new Vector3(scale, character.localScale.y);
		else
			character.localScale = new Vector3(-scale, character.localScale.y);
		SetupLook(personLook);
	}

	public void Update()
	{
		if(Walking)
		{
			Vector3 newPos = transform.position + realSpeed;
			newPos.z = newPos.y;
			transform.position = newPos; //Set(transform.position + realSpeed);
			if (sideMove > 0)
			{
				sideMove--;
				if (sideMove == 0)
					realSpeed.y = 0;
			}

			if (Random.Range(0, 100) < chanceToWait)
				StartCoroutine(Wait());
		}
		if(sideWalking)
		{
			if (transform.position.y < yPosDestination)
				sideSpeed.y = horizontalSpeed;
			else
				sideSpeed.y = -horizontalSpeed;
			Vector3 newPos = transform.position + sideSpeed;
			newPos.z = newPos.y;
			Debug.Log(newPos.y + "; " + (yPosDestination + sideSpeed.y * 6) + "; " + (yPosDestination - sideSpeed.y * 6));
			if (transform.position.y > yPosDestination + sideSpeed.y * 6
				&& transform.position.y < yPosDestination - sideSpeed.y * 6)
			{
				sideWalking = false;
				newPos.y = yPosDestination;
				animator.SetTrigger("Idle");
				eventController.CanStart();
			}
			transform.position = newPos;
		}
	}

	public void Collision(Vector2 obstacle)
	{
		sideMove = horizontalMovementTime;
		float dir = transform.position.y - obstacle.y;
		if (dir > 0)
			realSpeed.y = horizontalSpeed;
		else
			realSpeed.y = -horizontalSpeed;
		//speed = 0.01272
	}

	public void OnEventEnter(Vector2 place, EventController eventController)
	{
		this.eventController = eventController;
		Debug.Log("OnEventEnter)");
		yPosDestination = place.y;
		float dir = transform.position.y - place.y;
		Debug.Log(dir);

		actualEvent = eventController.eventType;
		walking = false;

		if (dir < 0.01f && dir > -0.01f)
		{
			transform.position = new Vector3(transform.position.x, yPosDestination, yPosDestination);
			animator.SetTrigger("Idle");
			eventController.CanStart();
		}
		else
		{
			if (transform.position.y < place.y)
				sideSpeed.y = horizontalSpeed;
			else
				sideSpeed.y = -horizontalSpeed;
			sideWalking = true;
		}

	}

	public void EventAnimationStart()
	{
		switch (actualEvent)
		{
			case EventType.Talking:
				animator.SetTrigger("Handshake");
				break;
			case EventType.Handshake:
				animator.SetTrigger("Handshake");
				break;
			case EventType.Hug:
				animator.SetTrigger("Handshake");
				break;
			case EventType.Group:
				animator.SetTrigger("Handshake");
				break;
		}
	}

	void SetupLook(PersonLook personLook)
	{
		body.sprite = personLook.body;
		arm1.sprite = personLook.arm;
		arm2.sprite = personLook.arm;
		leg1.sprite = personLook.leg;
		leg2.sprite = personLook.leg;
	}

	IEnumerator Wait()
	{
		Walking = false;
		yield return new WaitForSeconds(Random.Range(2f, 5f));
		Walking = true;
	}
}