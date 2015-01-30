using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {

    private int currentPlayer;
    private string[] currentPlayers;
    private int currentPlayerCount;

    private GameManager gameManager; // GameObject responsible for the management of the game
    private Camera playerCam;
    private bool isPieceSelected;

	// Use this for initialization
	void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
        playerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        isPieceSelected = false;

        //Could be more modular later
        currentPlayerCount = 2;
        currentPlayers = new string[currentPlayerCount];

        currentPlayers[0] = "HumanPiece";
        currentPlayers[1] = "InvaderPiece";
        currentPlayer = 0;
	}
	
	// Update is called once per frame
	void Update()
    {
        PollMouseInput();
	}

    private void PollMouseInput()
    {
        Ray selectionRay;
        RaycastHit hitInfo;

        if (Input.GetMouseButtonDown(0))
        {
            selectionRay = playerCam.ScreenPointToRay(Input.mousePosition);

            //Start that cast!
            if (Physics.Raycast(selectionRay, out hitInfo))
            {
                GameObject hitObj = hitInfo.collider.gameObject;
                
                if(hitObj.tag == "EndTurn")
                {
                    EndTurn();
                }
                else if (hitObj.tag == currentPlayers[currentPlayer] && ((PieceFunctions)hitObj.GetComponent(typeof(PieceFunctions))).turnsTillMove <= 0)
                {
                    GameObject selectedPiece = hitInfo.collider.gameObject;
                    gameManager.SelectPiece(selectedPiece, playerCam);
                }
                else if (hitInfo.collider.gameObject.tag == "Tile")
                {
                    if(isPieceSelected)
                    {
                        gameManager.MovePiece(hitInfo.collider.gameObject);
                    }
                }
            }

        }
    }

    private void EndTurn()
    {
        //Go to next player
        currentPlayer = (currentPlayer + 1) % currentPlayerCount;

        //Reactivate new player's pieces
        GameObject[] pieces = GameObject.FindGameObjectsWithTag(currentPlayers[currentPlayer]);

        foreach(GameObject piece in pieces)
        {
            PieceFunctions pf = ((PieceFunctions)piece.GetComponent(typeof(PieceFunctions)));
            int turnsTillMove = pf.TurnPassed();

            if(turnsTillMove <= 0 && pf.isIncubating)
            {
                gameManager.FinishIncubate(piece, playerCam);
            }
        }

        print("Turn Ended!");
    }

    public void SelectPiece()
    {
        isPieceSelected = true;
    }

    public void UnselectPiece()
    {
        isPieceSelected = false;
    }
}
