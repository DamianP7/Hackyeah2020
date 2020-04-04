using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventControllerCollider : MonoBehaviour
{
	public EventController eventController;
	public int id;

	public void AddMan(ManControl man)
	{
		eventController.JoinEvent(man);
		this.gameObject.SetActive(false);
	}
}
