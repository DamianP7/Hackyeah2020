using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdController : MonoBehaviour
{
	private static CrowdController instance;
	public static CrowdController Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType<CrowdController>();
			return instance;
		}
	}

	[Header("People")]
	public PeopleLook peopleLook;
	public GameObject[] prefabs;
	public List<ManControl> people;
	public float manSize;
	public float spawnTime;
	public int peopleWalkingInTheSameDir;

	[Header("City")]
	public Transform spawnLeft;
	public Transform spawnRight;
	public Transform spawnTopPoint;
	public Transform spawnBottomPoint;

	[Header("Events")]
	public GameObject[] eventPrefabs;
	public float eventSpawnTime;
	public int maxActiveEvents;
	public float distanceBeetwenEvents;

	int left, right;

	Vector2 spawnPosLeft, spawnPosRight;
	float spawnPosTop, spawnPosBottom;

	float timer;
	float eventTimer;

	int manId = 1001;
	int eventId = 1;
	int activeEvents;
	List<Vector2> eventPositions;
	List<EventController> events;
	List<int> lastIndexesGuy;
	List<int> lastIndexesGirl;
	int index = 0;

	private void Awake()
	{
		Setup();
	}

	private void Setup()
	{
		people = new List<ManControl>();
		eventPositions = new List<Vector2>();
		events = new List<EventController>();
		lastIndexesGuy = new List<int>();
		lastIndexesGuy.Add(100);
		lastIndexesGuy.Add(100);
		lastIndexesGuy.Add(100);
		lastIndexesGuy.Add(100);
		lastIndexesGuy.Add(100);
		lastIndexesGuy.Add(100);
		lastIndexesGuy.Add(100);
		lastIndexesGuy.Add(100);

		lastIndexesGirl = new List<int>();
		lastIndexesGirl.Add(100);
		lastIndexesGirl.Add(100);
		lastIndexesGirl.Add(100);
		lastIndexesGirl.Add(100);
		lastIndexesGirl.Add(100);

		timer = eventTimer = 0;
		left = right = 0;
		activeEvents = 0;
		spawnPosLeft = spawnLeft.position;
		spawnPosRight = spawnRight.position;
		spawnPosTop = spawnTopPoint.position.y;
		spawnPosBottom = spawnBottomPoint.position.y;
	}

	private void OnEnable()
	{
		Setup();
	}

	private void OnDisable()
	{
		for (int i = people.Count - 1; i >= 0; i--)
		{
			Destroy(people[i].gameObject);
		}
		for (int i = people.Count - 1; i >= 0; i--)
		{
			Destroy(people[i].gameObject);
		}
	}

	private void Update()
	{
		timer += Time.deltaTime;
		eventTimer += Time.deltaTime;
		if (timer > spawnTime)
		{
			timer = 0;
			SpawnMan();
		}
		if (eventTimer > eventSpawnTime)
		{
			eventTimer = 0;
			if (activeEvents < maxActiveEvents)
				SpawnEvent();
		}
	}

	void SpawnMan(WalkingDirection direction = WalkingDirection.None, float yPos = -100, int id = 0)
	{
		int random = Random.Range(0, prefabs.Length);
		GameObject newObject = GameObject.Instantiate(prefabs[random]);
		ManControl newMan = newObject.GetComponent<ManControl>();

		if (direction == WalkingDirection.None)
			direction = GetRandomDirection();

		Vector2 startVector, endVector, spawnVector = Vector2.zero;
		if (yPos > -100)
			spawnVector.y = yPos;
		else
			spawnVector.y = Random.Range(spawnPosTop, spawnPosBottom);

		if (direction == WalkingDirection.Left)
		{
			left++;
			startVector = spawnPosRight;
			endVector = spawnPosLeft;
		}
		else
		{
			right++;
			startVector = spawnPosLeft;
			endVector = spawnPosRight;
		}
		spawnVector.x = startVector.x;

		newObject.transform.position = spawnVector;
		newMan.speed += Random.Range(-newMan.speed / 3, newMan.speed / 3);
		newMan.Walking = true;
		if (id > 0)
			newMan.id = id;
		else
			newMan.id = manId++;
		int min, max, look;
		if (random == 1)
		{
			min = 0;
			max = 5;
			do
			{
				look = Random.Range(min, max);
			} while (lastIndexesGirl.Contains(look));
		}
		else
		{
			min = 5;
			max = peopleLook.people.Count;
			do
			{
				look = Random.Range(min, max);
			} while (lastIndexesGuy.Contains(look));
		}
		lastIndexesGuy[index % lastIndexesGuy.Count] = look;
		newMan.Initialize(startVector, endVector, direction, peopleLook.people[look]);
	}

	void SpawnEvent()
	{
		Vector2 spawnVector = GetEventPos();
		if (spawnVector == Vector2.zero)
			return;

		GameObject newObject = GameObject.Instantiate(eventPrefabs[Random.Range(0, eventPrefabs.Length)]);
		EventController newEvent = newObject.GetComponent<EventController>();

		newEvent.transform.position = spawnVector;

		if (newEvent.requiedNumber == 1)
		{
			SpawnMan(yPos: spawnVector.y, id: eventId);
		}
		else
		{
			WalkingDirection direction = WalkingDirection.Left;
			for (int i = 0; i < newEvent.requiedNumber; i++)
			{
				SpawnMan(direction, spawnVector.y, id: eventId);
				direction = direction == WalkingDirection.Left ? WalkingDirection.Right : WalkingDirection.Left;
			}
		}
		newEvent.eventId = eventId++;
		newEvent.Initialize();

		eventPositions.Add(spawnVector);
		activeEvents++;
	}

	Vector2 GetEventPos()
	{
		bool tooClose = false;
		Vector2 spawnVector = Vector2.zero;
		int check = 0;
		do
		{
			tooClose = false;
			spawnVector.y = Random.Range(spawnPosTop, spawnPosBottom);
			spawnVector.x = Random.Range(spawnPosLeft.x + 1.5f, spawnPosRight.x - 1.5f);

			foreach (Vector2 pos in eventPositions)
			{
				if (Vector2.Distance(spawnVector, pos) < distanceBeetwenEvents)
				{
					tooClose = true;
					break;
				}
			}
			if (check > 30)
			{
				spawnVector = Vector2.zero;
				break;
			}
			check++;
		} while (tooClose);

		return spawnVector;
	}

	public void CloseEvent(Vector2 pos)
	{
		Debug.Log("Closed event");

		activeEvents--;
		eventPositions.Remove(pos);
	}

	WalkingDirection GetRandomDirection()
	{
		if (left - right > peopleWalkingInTheSameDir)
			return WalkingDirection.Right;
		else if (right - left > peopleWalkingInTheSameDir)
			return WalkingDirection.Left;
		else
			return Random.Range(0, 2) < 1 ? WalkingDirection.Left : WalkingDirection.Right;
	}
}
