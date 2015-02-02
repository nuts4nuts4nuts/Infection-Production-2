using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DraftData : MonoBehaviour
{
    [HideInInspector]
    public List<string> humanPieces;
    public List<string> invaderPieces;

    [HideInInspector]
    public int currentPlayer = 0;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

	// Use this for initialization
	void Start ()
    {
        humanPieces = new List<string>();
        invaderPieces = new List<string>();
	}
}