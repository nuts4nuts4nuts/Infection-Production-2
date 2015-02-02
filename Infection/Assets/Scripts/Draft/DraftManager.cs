using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DraftManager : MonoBehaviour
{
    private Camera cam;

    private GameObject selectedCard;
    private CardFunctions selectedCardFunctions;
    private Vector3 selectedCardPos;

    private Vector3 cardHighlightPos;

    private int[] snakeDraftCounter;
    private CardFunctions.Team currentTeam;

    private DraftData draftData;

	// Use this for initialization
	void Start ()
    {
        cam = Camera.main;
        cardHighlightPos = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z + 3);

        snakeDraftCounter = new int[2];
        currentTeam = CardFunctions.Team.human;

        GameObject dataHolder = GameObject.Find("DraftDataHolder");
        if(dataHolder != null)
        {
            draftData = (DraftData)dataHolder.GetComponent(typeof(DraftData));
        }
	}

	// Update is called once per frame
	void Update ()
    {
        PollMouseInput();
	}

    private void PollMouseInput()
    {
        Ray selectionRay;
        RaycastHit hitInfo;

        if (Input.GetMouseButtonDown(0))
        {
            selectionRay = cam.ScreenPointToRay(Input.mousePosition);

            //Start that cast!
            if (Physics.Raycast(selectionRay, out hitInfo))
            {
                GameObject hitObj = hitInfo.collider.gameObject;
                CardFunctions cf = (CardFunctions)hitObj.GetComponent(typeof(CardFunctions));

                if(cf != null)
                {
                    if(selectedCard == hitObj)
                    {
                        if(currentTeam == selectedCardFunctions.team)
                        {
                            DraftCard();

                            int otherTeamInt = (int)(currentTeam + 1) % (int)CardFunctions.Team.teamCount; //This is stupid
                            int currentTeamInt = (int)currentTeam;
                            snakeDraftCounter[currentTeamInt]++;

                            if (snakeDraftCounter[currentTeamInt] > snakeDraftCounter[otherTeamInt])
                            {
                                currentTeam = (CardFunctions.Team)otherTeamInt;
                            }
                        }
                        else
                        {
                            ResetSelectedCard();
                        }
                    }
                    else if(!cf.isDrafted)
                    {
                        ShowCard(hitObj, cf);
                    }
                }
                else
                {
                    ResetSelectedCard();
                }
            }

        }
    }

    private void DraftCard()
    {
        if (currentTeam == CardFunctions.Team.human)
        {
            draftData.humanPieces.Add(selectedCardFunctions.associatedPiece);
        }
        else
        {
            draftData.invaderPieces.Add(selectedCardFunctions.associatedPiece);
        }

        selectedCardFunctions.DraftCard();
        ResetSelectedCard();

        if(draftData.humanPieces.Count + draftData.invaderPieces.Count == 4)
        {
            EndDraft();
        }
    }

    private void EndDraft()
    {
        Application.LoadLevel("TacticsScene");
    }

    private void ResetSelectedCard()
    {
        if (selectedCardFunctions)
        {
            selectedCardFunctions.LerpTo(selectedCardPos);
            selectedCardFunctions = null;
            selectedCard = null;
        }
    }

    private void ShowCard(GameObject card, CardFunctions cf)
    {
        ResetSelectedCard();

        selectedCard = card;
        selectedCardFunctions = cf;
        selectedCardPos = card.transform.position;

        selectedCardFunctions.LerpTo(cardHighlightPos);
    }
}
