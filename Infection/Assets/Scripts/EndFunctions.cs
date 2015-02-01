using UnityEngine;
using System.Collections;

public class EndFunctions : MonoBehaviour
{
    void Start()
    {
        SetColor(Color.blue);
    }

    public void SetColor(Color color)
    {
        renderer.material.color = color;
    }
}
