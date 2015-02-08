#define VIRION_DEBUG

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public enum GameState
    {
        draft = 0,
        tactics,
        start
    }
    public GameState currentState = GameState.start;

    public int numCleanTiles = 0;
    public int infectedTileThreshold = 12;
    public int piecesPerSide = 4;

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
    private bool canSelectCard = true;

    private CardHolder humanCardHolder;
    private CardHolder invaderCardHolder;
    private ViableTiles viableTiles;
    private Lerpable board; 

    private int[] snakeDraftCounter;
    private CardFunctions.Team currentDraftTeam;

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

        snakeDraftCounter = new int[2];
        currentDraftTeam = CardFunctions.Team.human;
	}

    public void EnterDraftMode()
    {
        currentState = GameState.draft;
        viableTiles.InfectCenterTile();
    }

    public void EnterTacticsMode()
    {
        currentState = GameState.tactics;

        viableTiles.DisinfectCenterTile();

        humanCardHolder.LerpToSecondaryPos();
        invaderCardHolder.LerpToSecondaryPos();
        Vector3 boardMainPosition = new Vector3(board.transform.position.x,
                                                board.transform.position.y,
                                                board.transform.position.z - 8.0f);

        board.LerpTo(boardMainPosition);
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
            if (piece.tag != currentPlayers[currentPlayerIndex] && selectedPiece)
            {
                TakePiece(piece);
            }
            else
            {
                SelectPiece(piece, cam);
            }
        }
    }

    public void HandleSelectTile(GameObject tile)
    {
        if(currentState == GameState.tactics)
        {
            TileFunctions tf = (TileFunctions)tile.GetComponent(typeof(TileFunctions));

            if(selectedPiece)
            {
                PieceFunctions pf = (PieceFunctions)selectedPiece.GetComponent(typeof(PieceFunctions));
                if (selectedPiece.tag == currentPlayers[currentPlayerIndex] && tf.isSelected && pf.turnsTillMove <= 0)
                {
                    MovePiece(tile);
                }
                else
                {
                    UnselectPiece();
                }
            }
        }
        else
        {
            TileFunctions tf = (TileFunctions)tile.GetComponent(typeof(TileFunctions));

            if(!tf.isOccupied())
                DraftMovePiece(tile);
        }
    }

    private void InitialPlacePiece()
    {
        Vector3 centerPos = viableTiles.centerTile.transform.position;
        Vector3 newPos = new Vector3(centerPos.x, centerPos.y, centerPos.z - 1);
        PieceFunctions pf = LoadPiece(selectedCardFunc.associatedPiece, newPos);
        SelectPiece(pf.gameObject, Camera.main);

        viableTiles.HighlightTeamTiles(((PieceFunctions)selectedPiece.GetComponent(typeof(PieceFunctions))).team);
    }

    public void HandleSelectCard(GameObject card, CardFunctions cf)
    {
        if(!cf.isDrafted && canSelectCard && currentDraftTeam == cf.team)
        { 
            uiManager.EnableDraftUi();
            ShowCard(card, cf);
        }
    }

    public void HandleHitCancelButton() //now called by cancelButton.onClick()
    {
        Destroy(selectedPiece);
        UnselectPiece();
        ResetSelectedCard();
        viableTiles.UnHighlightAllTiles();
    }

    public void HandleHitNothing()
    {
        if(currentState == GameState.tactics)
        {
            UnselectPiece();
        }
    }

    private void ShowCard(GameObject card, CardFunctions cf)
    {
        canSelectCard = false;
        selectedCard = card;
        selectedCardFunc = cf;

        selectedCardFunc.LerpToSecondaryPos();
        humanCardHolder.LerpToSecondaryPos();
        invaderCardHolder.LerpToSecondaryPos();

        board.LerpToSecondaryPos();
        board.ExecWhenFinishedLerp(InitialPlacePiece);
    }

    private void ResetSelectedCard()
    {
        if (selectedCardFunc)
        {
            uiManager.DisableDraftUi();
            selectedCardFunc.LerpToOriginalPos();
            selectedCardFunc.ExecWhenFinishedLerp(SetCanSelectCard);
            humanCardHolder.LerpToOriginalPos();
            invaderCardHolder.LerpToOriginalPos();
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
        PieceFunctions pf = ((PieceFunctions)selectedPiece.GetComponent(typeof(PieceFunctions)));
        Vector3 newPos = new Vector3(tile.transform.position.x, tile.transform.position.y, selectedPiece.transform.position.z);

        pf.LerpTo(newPos, PieceFunctions.LerpSpeed.faster);
        pf.JustMoved();

        UnselectPiece();

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

        selectedCardFunc.DraftCard();

        int otherTeamInt = (int)(currentDraftTeam + 1) % (int)CardFunctions.Team.teamCount; //This is stupid
        int currentTeamInt = (int)currentDraftTeam;
        snakeDraftCounter[currentTeamInt]++;

        if (snakeDraftCounter[currentTeamInt] > snakeDraftCounter[otherTeamInt])
        {
            currentDraftTeam = (CardFunctions.Team)otherTeamInt;
        }

        //TODO: make more flexible
        if(snakeDraftCounter[0] + snakeDraftCounter[1] == piecesPerSide)
        {
            selectedCardFunc.ExecWhenFinishedLerp(EnterTacticsMode);
        }

        HandleHitCancelButton();
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

    public void SetCanSelectCard()
    {
        canSelectCard = true;
    }
}
