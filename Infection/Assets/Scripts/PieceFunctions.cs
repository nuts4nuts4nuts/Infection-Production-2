using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieceFunctions : EntityFunctions
{
    public enum Team
    {
        human = 0,
        invader = 1,
        team_count = 2
    }

    public enum LerpSpeed
    {
        slow = 1,
        med = 2,
        fast = 4,
        faster = 6,
        blazin = 8
    }

    //[SerializeField]
    public List<Vector2> movementDirections = new List<Vector2>();
    public int ySign = 1; //does the piece move up or down when it goes "forward"?

    public int cooldown = 1;
    public int incubateTime = 2;

    [HideInInspector]
    public bool isIncubating = false;

    [HideInInspector]
    public int turnsTillMove = 0;

    private bool isLerping = false;
    private Vector3 lerpStart;
    private Vector3 lerpTarget;
    private LerpSpeed lerpSpeed = LerpSpeed.med;

    public Team team;

    void Start()
    {
        originalColor = renderer.material.color;
        currentColor = originalColor;

        if (tag == "InvaderPiece")
        {
            team = Team.invader;

            InfectTile();
        }
        else if(tag == "HumanPiece")
        {
            team = Team.human;
        }
    }

	// Update is called once per frame
    void Update()
    {
        if (isLerping)
        {
            //we slerping... or something
            transform.position = Vector3.Lerp(transform.position, lerpTarget, (float)lerpSpeed * Time.deltaTime);
            if (Vector3.SqrMagnitude(transform.position - lerpTarget) < 0.001f)
            {
                isLerping = false;

                if (team == Team.invader)
                {
                    InfectTile();
                }
            }
        }
    }

    private void InfectTile()
    {
        GameObject tile = GetTile();

        TileFunctions tf = (TileFunctions)tile.GetComponent(typeof(TileFunctions));
        tf.InfectTile();
    }

    public GameObject GetTile()
    {
        Vector3 direction = new Vector3(0, 0, 1);
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, direction, out hitInfo, 1000))
        {
            TileFunctions tf = (TileFunctions)hitInfo.collider.gameObject.GetComponent(typeof(TileFunctions));
            if (tf != null)
            {
                return hitInfo.collider.gameObject;
            }
        }
        return null;
    }

    public void LerpTo(Vector3 pos, LerpSpeed speed)
    {
        isLerping = true;
        lerpSpeed = speed;
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
