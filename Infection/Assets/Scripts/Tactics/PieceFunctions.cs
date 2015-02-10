using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieceFunctions : Lerpable
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

    [HideInInspector]
    public Team team;

    private ParticleSystem pSystem;


    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        if (tag == "InvaderPiece")
        {
            team = Team.invader;
        }
        else if(tag == "HumanPiece")
        {
            team = Team.human;
        }

        pSystem = (ParticleSystem)gameObject.GetComponentInChildren(typeof(ParticleSystem));
    }

	// Update is called once per frame
    protected override void Update()
    {
        if (isLerping)
        {
            //we slerping... or something
            transform.position = Vector3.Lerp(transform.position, lerpTarget, lerpSpeed * Time.deltaTime);
            if (Vector3.SqrMagnitude(transform.position - lerpTarget) < 0.001f)
            {
                isLerping = false;
                transform.position = lerpTarget;
                FinishedLerping();

                if (team == Team.invader)
                {
                    InfectTile();
                }
            }
        }
    }

    public void InfectTile()
    {
        TileFunctions tile = GetTile();

        tile.InfectTile();
    }

    public TileFunctions GetTile()
    {
        Vector3 direction = new Vector3(0, 0, 1);
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, direction, out hitInfo, 1000))
        {
            TileFunctions tf = (TileFunctions)hitInfo.collider.gameObject.GetComponent(typeof(TileFunctions));
            if (tf != null)
            {
                return tf;
            }
        }
        return null;
    }

    public void LerpTo(Vector3 pos, LerpSpeed speed)
    {
        isLerping = true;
        lerpSpeed = (float)speed;
        lerpTarget = pos;
    }

    public void JustMoved()
    {
        turnsTillMove = cooldown;
        SetUnavailable();
    }

    public void SetAvailable()
    {
        pSystem.enableEmission = true;
    }

    public void SetUnavailable()
    {
        pSystem.enableEmission = false;
    }

    public void StartIncubate()
    {
        turnsTillMove = incubateTime;
        SetUnavailable();
        isIncubating = true;
    }

    public int TurnPassed()
    {
        turnsTillMove--;

        if(turnsTillMove <= 0)
        {
            SetAvailable();
        }

        return turnsTillMove;
    }

    public void FinishIncubate()
    {
        isIncubating = false;
    }

    public List<GameObject> GeneratePossibleMoves(Camera cam, int tileSize)
    {
        MovementPattern pattern = (MovementPattern)gameObject.GetComponent(typeof(MovementPattern));

        return pattern.GeneratePossibleMoves(cam, tileSize);
    }
}
