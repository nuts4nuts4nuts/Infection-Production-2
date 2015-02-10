using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    public Button confirmButton;
    public Button cancelButton;
    public Button restartButton;
    public Button endTurnButton;
    public Button pauseButton;
    public Button[] CooldownList = new Button[10]; //Don't try this at home
    public Button[] SecondaryCooldown = new Button[10];

    public Text infectionText;
    public Image infectionFill;

    Vector3 confirmVector3 = new Vector3(3.5f, -4.5f, 0.0f); //coordinates correspond to pixel dimensions
    Vector3 cancelVector3 = new Vector3(-3.5f, -4.5f, 0.0f);
    Vector3 restartVector3 = new Vector3(0, 4.5f, 0.0f);
    Vector3 offScreenVector3 = new Vector3(-800.0f, 0.0f, 0.0f); //use for buttons offscreen & disabled

    Vector3 endTurnVector3 = new Vector3(3.5f, 0.0f, 0.0f);
    Vector3 pauseVector3 = new Vector3(-3.5f, 0.0f, 0.0f);
    Vector3 infectionBarVector3 = new Vector3(-3.5f, 3.0f, 0.0f);

    public void Init()
    {
        restartButton.enabled = false;
        confirmButton.enabled = false;
        cancelButton.enabled = false;
        endTurnButton.enabled = false;
        pauseButton.enabled = false;

        restartButton.transform.position = offScreenVector3;
        confirmButton.transform.position = offScreenVector3;
        cancelButton.transform.position = offScreenVector3;
        endTurnButton.transform.position = offScreenVector3;
        pauseButton.transform.position = offScreenVector3;
        infectionText.transform.position = offScreenVector3;

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
    }

    public void HandlePauseClick()
    {
        Debug.Log("Pause");
    }

    public void EnableDraftUi()
    {
        confirmButton.enabled = true;
        confirmButton.interactable = false;
        cancelButton.enabled = true;
        confirmButton.transform.position = confirmVector3;
        cancelButton.transform.position = cancelVector3;
    }

    public void DisableDraftUi()
    {
        confirmButton.interactable = false;
        cancelButton.enabled = false;
        confirmButton.transform.position = offScreenVector3;
        cancelButton.transform.position = offScreenVector3;
    }

    public void EnableRestartButton(string text)
    {
        restartButton.enabled = true;
        restartButton.transform.position = restartVector3;
        ((Text)restartButton.GetComponentInChildren(typeof(Text))).text = text;
    }

    public void DisableRestartButton()
    {
        restartButton.enabled = false;
        restartButton.transform.position = offScreenVector3;
    }

    public void EnableTacticsUi(int cleanTiles, int totalTiles)
    {
        endTurnButton.enabled = true;
        endTurnButton.transform.position = endTurnVector3;
        pauseButton.enabled = true;
        pauseButton.transform.position = pauseVector3;

        infectionText.transform.position = infectionBarVector3;
        UpdateInfectionFill(cleanTiles, totalTiles);
    }

    public void DisableTacticsUi()
    {
        endTurnButton.enabled = false;
        endTurnButton.transform.position = offScreenVector3;
        pauseButton.enabled = false;

        pauseButton.transform.position = offScreenVector3;
        infectionText.transform.position = offScreenVector3;
    }

    public void UpdateInfectionFill(int cleanTiles, int totalTiles)
    {
        infectionFill.fillAmount = (float)(totalTiles - cleanTiles) / (float)totalTiles;
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
                Vector3 temp = new Vector3(pieces[i].transform.position.x, pieces[i].transform.position.y, pieces[i].transform.position.z - 1);
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
}