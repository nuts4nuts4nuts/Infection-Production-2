using UnityEngine;
using System.Collections;

public class DraftManager : MonoBehaviour
{
    Camera cam;

    GameObject selectedCard;
    CardFunctions selectedCardFunctions;
    Vector3 selectedCardPos;

    Vector3 cardHighlightPos;

	// Use this for initialization
	void Start ()
    {
        cam = Camera.main;
        cardHighlightPos = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z + 5);
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
                        //Pick the card
                    }
                    else
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
