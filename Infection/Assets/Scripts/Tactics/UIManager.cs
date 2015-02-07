<<<<<<< HEAD
﻿using System.Collections.Generic;
using UnityEditor;
=======
﻿using UnityEditor;
>>>>>>> ca2d131a42cacee9008551f66dc4c83742c78c6d
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

<<<<<<< HEAD
    //buttons on buttons on buttons
    public Button confirmButton;
    public Button cancelButton;
    public Button endTurnButton;
    public Button pauseButton;
    public Button[] CooldownList = new Button[10]; //Don't try this at home
    public Button[] SecondaryCooldown = new Button[10];

    Vector3 confirmVector3 = new Vector3(3.5f, -4.5f, 0.0f); 
    Vector3 cancelVector3 = new Vector3(-3.5f, -4.5f, 0.0f);
    Vector3 endTurnVector3 = new Vector3(4.5f, -2.0f, 0.0f);
    Vector3 pauseVector3 = new Vector3(-4.5f, 2.0f, 0.0f);
=======
    public Button confirmButton;
    public Button cancelButton;
    Vector3 confirmVector3 = new Vector3(3.5f, -4.5f, 0.0f); //coordinates correspond to pixel dimensions
    Vector3 cancelVector3 = new Vector3(-3.5f, -4.5f, 0.0f);
>>>>>>> ca2d131a42cacee9008551f66dc4c83742c78c6d
    Vector3 offScreenVector3 = new Vector3(-800.0f, 0.0f, 0.0f); //use for buttons offscreen & disabled

	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    public void Init()
    {
        confirmButton.enabled = false;
        cancelButton.enabled = false;
<<<<<<< HEAD
        endTurnButton.enabled = false;
        pauseButton.enabled = false;
        confirmButton.transform.position = offScreenVector3;
        cancelButton.transform.position = offScreenVector3;
        endTurnButton.transform.position = offScreenVector3;
        pauseButton.transform.position = offScreenVector3;

        ColorBlock temp = endTurnButton.colors;
        temp.normalColor = Color.blue;
        temp.highlightedColor = Color.red;
        temp.pressedColor = Color.grey;
        endTurnButton.colors = temp;

        for(int i = 0; i < CooldownList.Length; i++)
        {
            CooldownList[i].enabled = false;
            CooldownList[i].transform.position = offScreenVector3;
            SecondaryCooldown[i].enabled = false;
            SecondaryCooldown[i].transform.position = offScreenVector3;
        }
=======
        confirmButton.transform.position = offScreenVector3;
        cancelButton.transform.position = offScreenVector3;
>>>>>>> ca2d131a42cacee9008551f66dc4c83742c78c6d
    }

    public void HandleConfirmClick()
    {
        Debug.Log("Confoim");
    }

<<<<<<< HEAD
    public void HandlePauseClick()
    {
        Debug.Log("Pause");
    }

    public void EnableDraftUi()
    {
        confirmButton.enabled = true;
        cancelButton.enabled = true;
        confirmButton.interactable = true;
=======
    public void EnableDraftUi()
    {
        confirmButton.enabled = true;
        confirmButton.interactable = false;
        cancelButton.enabled = true;
>>>>>>> ca2d131a42cacee9008551f66dc4c83742c78c6d
        confirmButton.transform.position = confirmVector3;
        cancelButton.transform.position = cancelVector3;
    }

    public void DisableDraftUi()
    {
<<<<<<< HEAD
        confirmButton.enabled = false;
        cancelButton.enabled = false;
        confirmButton.interactable = true;
        confirmButton.transform.position = offScreenVector3;
        cancelButton.transform.position = offScreenVector3;
    }

    public void EnableTacticsUi()
    {
        endTurnButton.enabled = true;
        endTurnButton.transform.position = endTurnVector3;
        pauseButton.enabled = true;
        pauseButton.transform.position = pauseVector3;
    }

    public void DisableTacticsUi()
    {
        endTurnButton.enabled = false;
        endTurnButton.transform.position = offScreenVector3;
        pauseButton.enabled = false;
        pauseButton.transform.position = offScreenVector3;
    }

    public void UpdateTurnButtonColor(int player)
    {
        ColorBlock temp = endTurnButton.colors;

        if (player == 0)
        {
            temp.normalColor = Color.blue;
            temp.highlightedColor = Color.red;
            temp.pressedColor = Color.grey;
        }
        else
        {
            temp.normalColor = Color.red;
            temp.highlightedColor = Color.blue;
            temp.pressedColor = Color.grey;
        }

        endTurnButton.colors = temp;
    }

    public void UpdateCooldownUi(GameObject[] pieces)
    {
        for(int j = 0; j < CooldownList.Length; j++)
        {
            SecondaryCooldown[j].transform.position = offScreenVector3;
            SecondaryCooldown[j].GetComponentInChildren<Text>().text = "0";
            SecondaryCooldown[j].transform.position = CooldownList[j].transform.position;
            SecondaryCooldown[j].GetComponentInChildren<Text>().text = CooldownList[j].GetComponentInChildren<Text>().text;
        }


        int i = 0;

        for (; i < pieces.Length; i++)
        {
            PieceFunctions pf = pieces[i].GetComponent<PieceFunctions>();

            if (pf.turnsTillMove > 1)
            {
                Vector3 temp = new Vector3(pieces[i].transform.position.x, pieces[i].transform.position.y, -2);
                CooldownList[i].transform.position = temp;

                CooldownList[i].GetComponentInChildren<Text>().text = (pf.turnsTillMove - 1).ToString();
            }
            else
            {
                CooldownList[i].transform.position = offScreenVector3;
                CooldownList[i].GetComponentInChildren<Text>().text = "0";

            }
        }

        for(; i < CooldownList.Length; i++) //if pieces.length < cooldown.length
        {
            CooldownList[i].transform.position = offScreenVector3;
            CooldownList[i].GetComponentInChildren<Text>().text = "0";
        }
    }
=======
        confirmButton.interactable = false;
        cancelButton.enabled = false;
        confirmButton.transform.position = offScreenVector3;
        cancelButton.transform.position = offScreenVector3;
    }
>>>>>>> ca2d131a42cacee9008551f66dc4c83742c78c6d
}