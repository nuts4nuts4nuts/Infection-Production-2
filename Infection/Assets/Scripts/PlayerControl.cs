using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    private GameManager gameManager; // GameObject responsible for the management of the game
    private Camera camera;
    private bool isPieceSelected;
 
	// Use this for initialization
	void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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
            selectionRay = camera.ScreenPointToRay(Input.mousePosition);

            //Start that cast!
            if (Physics.Raycast(selectionRay, out hitInfo))
            {
                if (!isPieceSelected)
                {
                    if (hitInfo.collider.gameObject.tag == "GamePiece")
                    {
                        isPieceSelected = gameManager.SelectPiece(hitInfo.collider.gameObject);
                    }
                }
                else
                {
                    if (hitInfo.collider.gameObject.tag == "Tile")
                    {
                        Transform colliderTransform = hitInfo.collider.gameObject.transform;
                        tileCoord = new Vector2(colliderTransform.position.x, colliderTransform.position.y);
                        isPieceSelected = !gameManager.MovePiece(tileCoord);
                    }
                }
            }
        }
    }
}
