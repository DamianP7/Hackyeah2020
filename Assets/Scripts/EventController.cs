using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public GameObject coinPrefab;
    public EventControllerCollider[] colliders;
    public EventType eventType;
    public int eventId;
    public int requiedNumber;
    public float duration;

    float time;
    int number;
    bool start = false;
    List<ManControl> people;
    List<bool> started;

    public void Initialize()
    {
        for (int i = 0; i < colliders.Length; i++)
            colliders[i].id = eventId;

        number = 0;
        time = 0;
        people = new List<ManControl>();
        started = new List<bool>();
    }

    public void JoinEvent(ManControl man)
    {
        people.Add(man);
        number++;
    }

    public void CanStart()
    {
        started.Add(true);
        if(started.Count >= requiedNumber)
        {
            Debug.Log("Animation");
            foreach (ManControl item in people)
            {
                item.EventAnimationStart();
            }
            start = true;
        }
    }


    public void Update()
    {
        if(start == true)
        {
            time += Time.deltaTime;
            if(time > duration)
            {
                foreach (ManControl man in people)
                {
                    man.Walking = true;
                }
                CrowdController.Instance.CloseEvent(transform.position);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnMouseDown()
    {
        foreach (ManControl man in people)
        {
            man.Walking = true;
        }
        if (people.Count > 0)
        {
            GameManager.Instance.Points += (requiedNumber - people.Count + 1);
            GameObject.Instantiate(coinPrefab, this.transform.position, Quaternion.identity);
        }
        CrowdController.Instance.CloseEvent(transform.position);
        Destroy(this.gameObject);
    }
}
