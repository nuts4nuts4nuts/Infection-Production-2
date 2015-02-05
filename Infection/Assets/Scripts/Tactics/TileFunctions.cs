using UnityEngine;
using System.Collections;

public class TileFunctions : Lerpable
{
    private bool isInfected = false;

    public bool humanSpawn = false;
    public bool invaderSpawn = false;

    GameObject gameManager;
    GameManager managerFunctions;

	// Use this for initialization
	protected void Start () 
    {
        originalColor = renderer.material.color;
        currentColor = originalColor;
        gameManager = GameObject.Find("GameManager");
        managerFunctions = (GameManager)gameManager.GetComponent(typeof(GameManager));

        managerFunctions.numCleanTiles++;
	}

    public void InfectTile()
    {
        isInfected = true;
        currentColor = Color.red;
        managerFunctions.numCleanTiles--;
        ResetColor();
    }

    public void DisinfectTile()
    {
        isInfected = false;
        currentColor = originalColor;
        managerFunctions.numCleanTiles++;
        ResetColor();
    }
}
