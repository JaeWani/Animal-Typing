using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject UIs;

    [SerializeField] private Button startButton;
    [SerializeField] private Button ruleButton;
    [SerializeField] private Button quitButton;

    void Start()
    {
        UIs.SetActive(false);
        StartCoroutine(UIsActive(2, true));
        startButton.onClick.AddListener(() =>
        {
            UIs.SetActive(false);
            SoundManager.PlaySound("Button_Sound",1,false);
            SceneManager.LoadScene(1);
        });

        ruleButton.onClick.AddListener(() =>
        {
            SoundManager.PlaySound("Button_Sound", 1, false);
        });

        quitButton.onClick.AddListener(() =>
        {
            SoundManager.PlaySound("Button_Sound", 1, false);
            Application.Quit();
        });

        SoundManager.PlaySound("Title_Bgm", 1, true);
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator UIsActive(float time, bool isActive)
    {
        yield return new WaitForSeconds(time);
        UIs.SetActive(isActive);
    }
}
