using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FPS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public TMP_Text Title;
    public Button ReStartButton;
    public Button BackButton;

    public Sequence showSequence;

    public void OnShow()
    {
        Title.transform.DOScale(new Vector3(1, 1, 0.5f), 1f).From();
        ReStartButton.transform.DOScale(new Vector3(1, 1, 0.5f), 1f).From().OnComplete((() =>
        {
            ReStartButton.onClick.RemoveAllListeners();
            ReStartButton.onClick.AddListener((() =>
            {
                GameEntry.Event.Fire(this, GameReStartEventArgs.Create(1));
            }));
        }));
        BackButton.transform.DOScale(new Vector3(1, 1, 0.5f), 1f).From().OnComplete((() =>
        {
            BackButton.onClick.RemoveAllListeners();
            BackButton.onClick.AddListener((() =>
            {
                GameEntry.Event.Fire(this,ChangeSceneEventArgs.Create(1));
            }));
        }));
        
    }
    
}
