using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {

    private GameManager gameManager; // GameObject responsible for the management of the game
    private Camera playerCam;

	// Use this for initialization
	void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
        playerCam = Camera.main;
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
                    EndFunctions ef = (EndFunctions)hitObj.GetComponent(typeof(EndFunctions));

                    gameManager.HandleEndTurn(ef, playerCam);
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
            else
            {
                //gameManager.HandleHitNothing();
            }
        }
    }
}
