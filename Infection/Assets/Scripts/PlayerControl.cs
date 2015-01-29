using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {

    private GameManager gameManager; // GameObject responsible for the management of the game
    private Camera playerCam;
    private bool isPieceSelected;
 
	// Use this for initialization
	void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
        playerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        isPieceSelected = false;
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
        Vector2 tileCoord;

        if (Input.GetMouseButtonDown(0))
        {
            selectionRay = playerCam.ScreenPointToRay(Input.mousePosition);

            //Start that cast!
            if (Physics.Raycast(selectionRay, out hitInfo))
            {
                if (!isPieceSelected)
                {
                    if (hitInfo.collider.gameObject.tag == "GamePiece")
                    {
                        GameObject selectedPiece = hitInfo.collider.gameObject;
                        isPieceSelected = gameManager.SelectPiece(selectedPiece);
                        gameManager.HighlightMovementOptions(GeneratePossibleMoves(selectedPiece));
                    }
                }
                else
                {
                    if (hitInfo.collider.gameObject.tag == "Tile")
                    {
                        Transform colliderTransform = hitInfo.collider.gameObject.transform;
                        isPieceSelected = !gameManager.MovePiece(hitInfo.collider.gameObject);
                    }
                }
            }
        }
    }

    private List<GameObject> GeneratePossibleMoves(GameObject selectedPiece)
    {
        List<GameObject> tileList = new List<GameObject>();
        Vector3 piecePos = selectedPiece.transform.position;
        List<Vector2> pieceDirections = ((PieceFunctions)selectedPiece.GetComponent(typeof(PieceFunctions))).movementDirections; //really ugly, but I don't need to use it again

        int tileSize = 1; //TODO: want to link this stronger.

        foreach (Vector2 movement in pieceDirections)
        {
            Vector3 newPos = piecePos;
            newPos.x += movement.x * tileSize;
            newPos.y += movement.y * tileSize;

            Vector3 direction = newPos - playerCam.transform.position;

            RaycastHit hitInfo;
            if (Physics.Raycast(playerCam.transform.position, direction, out hitInfo, 1000))
            {
                if (hitInfo.collider.gameObject.tag == "Tile")
                {
                    tileList.Add(hitInfo.collider.gameObject);
                }
            }
        }

        return tileList;
    }
}
