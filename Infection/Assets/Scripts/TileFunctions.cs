using UnityEngine;
using System.Collections;

public class TileFunctions : MonoBehaviour
{
    [HideInInspector]
    public bool isSelected = false;

    private bool isInfected = false;

    Color originalColor;
    Color currentColor;

	// Use this for initialization
	void Start () 
    {
        originalColor = renderer.material.color;
        currentColor = originalColor;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void ResetColor()
    {
        renderer.material.color = currentColor;
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
