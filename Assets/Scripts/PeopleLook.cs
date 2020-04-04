using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PeopleLook : ScriptableObject
{
	public List<PersonLook> people;
}

[System.Serializable]
public class PersonLook
{
	public Sprite body;
	public Sprite arm;
	public Sprite leg;
}