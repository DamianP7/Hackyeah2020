using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEvent : MonoBehaviour
{
	public int id;
	public ManControl man;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "EventCollider")
		{ 
			EventControllerCollider collider = collision.GetComponent<EventControllerCollider>();
			if (id == collider.id)
			{
				collider.AddMan(man);
				man.OnEventEnter(collider.transform.position, collider.eventType);
			}	
		}
	}
}
