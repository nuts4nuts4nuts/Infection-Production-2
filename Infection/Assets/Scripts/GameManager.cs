using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

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
            selectedPiece.renderer.material.color = Color.magenta;

            return true;
        }

        return false;
    }

    public bool MovePiece(GameObject tile) 
    {
        selectedPiece.renderer.material.color = pieceOldColor;

        if(((TileFunctions)tile.GetComponent(typeof(TileFunctions))).isSelected)
        {
            Vector3 newPos = new Vector3(tile.transform.position.x, tile.transform.position.y, selectedPiece.transform.position.z);
            selectedPiece.transform.position = newPos;
            PieceFunctions pf = ((PieceFunctions)selectedPiece.GetComponent(typeof(PieceFunctions)));
            pf.turnsTillMove = pf.cooldown;

            selectedPiece = null;
            UnHighlightMovementOptions();
            return true;
        }

        selectedPiece = null;
        UnHighlightMovementOptions();
        return false;
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
