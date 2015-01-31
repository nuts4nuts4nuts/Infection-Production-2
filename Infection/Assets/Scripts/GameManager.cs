﻿#define VIRION_DEBUG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    private GameObject selectedPiece;
    private List<GameObject> possibleMovementTiles;

	// Use this for initialization
	void Start ()
    {
#if VIRION_DEBUG
        print("hello world!");
#endif
        possibleMovementTiles = new List<GameObject>();
	}

    public void SelectPiece(GameObject newPiece, Camera playerCam)
    {
        if(newPiece != selectedPiece)
        {
            selectedPiece = newPiece;

            //Highlight that joint!
            selectedPiece.renderer.material.color = Color.magenta;
            HighlightMovementOptions(GeneratePossibleMoves(selectedPiece, playerCam));

            PlayerControl pc = gameObject.GetComponent<PlayerControl>();
            pc.SelectPiece();
        }
        else
        {
            StartIncubate();
        }
    }

    private void StartIncubate()
    {
        PieceFunctions pf = ((PieceFunctions)selectedPiece.GetComponent(typeof(PieceFunctions)));
        pf.StartIncubate();

        UnselectPiece();
    }

    public void FinishIncubate(GameObject piece, Camera camera)
    {
        PieceFunctions pf = (PieceFunctions)piece.GetComponent(typeof(PieceFunctions));
        pf.FinishIncubate();

        List<Vector2> directions = new List<Vector2>();
        Vector2 up = new Vector2(0, 1);
        Vector2 down = new Vector2(0, -1);
        Vector2 right = new Vector2(1, 0);
        Vector2 left = new Vector2(-1, 0);


        //TODO: make this work with the piece-wise forward stuff
        directions.Add(right);
        directions.Add(left);
        directions.Add(down);
        directions.Add(up);

        List<GameObject> tiles = GeneratePossibleMoves(piece, camera, directions);

        if(tiles.Count > 0)
        {
            GameObject newPiece = (GameObject)(Object.Instantiate(piece));

            PieceFunctions newPieceFuncs = ((PieceFunctions)newPiece.GetComponent(typeof(PieceFunctions)));
            Vector3 newPos = new Vector3(tiles[0].transform.position.x, tiles[0].transform.position.y, piece.transform.position.z);
            newPieceFuncs.LerpTo(newPos);
            //newPiece.transform.position = newPos;
        }
        else
        {
#if VIRION_DEBUG
            print("Incubation Blocked!");
#endif
        }
    }

    public void MovePiece(GameObject tile) 
    {
        TileFunctions tf = (TileFunctions)tile.GetComponent(typeof(TileFunctions));

        if (tf.isSelected)
        {
            Vector3 newPos = new Vector3(tile.transform.position.x, tile.transform.position.y, selectedPiece.transform.position.z);
            selectedPiece.transform.position = newPos;
            PieceFunctions pf = ((PieceFunctions)selectedPiece.GetComponent(typeof(PieceFunctions)));
            pf.JustMoved();

            if (selectedPiece.tag == "InvaderPiece")
            {
                tf.InfectTile();
            }

            UnselectPiece();
        }
        else
        {
            UnselectPiece();
        }
    }

    private void UnselectPiece()
    {
        PieceFunctions pf = (PieceFunctions)selectedPiece.GetComponent(typeof(PieceFunctions));
        pf.ResetColor();
        selectedPiece = null;
        UnHighlightMovementOptions();

        PlayerControl pc = gameObject.GetComponent<PlayerControl>();
        pc.UnselectPiece();
    }

    public void HighlightMovementOptions(List<GameObject> tiles)
    {
        possibleMovementTiles = tiles;

        foreach (GameObject obj in tiles)
        {
            obj.renderer.material.color = Color.black;

            if (obj.tag == "Tile")
            {
                ((TileFunctions)obj.GetComponent(typeof(TileFunctions))).isSelected = true;
            }
        }
    }

    private void UnHighlightMovementOptions()
    {
        foreach (GameObject obj in possibleMovementTiles)
        {
            if (obj.tag == "Tile")
            {
                TileFunctions tf = (TileFunctions)obj.GetComponent(typeof(TileFunctions));
                tf.ResetColor();
                tf.isSelected = false;
            }
            else if (obj.tag == "InvaderPiece")
            {
                PieceFunctions pf = (PieceFunctions)obj.GetComponent(typeof(PieceFunctions));
                pf.ResetColor();
            }
            
        }

        possibleMovementTiles.Clear();
    }

    private List<GameObject> GeneratePossibleMoves(GameObject selectedPiece, Camera playerCam, List<Vector2> directions = null)
    {
        List<GameObject> tileList = new List<GameObject>();
        Vector3 piecePos = selectedPiece.transform.position;

        List<Vector2> pieceDirections;

        if(directions == null)
        {
            pieceDirections = ((PieceFunctions)selectedPiece.GetComponent(typeof(PieceFunctions))).movementDirections; //really ugly, but I don't need to use it again
        }
        else
        {
            pieceDirections = directions;
        }

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
                else if (selectedPiece.tag == "HumanPiece" && hitInfo.collider.gameObject.tag == "InvaderPiece")
                {
                    tileList.Add(hitInfo.collider.gameObject);
                }
            }
        }

        return tileList;
    }

    public void TakePiece(GameObject toTake)
    {
        PieceFunctions pf = (PieceFunctions)toTake.GetComponent(typeof(PieceFunctions));
        GameObject tile = pf.GetTile();
        TileFunctions tf = (TileFunctions)tile.GetComponent(typeof(TileFunctions));
        tf.isSelected = true;

        MovePiece(tile);
        Destroy(toTake);
    }
}
