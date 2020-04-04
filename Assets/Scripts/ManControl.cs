using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManControl : MonoBehaviour
{
	public CheckCollider checkColliderLeft, checkColliderRight;
	public CheckEvent checkEvent;
	public WalkingDirection direction;
	[Range(0.008f, 0.015f)]
	public float speed = 0.01272f;
	public float horizontalSpeed = 0.003f;
	public int horizontalMovementTime = 30;
	public bool walking = false;
	public int id;

	int sideMove = 0;

	Vector3 realSpeed = Vector3.zero;

	public void Initialize(Vector2 startPos, Vector2 endPos, WalkingDirection dir)
	{
		direction = dir;
		checkColliderLeft.gameObject.SetActive(direction == WalkingDirection.Left);
		checkColliderRight.gameObject.SetActive(direction == WalkingDirection.Right);
		checkColliderLeft.id = checkColliderRight.id = checkEvent.id = id;
		realSpeed.x = direction == WalkingDirection.Right ? speed : -speed;
	}

	public void Update()
	{
		if(walking)
		{
			Vector3 newPos = transform.position + realSpeed;
			transform.position = newPos; //Set(transform.position + realSpeed);
			if (sideMove > 0)
			{
				sideMove--;
				if (sideMove == 0)
					realSpeed.y = 0;
			}
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
}