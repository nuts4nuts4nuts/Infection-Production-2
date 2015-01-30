using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieceFunctions : MonoBehaviour {

    //[SerializeField]
    public List<Vector2> movementDirections = new List<Vector2>();
    public int ySign = 1; //does the piece move up or down when it goes forward?

    public int cooldown = 1;
    public int incubateTime = 2;

    [HideInInspector]
    public bool isIncubating = false;

    [HideInInspector]
    public int turnsTillMove = 0;

    private bool isLerping = false;
    private Vector3 lerpStart;
    private Vector3 lerpTarget;
    private float lerpCounter = 0.0f;

	// Update is called once per frame
	void Update ()
    {
        if(isLerping)
        {
            Vector3.Lerp(lerpStart, lerpTarget, lerpCounter);
            lerpCounter += Time.deltaTime;

            if(lerpCounter >= 1.0f)
            {
                isLerping = false;
                lerpCounter = 0.0f;
            }
        }
    }

    public void LerpTo(Vector3 pos)
    {
        isLerping = true;
        lerpTarget = pos;
        lerpStart = transform.position;
    }

    public void JustMoved()
    {
        turnsTillMove = cooldown;
    }

    public void StartIncubate()
    {
        turnsTillMove = incubateTime;
        isIncubating = true;
    }

    public int TurnPassed()
    {
        turnsTillMove--;

        return turnsTillMove;
    }

    public void FinishIncubate()
    {
        isIncubating = false;
    }
}
