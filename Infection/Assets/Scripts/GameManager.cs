using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    // 0 = Player1's turn, 1 = Player2's turn, 2+ pause, menus, etc?
    public int currentState = 0;
    private GameObject selectedPiece;
    private Color pieceOldColor;

	// Use this for initialization
	void Start ()
    {
        print("hello world!");
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

    public bool MovePiece(Vector2 newPos) 
    {
        selectedPiece.transform.position = newPos;
        selectedPiece.renderer.material.color = pieceOldColor;
        selectedPiece = null;

        return true;
    }

}
