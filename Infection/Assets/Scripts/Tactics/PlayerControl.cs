using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {

<<<<<<< HEAD
    private int currentPlayer;
    private string[] currentPlayers;
    private int currentPlayerCount;

    UIManager uiManager;

=======
>>>>>>> ca2d131a42cacee9008551f66dc4c83742c78c6d
    private GameManager gameManager; // GameObject responsible for the management of the game
    private Camera playerCam;

	// Use this for initialization
	void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
        playerCam = Camera.main;
<<<<<<< HEAD
        isPieceSelected = false;

        uiManager = gameManager.GetComponent<UIManager>();

        //Could be more modular later
        currentPlayerCount = 2;
        currentPlayers = new string[currentPlayerCount];

        currentPlayers[0] = "HumanPiece";
        currentPlayers[1] = "InvaderPiece";
        currentPlayer = 0;
=======
>>>>>>> ca2d131a42cacee9008551f66dc4c83742c78c6d
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
                PieceFunctions pf = (PieceFunctions)hitObj.GetComponent(typeof(PieceFunctions));
                CardFunctions cf = (CardFunctions)hitObj.GetComponent(typeof(CardFunctions));
                
                if(hitObj.tag == "EndTurn")
                {
<<<<<<< HEAD
                    //EndFunctions ef = (EndFunctions)hitObj.GetComponent(typeof(EndFunctions));
                    //EndTurn(ef);
=======
                    EndFunctions ef = (EndFunctions)hitObj.GetComponent(typeof(EndFunctions));

                    gameManager.HandleEndTurn(ef, playerCam);
>>>>>>> ca2d131a42cacee9008551f66dc4c83742c78c6d
                }
                else if(pf != null)
                {
                    gameManager.HandleSelectPiece(hitObj, pf, playerCam);
                }
                else if (hitObj.tag == "Tile")
                {
                    gameManager.HandleSelectTile(hitObj);
                }
                else if(cf != null)
                {
                    gameManager.HandleSelectCard(hitObj, cf);
                }
            }
<<<<<<< HEAD

        }
    }

    public void EndTurn() //overloaded function called by UI
    {
        Debug.Log("End Button");

        //update Cooldown UI
        GameObject[] oldPieces = gameManager.GetPlayerPieces(currentPlayers[currentPlayer]);
        uiManager.UpdateCooldownUi(oldPieces);

        //Go to next player
        currentPlayer = (currentPlayer + 1) % currentPlayerCount;
        uiManager.UpdateTurnButtonColor(currentPlayer);

        //Reactivate new player's pieces
        GameObject[] pieces = gameManager.GetPlayerPieces(currentPlayers[currentPlayer]);
        
        foreach (GameObject piece in pieces)
        {
            PieceFunctions pf = ((PieceFunctions)piece.GetComponent(typeof(PieceFunctions)));

#if VIRION_DEBUG
            if(pf == null)
            {
                print("unity is dumb");
            }
=======
>>>>>>> ca2d131a42cacee9008551f66dc4c83742c78c6d
            else
            {
                //gameManager.HandleHitNothing();
            }
        }
<<<<<<< HEAD
    }

    public void SelectPiece()
    {
        isPieceSelected = true;
    }

    public void UnselectPiece()
    {
        isPieceSelected = false;
=======
>>>>>>> ca2d131a42cacee9008551f66dc4c83742c78c6d
    }
}
