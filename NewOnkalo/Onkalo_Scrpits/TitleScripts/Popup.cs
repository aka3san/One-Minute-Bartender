using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Popup : MonoBehaviour
{
    public GameObject credit;
    public GameObject tutorial;
    [SerializeField] AudioSource SE;

    [SerializeField] Canvas canvas;

    public void creditpopup() {
        // currentPanel = Instantiate(credit, canvas.transform).GetComponent<NotificationPanel>();
        SE.Play();
        var instance = Instantiate(credit);
        instance.transform.SetParent(gameObject.transform,false);
        credit.transform.DOScale(new Vector3(2,2,2),3f);

        // GameObject cre = Instantiate(credit);
        // cre.transform.DOScale(1f,1f).SetEase(Ease.OutQuart);
        // transform.localScale = new Vector3(0f,0f,0f);
        // cre.transform.DOScale(1f,0.2f);
    }

    public void tutorialpopup() {
        // currentPanel = Instantiate(credit, canvas.transform).GetComponent<NotificationPanel>();
        SE.Play();
        var instance = Instantiate(tutorial);
        instance.transform.SetParent(gameObject.transform,false);
        // GameObject cre = Instantiate(credit);
        // cre.transform.DOScale(1f,1f).SetEase(Ease.OutQuart);
        // transform.localScale = new Vector3(0f,0f,0f);
        // cre.transform.DOScale(1f,0.2f);
    }

    public void OnClose() {
        // SE.Play();
                Destroy(tutorial);
        Destroy(credit);
        // Destroy(tutorial);


        // Sequence seq = DOTween.Sequence();
        // seq.Append(transform.DOScale(0f, 0.2f));
        // seq.OnComplete(() => DestroyWindow());
        // seq.Play();
    }

    void DestroyWindow() {
        Destroy(gameObject);
    }
}

