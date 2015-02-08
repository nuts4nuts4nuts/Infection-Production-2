using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartScreenFuncs : MonoBehaviour
{
    public Button startButton;

    void Awake()
    {
        startButton.transform.position = new Vector3(0, -0.4f, -9.1f);
        startButton.enabled = true;
    }

    public void PlayGame()
    {
        startButton.enabled = false;
        Application.LoadLevel("TacticsScene");
    }
}
