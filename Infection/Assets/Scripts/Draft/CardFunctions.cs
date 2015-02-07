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

    void Start()
    {
        secondaryPosition = new Vector3(-1.5f, 0, -4f);
    }

    public void DraftCard()
    {
        isDrafted = true;
        renderer.material.color = Color.green; 
    }
}
