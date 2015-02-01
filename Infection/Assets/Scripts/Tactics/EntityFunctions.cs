using UnityEngine;
using System.Collections;

public class EntityFunctions : MonoBehaviour {

    [HideInInspector]
    public bool isSelected = false;

    protected Color originalColor;
    protected Color currentColor;

    public void ResetColor()
    {
        renderer.material.color = currentColor;
    }
}
