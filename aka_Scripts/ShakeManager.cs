using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.Rendering;

public class ShakeManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject shakeGauge;
    [SerializeField] GameObject totalShakeGauge;
    public GameObject cocktailShaker;
    [SerializeField] GameObject shakePanel;
    [SerializeField] CocktailShakerController cocktailShakerController;
    [SerializeField] Text leftTimeText;
    [SerializeField] Image shakeGaugeImage;
    [SerializeField] Animator shakeGaugeAnimator;
    [SerializeField] Animator shakeGaugeImageAnimator;
    [SerializeField] GameObject lightGaugeHandle;
    [SerializeField] GameObject arrows;
    [SerializeField] GameObject bonusText;
    [SerializeField] GameObject  BonusBackground;
    [SerializeField] GameObject BonusLight;
    [SerializeField] GameObject scoreText;
    [SerializeField] GameObject PlusScoreEffect;
    [SerializeField] AudioSource bonusBGM;
    [SerializeField] GameObject mixImage;
    [SerializeField] RenderPipelineAsset urp;
    Image mixImage_Image;
    GameObject bonusBackground_clone;
    GameObject bonusLight_clone;
    [SerializeField] GameObject canvas;
    public bool IsBonus = false;
    int bonusShakeCount = 0;
    public float bonusTime = 0;
    public float leftTime = 3;
    public float bonusShakeAmount;
    // Start is called before the first frame update
    void Start()
    {
        bonusTime = 0;
        bonusBGM.volume = gameManager.bgmVolume;
    }

    private void FixedUpdate()
    {
        shakeGaugeImage.fillAmount -= 0.004f;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsBonus)
        {
            shakeGaugeImage.color = new Color(1, 1 - shakeGaugeImage.fillAmount,1);
            bonusTime += Time.deltaTime;
            if(bonusTime > 10)
            {
                gameManager.BGM.UnPause();
                Destroy(bonusBackground_clone);
                Destroy(bonusLight_clone);
                bonusTime = 0;
                IsBonus = false;
                gameManager.audioSource.PlayOneShot(gameManager.audioClips[2]);
                Debug.Log("シェイク成功");
                InactivateCocktailShaler();
                gameManager.phase = "ConversationPhase";
                gameObject.SetActive(false);
                GraphicsSettings.renderPipelineAsset = urp;
                return;
            }

            if(shakeGaugeImage.fillAmount >= 0.99f && bonusShakeAmount >= 2)
            {
                bonusShakeAmount = 0;
                PlusScoreInBonus();
            }
            return;
        }
        leftTime -= Time.deltaTime;
        leftTimeText.text = ((int)leftTime+1).ToString();
        if(shakeGaugeImage.fillAmount < 0.717f)
        {
            lightGaugeHandle.SetActive(false);
            shakeGaugeImage.color = new Color(1, 1 - shakeGaugeImage.fillAmount/1.5f , 1 - shakeGaugeImage.fillAmount/1.5f);
        }
        else if(0.717f <= shakeGaugeImage.fillAmount && shakeGaugeImage.fillAmount <= 0.816f)
        {
            lightGaugeHandle.SetActive(true);
            shakeGaugeImage.color = new Color(1, 1 - shakeGaugeImage.fillAmount / 0.75f, 1 - shakeGaugeImage.fillAmount / 0.75f);
        }
        else
        {
            lightGaugeHandle.SetActive(false);
            shakeGaugeImage.color = new Color(1, (shakeGaugeImage.fillAmount / 0.6f) - 1 ,  (shakeGaugeImage.fillAmount / 0.6f) - 1);
        }

        if(leftTime <= 0)
        {
            if(0.717f <= shakeGaugeImage.fillAmount && shakeGaugeImage.fillAmount <= 0.816f)
            {
                gameManager.audioSource.PlayOneShot(gameManager.audioClips[2]);
                Debug.Log("シェイク成功");
                gameManager.ShakeIsClear();
                gameObject.SetActive(false);
            }
            else
            {
                gameManager.audioSource.PlayOneShot(gameManager.audioClips[3]);
                Debug.Log("シェイク失敗");
                InactivateCocktailShaler();
                gameManager.ShakeIsMiss();
                gameObject.SetActive(false);
            }
        }
    }

    //シェイクゲージを増やす。
    public void IncreaseShakeGauge(float shakeDegree)
    {
        shakeGaugeImage.fillAmount += shakeDegree;
        if(IsBonus)
        {
            bonusShakeAmount += shakeDegree*2;
            if (shakeGaugeImage.fillAmount >= 0.95f)
            {
                shakeGaugeImageAnimator.SetBool("VibrationBool",true);
            }
            else
            {
                shakeGaugeImageAnimator.SetBool("VibrationBool",false);
            }
        }
    }

    //カクテルシェイカーを生成
    public void GenerateCocktailShaker(string cocktailName)
    {
        totalShakeGauge.transform.localScale = new Vector3(1, 1, 1);
        shakeGauge.transform.localScale = new Vector3(1, 1, 1);
        shakePanel.SetActive(true);
        cocktailShaker.SetActive(true);
        shakeGauge.SetActive(true);
        if (IsBonus)
        {
            GraphicsSettings.renderPipelineAsset = null;
            bonusBGM.Play();
            gameManager.BGM.Pause();
            GameObject bonusText_clone = Instantiate(bonusText, canvas.transform);
            bonusText_clone.transform.SetParent(shakePanel.transform);
            bonusText_clone.transform.localScale = new Vector3(1, 1, 1);
            bonusBackground_clone = Instantiate(BonusBackground,canvas.transform);
            bonusBackground_clone.transform.SetParent(shakePanel.transform);
            bonusLight_clone = Instantiate(BonusLight, canvas.transform);
            bonusLight_clone.transform.SetParent(shakePanel.transform);
            bonusTime = 0;
            arrows.SetActive(false);
            leftTimeText.gameObject.SetActive(false);

        }
        else
        {
            bonusBGM.Stop();
            arrows.SetActive(true);
            leftTimeText.gameObject.SetActive(true);
        }

        switch (cocktailName)
        {
            case "ニコラシカ":
                cocktailShakerController.shakeDegree = 0.024f;
                break;
            case "スレッジハンマー":
                cocktailShakerController.shakeDegree = 0.012f;
                break;
            case "ジン・バック":
                cocktailShakerController.shakeDegree = 0.015f;
                break;
            case "ギムレット":
                cocktailShakerController.shakeDegree = 0.06f;
                break;
            case "モスコミュール":
                cocktailShakerController.shakeDegree = 0.12f;
                break;
        }
    }

    //シェイクフェーズの各値をリセット
    public void ResetValues()
    {
        leftTime = 3;
        shakeGaugeImage.fillAmount = 0;
        lightGaugeHandle.SetActive(false);
    }

    //カクテルシェイカーを消去
    public void InactivateCocktailShaler()
    {
        shakePanel.SetActive(false);
        cocktailShaker.SetActive(false);
        shakeGauge.SetActive(false);
        leftTimeText.gameObject.SetActive(false);
    }

    //ボーナス時に得点を追加する
    public void PlusScoreInBonus()
    {
        gameManager.ScorePlus();
        GameObject plusScoreEffect_clone = Instantiate(PlusScoreEffect, canvas.transform);
        plusScoreEffect_clone.transform.SetParent(scoreText.transform);
        plusScoreEffect_clone.transform.localPosition = new Vector3(0, 0, 0);
        shakeGaugeAnimator.SetTrigger("BigTrigger");
    }

}
