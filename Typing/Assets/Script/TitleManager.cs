using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject UIs;

    [SerializeField] private Button startButton;
    [SerializeField] private Button ruleButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private Image rulePanel;

    public List<Sprite> ruleSprites = new List<Sprite>();


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
            Rule();
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
    public void Rule()
    {

        StartCoroutine(rule());
        IEnumerator rule()
        {
            rulePanel.GetComponent<RectTransform>().DOAnchorPosY(0, 1).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(5);
            rulePanel.sprite = ruleSprites[0];
            yield return new WaitForSeconds(5);
            rulePanel.sprite = ruleSprites[1];
            yield return new WaitForSeconds(5);
            rulePanel.sprite = ruleSprites[2];
            yield return new WaitForSeconds(5);
            rulePanel.GetComponent<RectTransform>().DOAnchorPosY(2000, 1).SetEase(Ease.OutQuad);
        }
    }
}
