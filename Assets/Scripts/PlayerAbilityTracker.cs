using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityTracker : MonoBehaviour
{
    public bool canDoubleJump, canDash, canBecomeBall, canDropBomb;

    public void ResetSkill()
    {
        canDash = false;
        canDoubleJump = false;
        canBecomeBall = false;
        canDropBomb = false;
    }
}
