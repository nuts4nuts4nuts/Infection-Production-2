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
        playerCam = Camera.main;
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
                    EndFunctions ef = (EndFunctions)hitObj.GetComponent(typeof(EndFunctions));
                    EndTurn(ef);
                }
                else if (hitObj.tag == currentPlayers[currentPlayer] && ((PieceFunctions)hitObj.GetComponent(typeof(PieceFunctions))).turnsTillMove <= 0)
                {
                    GameObject selectedPiece = hitObj;
                    gameManager.SelectPiece(selectedPiece, playerCam);
                }
                else if (hitObj.tag == "Tile")
                {
                    if(isPieceSelected)
                    {
                        gameManager.MovePiece(hitObj);
                    }
                }
                else if (hitObj.tag == currentPlayers[(currentPlayer + 1) % 2]) //TODO: This does not support more than 2 players
                {
                    gameManager.TakePiece(hitObj);
                }
            }

        }
    }

    private void EndTurn(EndFunctions endFunctions)
    {
        //Go to next player
        currentPlayer = (currentPlayer + 1) % currentPlayerCount;

        if (currentPlayer == 0)
        {
            endFunctions.SetColor(Color.blue);
        }
        else if (currentPlayer == 1)
        {
            endFunctions.SetColor(Color.red);
        }

        //Reactivate new player's pieces
        GameObject[] pieces = gameManager.GetPlayerPieces(currentPlayers[currentPlayer]);

        foreach(GameObject piece in pieces)
        {
            PieceFunctions pf = ((PieceFunctions)piece.GetComponent(typeof(PieceFunctions)));

#if VIRION_DEBUG
            if(pf == null)
            {
                print("unity is dumb");
            }
            else
#endif
            {
                int turnsTillMove = pf.TurnPassed();

                if (turnsTillMove <= 0 && pf.isIncubating)
                {
                    gameManager.FinishIncubate(piece, playerCam);
                }
            }

        }
#if VIRION_DEBUG
        print("Turn Ended!");
#endif
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
