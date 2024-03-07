using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayGame()
    {
        animator.SetTrigger("Start");
        StartCoroutine(WaitAnimation());
        SceneManager.LoadScene(1);
    }

    IEnumerator WaitAnimation()
    {
        yield return new WaitForSeconds(1.5f);
    }
}
