﻿using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    public Button confirmButton;
    public Button cancelButton;
    public Button restartButton;
    Vector3 confirmVector3 = new Vector3(3.5f, -4.5f, 0.0f); //coordinates correspond to pixel dimensions
    Vector3 cancelVector3 = new Vector3(-3.5f, -4.5f, 0.0f);
    Vector3 restartVector3 = new Vector3(0, 4.5f, 0.0f);
    Vector3 offScreenVector3 = new Vector3(-800.0f, 0.0f, 0.0f); //use for buttons offscreen & disabled

    public void Init()
    {
        confirmButton.enabled = false;
        cancelButton.enabled = false;
        restartButton.enabled = false;
        confirmButton.transform.position = offScreenVector3;
        cancelButton.transform.position = offScreenVector3;
        restartButton.transform.position = offScreenVector3;
    }

    public void HandleConfirmClick()
    {
        Debug.Log("Confoim");
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
}