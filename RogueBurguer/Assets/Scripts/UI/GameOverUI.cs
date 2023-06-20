using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button returnButton;


    private void Start()
    {
        Hide();

        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        scoreText.text = DeliveryManager.Instance.GetScore().ToString();
        returnButton.onClick.AddListener(() =>
        {
            Loader.Load("MainMenu");
        });

    }
    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            scoreText.text = DeliveryManager.Instance.GetScore().ToString();
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
