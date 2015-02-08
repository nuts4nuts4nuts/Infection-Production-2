﻿using UnityEngine;
using System.Collections;

public class TileFunctions : Lerpable
{
    private bool isInfected = false;

    public bool humanSpawn = false;
    public bool invaderSpawn = false;

    GameObject gameManager;
    GameManager managerFunctions;

    protected override void Awake()
    {
        base.Awake();

        gameManager = GameObject.Find("GameManager");
        managerFunctions = (GameManager)gameManager.GetComponent(typeof(GameManager));
    }

	// Use this for initialization
	protected void Start() 
    {
        managerFunctions.numCleanTiles++;
	}

    public void InfectTile()
    {
        isInfected = true;
        SetColorCurrent(Color.red);
        managerFunctions.numCleanTiles--;
    }

    public void DisinfectTile()
    {
        isInfected = false;
        managerFunctions.numCleanTiles++;
        ResetToOriginalColor();
    }

    public bool isOccupied()
    {
        Vector3 direction = new Vector3(0, 0, -1);
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, direction, out hitInfo, 5))
        {
            return true;
        }

        return false;
    }
}
