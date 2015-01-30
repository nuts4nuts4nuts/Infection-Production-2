using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieceFunctions : MonoBehaviour {

    //[SerializeField]
    public List<Vector2> movementDirections = new List<Vector2>();
    public int ySign = 1; //does the piece move up or down when it goes forward?

    public int cooldown = 1;
    [HideInInspector]
    public int turnsTillMove = 0;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
