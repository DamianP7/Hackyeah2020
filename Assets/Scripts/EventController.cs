﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public EventControllerCollider[] colliders;
    public EventType eventType;
    public int eventId;
    public int requiedNumber;
    public float duration;

    float time;
    int number;
    List<ManControl> people;

    public void Initialize()
    {
        for (int i = 0; i < colliders.Length; i++)
            colliders[i].id = eventId;

        number = 0;
        time = 0;
        people = new List<ManControl>();
    }

    public void JoinEvent(ManControl man)
    {
        people.Add(man);
        number++;
    }


    public void Update()
    {
        if(number >= requiedNumber)
        {
            time += Time.deltaTime;
            // animation
        }
    }

    private void OnMouseDown()
    {
        foreach (ManControl man in people)
        {
            man.walking = true;
        }
        GameManager.Instance.Points += people.Count;
        CrowdController.Instance.CloseEvent(transform.position);
        Destroy(this.gameObject);
    }
}
