using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour
{
	public int id;
	public ManControl man;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "SpawnPoint")
		{
			CrowdController.Instance.people.Remove(man);
			Destroy(man.gameObject);
		}
		else if (collision.tag == "CheckRoad")
		{
			CheckCollider col = collision.GetComponent<CheckCollider>();
			if (col == null)
				man.Collision(collision.transform.position);
			else if (man.id != col.id)
				man.Collision(collision.transform.position);
		}
	}
}
