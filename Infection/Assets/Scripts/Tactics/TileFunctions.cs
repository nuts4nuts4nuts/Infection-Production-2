using UnityEngine;
using System.Collections;

public class TileFunctions : EntityFunctions
{
    private bool isInfected = false;

    public bool humanSpawn = false;
    public bool invaderSpawn = false;

	// Use this for initialization
	void Start () 
    {
        originalColor = renderer.material.color;
        currentColor = originalColor;
	}

    public void InfectTile()
    {
        isInfected = true;
        currentColor = Color.red;
        ResetColor();
    }

    public void DisinfectTile()
    {
        isInfected = false;
        currentColor = originalColor;
        ResetColor();
    }
}
