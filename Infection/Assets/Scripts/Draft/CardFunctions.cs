using UnityEngine;
using System.Collections;

public class CardFunctions : Lerpable
{

    public enum Team
    {
        human = 0,
        invader,
        teamCount
    }

    public string associatedPiece = "";

    public Team team;

    [HideInInspector]
    public bool isDrafted = false;

    public void DraftCard()
    {
        isDrafted = true;
        renderer.material.color = Color.green; 
    }
}
