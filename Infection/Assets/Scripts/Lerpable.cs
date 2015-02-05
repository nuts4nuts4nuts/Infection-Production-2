using UnityEngine;
using System.Collections;

public class Lerpable : MonoBehaviour {

    public Vector3 originalPosition;

    protected Vector3 lerpTarget;
    protected bool isLerping = false;
    protected float lerpSpeed = 10.0f;


    [HideInInspector]
    public bool isSelected = false;

    protected Color originalColor;
    protected Color currentColor;

    protected virtual void Awake()
    {
        originalPosition = gameObject.transform.position;
    }

	// Update is called once per frame
	protected virtual void Update ()
    {
        if (isLerping)
        {
            //we slerping... or something
            transform.position = Vector3.Lerp(transform.position, lerpTarget, lerpSpeed * Time.deltaTime);
            if (Vector3.SqrMagnitude(transform.position - lerpTarget) < 0.0001f)
            {
                isLerping = false;
            }
        }
	}

    public virtual void LerpTo(Vector3 pos, float speed = -1)
    {
        isLerping = true;
        lerpTarget = pos;

        if(speed > 0)
        {
            lerpSpeed = speed;
        }
    }

    public virtual void LerpToOriginalPos(float speed = -1)
    {
        isLerping = true;
        lerpTarget = originalPosition;
        
        if(speed > 0)
        {
            lerpSpeed = speed;
        }
    }

    public void ResetColor()
    {
        renderer.material.color = currentColor;
    }
}
