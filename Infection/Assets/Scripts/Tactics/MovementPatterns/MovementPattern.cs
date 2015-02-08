using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementPattern : MonoBehaviour
{
    public virtual List<GameObject> GeneratePossibleMoves(Camera cam, int tileSize)
    {
        PieceFunctions pf = (PieceFunctions)gameObject.GetComponent(typeof(PieceFunctions));
        List<Vector2> directions = pf.movementDirections;

        List<GameObject> tileList = new List<GameObject>();
        Vector3 piecePos = gameObject.transform.position;


        foreach (Vector2 movement in directions)
        {
            Vector3 newPos = piecePos;
            newPos.x += movement.x * tileSize;
            newPos.y += movement.y  * pf.ySign * tileSize;

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
