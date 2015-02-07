﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ViableTiles : MonoBehaviour
{
    public List<GameObject> humanTiles;
    public List<GameObject> invaderTiles;

    private List<TileFunctions> humanFuncs;
    private List<TileFunctions> invaderFuncs;

    void Start()
    {

        humanFuncs = new List<TileFunctions>();
        invaderFuncs = new List<TileFunctions>();

        foreach( GameObject obj in humanTiles)
        {
            TileFunctions tf = (TileFunctions)obj.GetComponent(typeof(TileFunctions));
            if(tf)
            {
                humanFuncs.Add(tf);
            }
        }

        foreach( GameObject obj in invaderTiles)
        {
            TileFunctions tf = (TileFunctions)obj.GetComponent(typeof(TileFunctions));
            if(tf)
            {
                invaderFuncs.Add(tf);
            }
        }
    }

    public void HighlightTeamTiles(PieceFunctions.Team team)
    {
        if(team == PieceFunctions.Team.human)
        {
            HighlightHumanTiles();
        }
        else
        {
            HighlightInvaderTiles();
        }
    }

    private void HighlightHumanTiles()
    {
        foreach( TileFunctions tile in humanFuncs)
        {
            //TODO: make this more robust
            if(!tile.isOccupied())
            {
                tile.SetColorCurrent(Color.cyan);
                tile.isSelected = true;
            }
        }
    }

    private void HighlightInvaderTiles()
    { 
        foreach( TileFunctions tile in invaderFuncs)
        {
            //TODO: make this more robust
            if(!tile.isOccupied())
            {
                tile.SetColorCurrent(Color.cyan);
                tile.isSelected = true;
            }
        }
    }

    public void UnHighlightAllTiles()
    {
        foreach( TileFunctions tile in humanFuncs)
        {
            tile.ResetToOriginalColor();
            tile.isSelected = false;
        }
        foreach( TileFunctions tile in invaderFuncs)
        {
            tile.ResetToOriginalColor();
            tile.isSelected = true;
        }

    }
}
