using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    public Button confirmButton;
    public Button cancelButton;

	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    public void Init()
    {
        Debug.Log("manager.init()");
        confirmButton.enabled = false;
        cancelButton.enabled = false;
    }

    public void HandleConfirmClick()
    {
        Debug.Log("Confoim");
    }

    public void HandleCancelClick()
    {
        Debug.Log("Nah");
    }

    public void EnableDraftUI()
    {
        confirmButton.enabled = true;
        cancelButton.enabled = true;
    }

    public void DisableDraftUI()
    {
        confirmButton.enabled = false;
        cancelButton.enabled = false;
    }
}