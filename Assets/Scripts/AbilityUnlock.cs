using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityUnlock : MonoBehaviour
{
    public bool unlockDoubleJump, unlockDash, unlockBecomeBall, unlockDropBomb;
    public GameObject pickupEffect;
    public string unlockMessage;
    public TMP_Text unlockTextBox;

    private void OnTriggerEnter2D(Collider2D otherObject)
    {
        if(otherObject.tag == "Player")
        {
            PlayerAbilityTracker player = otherObject.GetComponentInParent<PlayerAbilityTracker>();
            if (player == null) Debug.Log("No Player");
            if(unlockDoubleJump)
            {
                player.canDoubleJump = true;
            }
            if(unlockDash)
            {
                player.canDash = true;
            }
            if(unlockBecomeBall)
            {
                player.canBecomeBall = true;
            }
            if(unlockDropBomb)
            {
                player.canDropBomb = true;
            }
            Instantiate(pickupEffect, transform.position, transform.rotation);
            // remove the canvas of the gameObject from the main gameObject, so it doenst get removed on destoying the main gameobject
            unlockTextBox.transform.parent.SetParent(null);
            unlockTextBox.transform.parent.position = transform.position;
            unlockTextBox.text = unlockMessage;
            unlockTextBox.gameObject.SetActive(true);
            Destroy(unlockTextBox.transform.parent.gameObject, 5f);
            Destroy(gameObject);
        }
    }
}
