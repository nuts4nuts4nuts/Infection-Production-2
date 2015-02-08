using UnityEngine;
using System;
using System.Collections;

public class Lerpable : MonoBehaviour {

    public Vector3 originalPosition;
    public Vector3 secondaryPosition;

    protected Vector3 lerpTarget;
    protected bool isLerping = false;
    protected float lerpSpeed = 13.0f;

    [HideInInspector]
    public bool isSelected = false;

    private Color originalColor;
    private Color currentColor;

    private Action<Vector3> doneLerpingDelegateVector;
    private Vector3 doneLerpingDataVector;

    private Action<bool> doneLerpingDelegateBool;
    private bool doneLerpingDataBool;

    protected virtual void Awake()
    {
        originalPosition = gameObject.transform.position;

        if(gameObject.renderer)
        {
            originalColor = renderer.material.color;
            currentColor = originalColor;
        }
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
                transform.position = lerpTarget;
                FinishedLerping();
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

    public void ExecWhenFinishedLerp(Action<Vector3> delegateFunc, Vector3 delegateData)
    {
        doneLerpingDelegateVector = delegateFunc;
        doneLerpingDataVector = delegateData;
    }

    public void ExecWhenFinishedLerp(Action<bool> delegateFunc, bool delegateData)
    {
        doneLerpingDelegateBool = delegateFunc;
        doneLerpingDataBool = delegateData;
    }

    private void FinishedLerping()
    {
        if(doneLerpingDelegateVector != null)
        {
            doneLerpingDelegateVector(doneLerpingDataVector);

            doneLerpingDataVector = Vector3.zero;
            doneLerpingDelegateVector = null;
        }
        
        if(doneLerpingDelegateBool != null)
        {
            doneLerpingDelegateBool(doneLerpingDataBool);

            doneLerpingDataBool = false;
            doneLerpingDelegateBool = null;
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

    public virtual void LerpToSecondaryPos(float speed = -1)
    {
        isLerping = true;
        lerpTarget = secondaryPosition;
        
        if(speed > 0)
        {
            lerpSpeed = speed;
        }
    }

    public void ResetToCurrentColor()
    {
        renderer.material.color = currentColor;
    }

    public void ResetToOriginalColor()
    {
        renderer.material.color = originalColor;
        currentColor = originalColor;
    }

    public void SetColorTemp(Color color)
    {
        renderer.material.color = color;
    }

    public void SetColorCurrent(Color color)
    {
        currentColor = color;
        renderer.material.color = color;
    }
}
