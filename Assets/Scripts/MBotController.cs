using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MBotController : MonoBehaviour
{
    public GameObject player;
    public GameObject earth;
    public TextMeshProUGUI mBotMessage;
    public TextMeshProUGUI earthMessage;
    public float followTime;
    public float maxDistanceToStartClock;

    private SpaceshipMover mover;
    private float startTime;
    private float awakeTime;
    private bool hasMet;

    private float earthMaxDistance;


    // Start is called before the first frame update
    void Start()
    {
        print("AWAKE");
        awakeTime = Time.fixedTime;
        mover = transform.GetComponent<SpaceshipMover>();
        earthMaxDistance = earthMessage.GetComponent<MessagePopup>().fadeDistance;
        earthMessage.GetComponent<MessagePopup>().fadeDistance = 0;
        earthMessage.GetComponent<MessagePopup>().maxDistance = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        print("FixedTime: " + Time.fixedTime);
        print("StarTime + FollowTime: " + startTime + followTime);
        print("AwakeTime: " + awakeTime);
        print("Remaining: " + (startTime + followTime - Time.fixedTime));
        
        if (startTime + followTime - Time.fixedTime > 0 || !hasMet)
        {
            mover.Move(player.transform.position);
        }
        else
        {
            mover.Move(earth.transform.position);
            transform.GetComponent<SpaceshipMover>().speed = 10;
        }

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= maxDistanceToStartClock)
        {
            OnMeetup();
        }

        if (earthMessage.GetComponent<MessagePopup>().hasArrived == true)
        {
            OnArrival();
        }
    }

    public void OnMeetup()
    {
        if (!hasMet)
        {
            earthMessage.GetComponent<MessagePopup>().fadeDistance = earthMaxDistance;
            earthMessage.GetComponent<MessagePopup>().maxDistance = 4;

            hasMet = true;
            print("Met!");
            startTime = Time.fixedTime;
        }
    }

    public void OnArrival()
    {
        mBotMessage.text = "";
    }
}
