using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardHolder : Lerpable 
{
    public List<GameObject> cards;

    void Start()
    {
        LoadCards();
        secondaryPosition = new Vector3(0, 10, 0);
    }

    private void LoadCards()
    {
        int distanceCoef = -1;
        int signAlt = 1;
        float buffer = 1.7f;

        for (int i = 0; i < cards.Count; ++i)
        {
            if(i % 3 == 0)
            {
                distanceCoef++;
            }

            cards[i] = (GameObject)GameObject.Instantiate((Object)cards[i]);
            cards[i].transform.parent = gameObject.transform;
            CardFunctions cf = (CardFunctions)cards[i].GetComponent(typeof(CardFunctions));
            Vector3 pos = gameObject.transform.position;

            cf.originalPosition = new Vector3(pos.x - (buffer * distanceCoef * signAlt), pos.y, pos.z);
            cf.LerpToOriginalPos();

            signAlt *= -1;

            if(i == 0)
            {
                distanceCoef++;
            }
        }
    }
}
