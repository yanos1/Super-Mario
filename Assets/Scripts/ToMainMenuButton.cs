using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMainMenuButton : MonoBehaviour
{
    // Start is called before the first frame update
    public float timer = 7f;
    public TextMeshProUGUI buttonText;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        buttonText.text = $"Main Menu ({Mathf.Ceil(timer)})";
        timer -= Time.deltaTime;
    }

    public void TryAgain()
    {
        GameManager.game.ResetStats();
        SceneManager.LoadScene(0);

    }
}
