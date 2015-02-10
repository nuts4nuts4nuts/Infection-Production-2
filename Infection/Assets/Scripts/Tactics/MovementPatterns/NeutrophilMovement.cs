using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NeutrophilMovement : MovementPattern 
{
    public override List<GameObject> GeneratePossibleMoves(Camera cam, int tileSize)
    {
        PieceFunctions pf = (PieceFunctions)gameObject.GetComponent(typeof(PieceFunctions));
        List<Vector2> directions = new List<Vector2>(pf.movementDirections);

        Vector3 piecePos = gameObject.transform.position;

        for (int i = -2; i < 3; i++) //manual input of non-adjacent coordinates
        {   
            float x = piecePos.x + i; //each tile is about 1 unit apart
            Vector2 temp = new Vector2(x, 0); //x pos is set for each loop pass

            switch (i)
            {
                case -2:
                    temp.y = piecePos.y;
                    directions.Add(temp); //adds tile at (-2, 0) from piece
                    break;

                case -1:
                    temp.y = piecePos.y - 1;
                    directions.Add(temp); //adds tile at (-1, 1) from piece

                    temp.y = piecePos.y + 1;
                    directions.Add(temp); //adds tile at (-1, -1) from piece
                    break;

                case 0:
                    temp.y = piecePos.y - 2;
                    directions.Add(temp); // (0, -2)

                    temp.y = piecePos.y + 2;
                    directions.Add(temp); // (0, 2)
                    break;

                case 1:
                    temp.y = piecePos.y - 1;
                    directions.Add(temp); // (1, -1)

                    temp.y = piecePos.y + 1;
                    directions.Add(temp); // (1, 1)
                    break;

                case 2:
                    temp.y = piecePos.y;
                    directions.Add(temp); // (2, 0)
                    break;
            }
        }

        //raycast logic

        List<GameObject> tileList = new List<GameObject>();

        foreach (Vector2 movement in directions)
        {
            Vector3 newPos = piecePos;
            newPos.x += movement.x * tileSize;
            newPos.y += movement.y * pf.ySign * tileSize;

            Vector3 direction = newPos - cam.transform.position;

            RaycastHit hitInfo;
            if (Physics.Raycast(cam.transform.position, direction, out hitInfo, 1000))
            {
                PieceFunctions hitPieceFunctions = (PieceFunctions)hitInfo.collider.gameObject.GetComponent(typeof(PieceFunctions));

                if (hitInfo.collider.gameObject.tag == "Tile")
                {
                    tileList.Add(hitInfo.collider.gameObject);
                }
                else if (hitPieceFunctions)
                {
                    bool teamsRight = pf.team == PieceFunctions.Team.human && hitPieceFunctions.team == PieceFunctions.Team.invader;

                    if (teamsRight)
                    {
                        tileList.Add(hitInfo.collider.gameObject);
                    }
                }
            }
        }

        return tileList;
    }
}