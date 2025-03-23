using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBotSpawner : MonoBehaviour
{
    public MessagePopup[] popups;
    public MessagePopup mBotMessage;
    public GameObject mBot;

    private bool hasSpawned;

    // Update is called once per frame
    void Update()
    {
        foreach (MessagePopup popup in popups)
        {
            if (!popup.hasArrived)
            {
                return;
            }
        }

        if (!hasSpawned)
        {
            hasSpawned = true;

            mBotMessage.enabled = true;

            mBot.transform.GetComponent<MBotController>().enabled = true;
            mBot.transform.GetComponent<SpaceshipMover>().enabled = true;
        }
    }
}
