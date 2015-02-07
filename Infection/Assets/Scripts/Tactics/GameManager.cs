﻿#define VIRION_DEBUG

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public enum GameState
    {
        draft = 0,
        tactics
    }
    public GameState currentState = GameState.draft;

    public int numCleanTiles = 0;
    public int infectedTileThreshold = 12;

    //reference to UIManager script on GameManager
    private UIManager uiManager;

    private GameObject selectedPiece;
    private List<GameObject> possibleMovementTiles;

    private int currentPlayerIndex = 0;
    private string[] currentPlayers;
    private int numPlayers = 2;

    //Draft data
    private GameObject selectedCard;
    private CardFunctions selectedCardFunc;

    private CardHolder humanCardHolder;
    private CardHolder invaderCardHolder;
    private ViableTiles viableTiles;
    private Lerpable board; 

	// Use this for initialization
	void Start ()
    {
        possibleMovementTiles = new List<GameObject>();
        humanCardHolder = (CardHolder)(GameObject.Find("HumanCards")).GetComponent(typeof(CardHolder));
        invaderCardHolder = (CardHolder)(GameObject.Find("InvaderCards")).GetComponent(typeof(CardHolder));
        viableTiles = (ViableTiles)(GameObject.Find("ViableTiles")).GetComponent(typeof(ViableTiles));

        board = (Lerpable)(GameObject.Find("Board").GetComponent(typeof(Lerpable)));

        currentPlayers = new string[numPlayers];
        currentPlayers[0] = "HumanPiece";
        currentPlayers[1] = "InvaderPiece";

        uiManager = gameObject.GetComponent<UIManager>();
        uiManager.Init();

        EnterDraftMode();
	}

    public void EnterDraftMode()
    {
        currentState = GameState.draft;

        GameObject endTurn = GameObject.Find("EndTurn");
        endTurn.SetActive(false);
    }

    public void EnterTacticsMode()
    {
        currentState = GameState.tactics;

        GameObject endTurn = GameObject.Find("EndTurn");
        endTurn.SetActive(true);
    }

    private PieceFunctions LoadPiece(string pieceName, Vector3 pos)
    {
        string name = "Prefabs/";
        name += pieceName;
        GameObject piece = (GameObject)Instantiate(Resources.Load(name));
        piece.transform.parent = board.gameObject.transform;
        piece.transform.position = pos;
        PieceFunctions pf = (PieceFunctions)piece.GetComponent(typeof(PieceFunctions));

        return pf;
    }

    public void HandleEndTurn(EndFunctions ef, Camera cam)
    { 
        //Go to next player
        currentPlayerIndex = (currentPlayerIndex + 1) % numPlayers;

        if (currentPlayerIndex == 0)
        {
            ef.SetColor(Color.blue);
        }
        else if (currentPlayerIndex == 1)
        {
            ef.SetColor(Color.red);
        }

        //Reactivate new player's pieces
        GameObject[] pieces = GetPlayerPieces(currentPlayers[currentPlayerIndex]);

        foreach(GameObject piece in pieces)
        {
            PieceFunctions pf = ((PieceFunctions)piece.GetComponent(typeof(PieceFunctions)));

#if VIRION_DEBUG
            if(pf == null)
            {
                print("unity is dumb"); //sure is
            }
            else
#endif
            {
                int turnsTillMove = pf.TurnPassed();

                if (turnsTillMove <= 0 && pf.isIncubating)
                {
                    FinishIncubate(piece, cam);
                }
            }

        }
#if VIRION_DEBUG
        print("Turn Ended!");
#endif
    }

    public void HandleSelectPiece(GameObject piece, PieceFunctions pf, Camera cam)
    {
        if (currentState == GameState.tactics)
        {
            if (piece.tag == currentPlayers[currentPlayerIndex] && pf.turnsTillMove <= 0)
            {
                SelectPiece(piece, cam);
            }

            if (piece.tag != currentPlayers[currentPlayerIndex])
            {
                TakePiece(piece);
            }
        }
    }

    public void HandleSelectTile(GameObject tile)
    {
        if(currentState == GameState.tactics)
        {
            if (selectedPiece)
            {
                MovePiece(tile);
            }
        }
        else
        {
            TileFunctions tf = (TileFunctions)tile.GetComponent(typeof(TileFunctions));

            if(!tf.isOccupied())
                DraftMovePiece(tile);
        }
    }

    private void PlacePiece(Vector3 pos)
    {
        Vector3 newPos = new Vector3(pos.x, pos.y, pos.z - 1);
        PieceFunctions pf = LoadPiece(selectedCardFunc.associatedPiece, newPos);
        SelectPiece(pf.gameObject, Camera.main);

        viableTiles.HighlightTeamTiles(((PieceFunctions)selectedPiece.GetComponent(typeof(PieceFunctions))).team);
    }

    public void HandleSelectCard(GameObject card, CardFunctions cf)
    {
        if(selectedCard != card && !cf.isDrafted)
        { 
            uiManager.EnableDraftUi();
            ShowCard(card, cf);
        }
    }

    public void HandleHitNothing() //now called by cancelButton.onClick()
    {
        Destroy(selectedPiece);
        UnselectPiece();
        ResetSelectedCard();
        viableTiles.UnHighlightAllTiles();
    }

    private void ShowCard(GameObject card, CardFunctions cf)
    {
        ResetSelectedCard();

        selectedCard = card;
        selectedCardFunc = cf;

        humanCardHolder.LerpToSecondaryPos();
        invaderCardHolder.LerpToSecondaryPos();
        selectedCardFunc.LerpToSecondaryPos();

        board.LerpToSecondaryPos();
        board.ExecWhenFinishedLerp(PlacePiece, board.secondaryPosition);
    }

    private void ResetSelectedCard()
    {
        if (selectedCardFunc)
        {
            uiManager.DisableDraftUi();
            humanCardHolder.LerpToOriginalPos();
            invaderCardHolder.LerpToOriginalPos();
            selectedCardFunc.LerpToOriginalPos();
            board.LerpToOriginalPos();

            selectedCardFunc = null;
            selectedCard = null;
        }
    }

    public void SelectPiece(GameObject newPiece, Camera playerCam)
    {
        if(newPiece != selectedPiece)
        {
            UnselectPiece();

            selectedPiece = newPiece;

            //Highlight that joint!
            selectedPiece.renderer.material.color = Color.magenta;
            HighlightMovementOptions(GeneratePossibleMoves(selectedPiece, playerCam));
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

        //enable Incubation counter here

        UnselectPiece();
    }

    public void FinishIncubate(GameObject piece, Camera camera)
    {
        PieceFunctions pf = (PieceFunctions)piece.GetComponent(typeof(PieceFunctions));
        pf.FinishIncubate();

        //disable incubation counter here

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

        TestInfectionLevel();
    }
    
    public void DraftMovePiece(GameObject tile)
    {
        TileFunctions tf = (TileFunctions)tile.GetComponent(typeof(TileFunctions));
        PieceFunctions pf = ((PieceFunctions)selectedPiece.GetComponent(typeof(PieceFunctions)));
        if(pf)
        { 
            Vector3 newPos = new Vector3(tile.transform.position.x, tile.transform.position.y, selectedPiece.transform.position.z);

            pf.transform.position = newPos;
            UnselectPiece();
            SelectPiece(pf.gameObject, Camera.main);

            if(tf.isSelected)
            {
                uiManager.confirmButton.interactable = true;
            }
            else
            {
                uiManager.confirmButton.interactable = false;
            }
            
        }
    }

    public void ConfirmPiecePlacement()
    {
        UnselectPiece();
        HandleHitNothing();
    }

    private void TestInfectionLevel()
    {
#if VIRION_DEBUG
        if(numCleanTiles <= infectedTileThreshold)
        {
            print("INVADERS WIN!");
        }
#endif
    }

    private void UnselectPiece()
    {
        if(selectedPiece)
        {
            PieceFunctions pf = (PieceFunctions)selectedPiece.GetComponent(typeof(PieceFunctions));

            pf.ResetToOriginalColor();
            selectedPiece = null;
            UnHighlightMovementOptions();
        }
    }

    public void HighlightMovementOptions(List<GameObject> tiles)
    {
        possibleMovementTiles = tiles;

        foreach (GameObject obj in tiles)
        {
            Lerpable objFunc = (Lerpable)obj.GetComponent(typeof(Lerpable));
            objFunc.SetColorTemp(Color.black);

            if (currentState == GameState.tactics)
            {
                objFunc.isSelected = true;
            }
        }
    }

    private void UnHighlightMovementOptions()
    {
        foreach (GameObject obj in possibleMovementTiles)
        {
            Lerpable ef = (Lerpable)obj.GetComponent(typeof(Lerpable));
            ef.ResetToCurrentColor();

            if (currentState == GameState.tactics)
            {
                ef.isSelected = false;
            }
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
            TileFunctions tile = pf.GetTile();
            tile.isSelected = true;

            MovePiece(tile.gameObject);
            Destroy(toTake);

            //TODO: Should not be hardcoded
            GameObject[] pieces = GetPlayerPieces("InvaderPiece");

            if(pieces.Length <= 1)
            {
#if VIRION_DEBUG
                print("HUMANS WIN!");
#endif
            }
        }
        else
        {
            UnselectPiece();
        }
    }

    public GameObject[] GetPlayerPieces(string playerName)
    {
        GameObject[] pieces = GameObject.FindGameObjectsWithTag(playerName);
        return pieces;
    }
}
