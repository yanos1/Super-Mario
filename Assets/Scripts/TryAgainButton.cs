using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TryAgainButton : MonoBehaviour
{
    public float timer = 7f;
    public TextMeshProUGUI buttonText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        buttonText.text = $"Try Again ! ({Mathf.Ceil(timer)})";
        timer-= Time.deltaTime ;
    }

    public void TryAgain()
    {
        GameManager.game.TryAgain();
        SceneManager.LoadScene(1);
    }
}
