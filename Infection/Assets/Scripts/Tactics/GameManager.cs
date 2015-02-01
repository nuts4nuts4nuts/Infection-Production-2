#define VIRION_DEBUG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    private GameObject selectedPiece;
    private List<GameObject> possibleMovementTiles;

    private List<GameObject> originalPieces;

	// Use this for initialization
	void Start ()
    {
#if VIRION_DEBUG
        print("hello world!");
#endif
        possibleMovementTiles = new List<GameObject>();
        originalPieces = new List<GameObject>();

        LoadOriginalPieces();
	}

    private void LoadOriginalPieces()
    {
        GameObject humanPiece2 = (GameObject)Instantiate(Resources.Load("Prefabs/Humans/DefaultHuman"));
        GameObject humanPiece = (GameObject)Instantiate(Resources.Load("Prefabs/Humans/HumanHorse"));
        GameObject humanPiece3 = (GameObject)Instantiate(Resources.Load("Prefabs/Humans/HumanBishop"));

        humanPiece.transform.position = new Vector3(-0.32355530f, -3.61685f, -1);
        humanPiece2.transform.position = new Vector3(0.6764446f, -3.61685f, -1);
        humanPiece3.transform.position = new Vector3(1.6764446f, -3.61685f, -1);

        GameObject invaderPiece = (GameObject)Instantiate(Resources.Load("Prefabs/Invaders/InvaderKing"));
        GameObject invaderPiece2 = (GameObject)Instantiate(Resources.Load("Prefabs/Invaders/DefaultInvader"));
        GameObject invaderPiece3 = (GameObject)Instantiate(Resources.Load("Prefabs/Invaders/InvaderAggroKnight"));

        invaderPiece.transform.position = new Vector3(-0.32355530f, 4.38315f, -1);
        invaderPiece2.transform.position = new Vector3(0.6764446f, 4.38315f, -1);
        invaderPiece3.transform.position = new Vector3(1.6764446f, 4.38315f, -1);
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
        else if( ((PieceFunctions)newPiece.GetComponent(typeof(PieceFunctions))).team == PieceFunctions.Team.invader)
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
            newPieceFuncs.LerpTo(newPos, PieceFunctions.LerpSpeed.med);
        }
#if VIRION_DEBUG
        else
        {
            print("Incubation Blocked!");
        }
#endif
    }

    public void MovePiece(GameObject tile) 
    {
        TileFunctions tf = (TileFunctions)tile.GetComponent(typeof(TileFunctions));

        if (tf.isSelected)
        {
            PieceFunctions pf = ((PieceFunctions)selectedPiece.GetComponent(typeof(PieceFunctions)));
            Vector3 newPos = new Vector3(tile.transform.position.x, tile.transform.position.y, selectedPiece.transform.position.z);

            pf.LerpTo(newPos, PieceFunctions.LerpSpeed.faster);
            pf.JustMoved();

            UnselectPiece();
        }
        else
        {
            UnselectPiece();
        }
    }

    private void UnselectPiece()
    {
        if(selectedPiece)
        {
            PieceFunctions pf = (PieceFunctions)selectedPiece.GetComponent(typeof(PieceFunctions));

            pf.ResetColor();
            selectedPiece = null;
            UnHighlightMovementOptions();

            PlayerControl pc = gameObject.GetComponent<PlayerControl>();
            pc.UnselectPiece();
        }
    }

    public void HighlightMovementOptions(List<GameObject> tiles)
    {
        possibleMovementTiles = tiles;

        foreach (GameObject obj in tiles)
        {
            obj.renderer.material.color = Color.black;

            ((EntityFunctions)obj.GetComponent(typeof(EntityFunctions))).isSelected = true;
        }
    }

    private void UnHighlightMovementOptions()
    {
        foreach (GameObject obj in possibleMovementTiles)
        {
            EntityFunctions ef = (EntityFunctions)obj.GetComponent(typeof(EntityFunctions));
            ef.ResetColor();
            ef.isSelected = false;
        }

        possibleMovementTiles.Clear();
    }

    private List<GameObject> GeneratePossibleMoves(GameObject selectedPiece, Camera playerCam, List<Vector2> directions = null)
    {
        List<GameObject> tileList = new List<GameObject>();
        Vector3 piecePos = selectedPiece.transform.position;

        List<Vector2> pieceDirections;
        int ySign = 1;

        if(directions == null)
        {
            PieceFunctions pf = (PieceFunctions)selectedPiece.GetComponent(typeof(PieceFunctions));
            pieceDirections = pf.movementDirections;
            ySign = pf.ySign;
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
            newPos.y += movement.y * ySign * tileSize;

            Vector3 direction = newPos - playerCam.transform.position;

            RaycastHit hitInfo;
            if (Physics.Raycast(playerCam.transform.position, direction, out hitInfo, 1000))
            {
                PieceFunctions selectedPieceFunctions = (PieceFunctions)selectedPiece.GetComponent(typeof(PieceFunctions));
                PieceFunctions hitPieceFunctions = (PieceFunctions)hitInfo.collider.gameObject.GetComponent(typeof(PieceFunctions));

                bool nullCheck = selectedPieceFunctions && hitPieceFunctions;

                if (hitInfo.collider.gameObject.tag == "Tile")
                {
                    tileList.Add(hitInfo.collider.gameObject);
                }
                else if (nullCheck)
                {
                    bool teamsRight = selectedPieceFunctions.team == PieceFunctions.Team.human && hitPieceFunctions.team == PieceFunctions.Team.invader;

                    if(teamsRight)
                    {
                        tileList.Add(hitInfo.collider.gameObject);
                    }
                }
            }
        }

        return tileList;
    }

    public void TakePiece(GameObject toTake)
    {
        PieceFunctions pf = (PieceFunctions)toTake.GetComponent(typeof(PieceFunctions));
        if(pf.isSelected)
        {
            GameObject tile = pf.GetTile();
            TileFunctions tf = (TileFunctions)tile.GetComponent(typeof(TileFunctions));
            tf.isSelected = true;

            MovePiece(tile);
            Destroy(toTake);
        }
        else
        {
            UnselectPiece();
        }
    }
}
