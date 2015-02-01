using UnityEngine;
using System.Collections;

public class CardFunctions : MonoBehaviour
{
    private bool isLerping = false;
    private Vector3 lerpTarget;
    private float lerpSpeed = 10.0f;
	
	// Update is called once per frame
	void Update ()
    {
        if (isLerping)
        {
            //we slerping... or something
            transform.position = Vector3.Lerp(transform.position, lerpTarget, (float)lerpSpeed * Time.deltaTime);
            if (Vector3.SqrMagnitude(transform.position - lerpTarget) < 0.001f)
            {
                isLerping = false;
            }
        }
	}

    public void LerpTo(Vector3 pos)
    {
        isLerping = true;
        lerpTarget = pos;
    }
}
