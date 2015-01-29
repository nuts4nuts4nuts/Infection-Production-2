using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    // 0 = Player1's turn, 1 = Player2's turn, 2+ pause, menus, etc?
    public int currentState = 0;
    private GameObject selectedPiece;
    private List<GameObject> possibleMovementTiles;
    private Color tileOldColor;
    private Color pieceOldColor;

	// Use this for initialization
	void Start ()
    {
        print("hello world!");

        possibleMovementTiles = new List<GameObject>();
	}

    void Update()
    {
    }

    public bool SelectPiece(GameObject newPiece)
    {
        selectedPiece = newPiece;

        if (selectedPiece)
        {
            pieceOldColor = selectedPiece.renderer.material.color;

            //Highlight that joint!
            selectedPiece.renderer.material.color = Color.blue;

            return true;
        }

        return false;
    }

    public bool MovePiece(GameObject tile) 
    {
        if(((TileFunctions)tile.GetComponent(typeof(TileFunctions))).isSelected)
        {
            Vector3 newPos = new Vector3(tile.transform.position.x, tile.transform.position.y, selectedPiece.transform.position.z);
            selectedPiece.transform.position = newPos;
        }

        selectedPiece.renderer.material.color = pieceOldColor;
        selectedPiece = null;
        UnHighlightMovementOptions();

        return true;
    }

    public void HighlightMovementOptions(List<GameObject> tiles)
    {
        possibleMovementTiles = tiles;

        foreach (GameObject tile in tiles)
        {
            tileOldColor = tile.renderer.material.color; //slow and redundant, but fast to write.
            tile.renderer.material.color = Color.black;
            ((TileFunctions)tile.GetComponent(typeof(TileFunctions))).isSelected = true;
        }
    }

    private void UnHighlightMovementOptions()
    {
        foreach (GameObject tile in possibleMovementTiles)
        {
            tile.renderer.material.color = tileOldColor;
            ((TileFunctions)tile.GetComponent(typeof(TileFunctions))).isSelected = false;
        }

        possibleMovementTiles.Clear();
    }
}
