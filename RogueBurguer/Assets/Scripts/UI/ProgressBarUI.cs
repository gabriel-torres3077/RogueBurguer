using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBarUI : MonoBehaviour
{

    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IProgress hasProgress;
    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IProgress>();
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        barImage.fillAmount = 0;

        Hide();

    }

    private void HasProgress_OnProgressChanged(object sender, IProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNomalized;

        if(e.progressNomalized == 0f || e.progressNomalized == 1f)
        {
            Hide();
        }
        else
        {
            Show();
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
