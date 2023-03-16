using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string phase;//今どのフェーズにいるか。
    public List<int> instructedBlendArray = new List<int>();//現在指示されているカクテルの材料
    public string instructedCocktail;　//注文されているカクテルの名前
    public GameObject instructedCocktailImage;
    public CharacterName currentCharacterName;//現在のお客さんの名前
    public bool IsPoliceCame = false; //警察が来たか着てないか
    public bool isMixed; //ミックスしたかしてないか
    public List<int> currentBlendArray = new List<int>(); //現在カクテルに入っている材料
    public GameObject currentCustomerUI;
    public float leftTime = 10;//残り時間
    public static int score = 0; //スコア
    public static int Getscore() { return score; }
    public AudioSource audioSource;
    public bool isCocktailDisplayed = false;
    int comboCount = 0; //コンボ数
    bool isClear = false;
    public float bgmVolume = 1; //BGMのボリューム
    public float SEVolume = 1; //SEのボリューム
    GameObject timeUpPanel_clone;
    bool IsFadeOutPanelIntantiated;//フェイドアウトパネルが存在するかどうか。

    [SerializeField] EventSystem eventSystem;
    [SerializeField] DebugScripts debugScript;//デバッグ様のスクリプト
    [SerializeField] ShakeManager shakeManager; //シェイク管理クラス
    //UI群
    [SerializeField] GameObject policeButton;
    [SerializeField] GameObject mixButton;
    [SerializeField] GameObject resetButton;
    [SerializeField] GameObject cocktailStaff;
    [SerializeField] GameObject Man;
    [SerializeField] GameObject Woman;
    [SerializeField] GameObject Suspect;
    [SerializeField] GameObject nikolaschkaColorImage;
    [SerializeField] GameObject ginBuckColorImage;
    [SerializeField] GameObject gimletColorImage;
    [SerializeField] GameObject sledgeHammerColorImage;
    [SerializeField] GameObject moscowMuleColorImage;
    [SerializeField] Text scoreText;
    [SerializeField] Text leftTimeText;
    [SerializeField] Text comboCountText;
    [SerializeField] GameObject prestartPanel;
    [SerializeField] GameObject[] pourImages;
    [SerializeField] GameObject tapEffect;
    [SerializeField] GameObject[] CocktailButtons;
    [SerializeField] GameObject[] CocktailsPrefab;
    [SerializeField] GameObject timeUpPanel;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject policeLump;
    [SerializeField] GameObject policeBackground;
    [SerializeField] GameObject backGroundSlide;
    [SerializeField] GameObject[] resulsSceneUIs;
    [SerializeField] GameObject CocktailGenerateEffect;
    public AudioSource BGM;
    public AudioClip[] audioClips;
    [SerializeField] Image policePanel;
    Camera mainCamera;


    // Start is called before the first frame update
    void Start()
    {
        leftTime = 60f;
        score = 0;
        SEVolume = AudioSettingManager.Audio_SE();
        bgmVolume = AudioSettingManager.Audio_BGM();
        IsFadeOutPanelIntantiated = false;
        StartCoroutine("PrestartPhase");
        mainCamera = Camera.main;
        isCocktailDisplayed = false;
        audioSource.volume = SEVolume;
        BGM.volume = bgmVolume;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(0 <= leftTime && leftTime <= 0.1f)
        {
            if(!IsFadeOutPanelIntantiated)
            {
                timeUpPanel_clone = Instantiate(timeUpPanel, canvas.transform);
                timeUpPanel_clone.GetComponent<Animator>().SetTrigger("PanelDownTrigger");
                IsFadeOutPanelIntantiated = true;
            }
            else
            {
                timeUpPanel_clone.GetComponent<Animator>().SetFloat("MotionTime", 1 - leftTime*10);
            }
        }
        if (leftTime < 0 && phase != "AfterGamePhase") 
        {
            timeUpPanel_clone.GetComponent<Animator>().SetFloat("MotionTime", 1);
            StartCoroutine("AfterGamePhase");
            return;
        }
        if(leftTime <= 0)
        {
            return;
        }
        
        if(shakeManager.IsBonus)
        {
            return;
        }
        leftTime -= Time.deltaTime;
        leftTimeText.text = ((int)leftTime + 1).ToString();
        if(0 < leftTime && leftTime < 5)
        {
            leftTimeText.GetComponent<Animator>().SetTrigger("CountDownTrigger");
            leftTimeText.GetComponent<Animator>().SetFloat("MotionTime", (float)Math.Ceiling(leftTime) - leftTime);
        }
        if(phase != "ShakePhase" && Input.GetMouseButtonDown(0))
        {
            Vector3 touchPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = 0;
            Instantiate(tapEffect,touchPosition, Quaternion.identity);
        }
    }

    //下部のUIを非表示
    void UIDelete()
    {
        policeButton.SetActive(false);
        mixButton.SetActive(false);
        resetButton.SetActive(false);
        cocktailStaff.SetActive(false);
    }

    //下部のUIを表示
    void UIActivate()
    {
        policeButton.SetActive(true);
        mixButton.SetActive(true);
        resetButton.SetActive(true);
        cocktailStaff.SetActive(true);
    }

    //scoreを１増やす。
    public void ScorePlus()
    {
        if(phase == "BonusPhase")
        {
            score += 5;
            scoreText.text = score.ToString();
        }
        else if(currentCharacterName == CharacterName.Suspect)
        {
            score += 1;
            scoreText.text = score.ToString();
        }
        else if(currentCharacterName == CharacterName.Woman)
        {
            score += 30;
            scoreText.text = score.ToString();
        }
        else if(instructedCocktail == "ジン・バック")
        {
            score += 35;
            scoreText.text = score.ToString();
        }
        else
        {
            score += 25;
            scoreText.text = score.ToString();
        }
    }

    //カクテルに材料を入れる
    public void AddLemon()
    {
        if(eventSystem.currentSelectedGameObject.transform.localScale.x > 1)
        {
            audioSource.PlayOneShot(audioClips[3]);
            return;
        }
        if(currentBlendArray.Contains((int)CocktailName.Lemon))
        {
            GameObject currentButton = eventSystem.currentSelectedGameObject;
            currentButton.transform.localScale *= 1.3f;
        }
        audioSource.PlayOneShot(audioClips[0]);
        currentBlendArray.Add((int)CocktailName.Lemon);
        CocktailButtons[(int)CocktailName.Lemon].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        CocktailButtons[(int)CocktailName.Lemon].transform.Find("Text").gameObject.SetActive(false);
        Debug.Log(currentBlendArray);
    }
    public void AddBrandy()
    {
        if (eventSystem.currentSelectedGameObject.transform.localScale.x > 1)
        {
            audioSource.PlayOneShot(audioClips[3]);
            return;
        }
        if (currentBlendArray.Contains((int)CocktailName.Brandy))
        {
            GameObject currentButton = eventSystem.currentSelectedGameObject;
            currentButton.transform.localScale *= 1.3f;
        }
        audioSource.PlayOneShot(audioClips[0]);
        currentBlendArray.Add((int)CocktailName.Brandy);
        CocktailButtons[(int)CocktailName.Brandy].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        CocktailButtons[(int)CocktailName.Brandy].transform.Find("Text").gameObject.SetActive(false);
    }
    public void AddDryGin()
    {
        if (eventSystem.currentSelectedGameObject.transform.localScale.x > 1)
        {
            audioSource.PlayOneShot(audioClips[3]);
            return;
        }
        if (currentBlendArray.Contains((int)CocktailName.DryGin))
        {
            GameObject currentButton = eventSystem.currentSelectedGameObject;
            currentButton.transform.localScale *= 1.3f;
        }
        audioSource.PlayOneShot(audioClips[0]);
        currentBlendArray.Add((int)CocktailName.DryGin);
        CocktailButtons[(int)CocktailName.DryGin].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        CocktailButtons[(int)CocktailName.DryGin].transform.Find("Text").gameObject.SetActive(false);
    }
    public void AddVodka()
    {
        if (eventSystem.currentSelectedGameObject.transform.localScale.x > 1)
        {
            audioSource.PlayOneShot(audioClips[3]);
            return;
        }
        if (currentBlendArray.Contains((int)CocktailName.Vodka))
        {
            GameObject currentButton = eventSystem.currentSelectedGameObject;
            currentButton.transform.localScale *= 1.3f;
        }
        audioSource.PlayOneShot(audioClips[0]);
        currentBlendArray.Add((int)CocktailName.Vodka);
        CocktailButtons[(int)CocktailName.Vodka].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        CocktailButtons[(int)CocktailName.Vodka].transform.Find("Text").gameObject.SetActive(false);
    }
    public void AddLimeJuice()
    {
        if (eventSystem.currentSelectedGameObject.transform.localScale.x > 1)
        {
            audioSource.PlayOneShot(audioClips[3]);
            return;
        }
        if (currentBlendArray.Contains((int)CocktailName.LimeJuice))
        {
            GameObject currentButton = eventSystem.currentSelectedGameObject;
            currentButton.transform.localScale *= 1.3f;
        }
        audioSource.PlayOneShot(audioClips[0]);
        currentBlendArray.Add((int)CocktailName.LimeJuice);
        CocktailButtons[(int)CocktailName.LimeJuice].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        CocktailButtons[(int)CocktailName.LimeJuice].transform.Find("Text").gameObject.SetActive(false);
    }
    public void AddGingerAle()
    {
        if (eventSystem.currentSelectedGameObject.transform.localScale.x > 1)
        {
            audioSource.PlayOneShot(audioClips[3]);
            return;
        }
        if (currentBlendArray.Contains((int)CocktailName.GingerAle))
        {
            GameObject currentButton = eventSystem.currentSelectedGameObject;
            currentButton.transform.localScale *= 1.3f;
        }
        audioSource.PlayOneShot(audioClips[0]);
        currentBlendArray.Add((int)CocktailName.GingerAle);
        CocktailButtons[(int)CocktailName.GingerAle].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        CocktailButtons[(int)CocktailName.GingerAle].transform.Find("Text").gameObject.SetActive(false);
    }

    //ミックスした材料と指示されたカクテルが一致するか判定する
    bool IsMatchedCocktail()
    {
        if(currentCharacterName !=  CharacterName.Suspect && IsPoliceCame)
        {
            IsPoliceCame = false;
            return false;
        }
       if(currentBlendArray.Count != instructedBlendArray.Count)
        {
            return false;
        }

       for(int i=0; i < currentBlendArray.Count; i++)
        {
            if(currentBlendArray.Contains(instructedBlendArray[i]) == false)
            {
                return false;
            }
        }
        return true;
    }



    //ミックス完了通知
    public void DecideMix()
    {
        eventSystem.SetSelectedGameObject(null);
        isMixed = true;
        foreach(GameObject cocktailButton in CocktailButtons)
        {
            cocktailButton.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            cocktailButton.transform.Find("Text").gameObject.SetActive(true);
        }
    }

    //キャラクター名を列挙型にして扱いやすくしている。
    public enum CharacterName
    {
        Man, Woman, Suspect
    }

    public enum CocktailName
    {
        Lemon, Brandy, DryGin, Vodka, LimeJuice, GingerAle
    }

    //currentBlendArrayをリセットする。
    public void ResetCurrentBlendArray()
    {
        audioSource.PlayOneShot(audioClips[1]);
        eventSystem.SetSelectedGameObject(null);
        currentBlendArray = new List<int>();
        foreach (GameObject cocktailButton in CocktailButtons)
        {
            cocktailButton.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            cocktailButton.transform.localScale = new Vector3(1, 1, 1);
            cocktailButton.transform.Find("Text").gameObject.SetActive(true);
        }
        
    }

    //警察を呼ぶ
    public void CallToPolice()
    {
        isMixed = true;
        IsPoliceCame = true;
        foreach (GameObject cocktailButton in CocktailButtons)
        {
            cocktailButton.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            cocktailButton.transform.Find("Text").gameObject.SetActive(true);
        }
        eventSystem.SetSelectedGameObject(null);
    }

    //ランダムでキャラクターの名前を返す。
    public CharacterName RandomGenerateCharacterName()
    {
        int num = UnityEngine.Random.Range(0, 3);
        CharacterName name = (CharacterName)Enum.ToObject(typeof(CharacterName), num);
        return name;
    }

    //キャラに応じてで指示されたカクテルの材料をinstructedBlendArrayに代入する。
    void SetInstructedBlendArray(CharacterName characterName)
    {
        switch(characterName)
        {
            case CharacterName.Man:
                int randomNumber = UnityEngine.Random.Range(0, 3);
                if(randomNumber == 0)
                {
                    instructedBlendArray = new List<int> { (int)CocktailName.Lemon, (int)CocktailName.Brandy, (int)CocktailName.Lemon, (int)CocktailName.Brandy };
                    instructedCocktail = "ニコラシカ";
                    instructedCocktailImage = nikolaschkaColorImage;
                    break;
                }
                else if(randomNumber == 1)
                {
                    instructedBlendArray = new List<int> { (int)CocktailName.Vodka, (int)CocktailName.LimeJuice, (int)CocktailName.Vodka, (int)CocktailName.LimeJuice };
                    instructedCocktail = "スレッジハンマー";
                    instructedCocktailImage = sledgeHammerColorImage;
                    break;
                }
                else
                {
                    instructedBlendArray = new List<int> { (int)CocktailName.DryGin, (int)CocktailName.Lemon, (int)CocktailName.GingerAle, (int)CocktailName.DryGin, (int)CocktailName.Lemon, (int)CocktailName.GingerAle };
                    instructedCocktail = "ジン・バック";
                    instructedCocktailImage = ginBuckColorImage;
                    break;
                }
                
            case CharacterName.Woman:
                int randomNumber2 = UnityEngine.Random.Range(0, 2);
                if(randomNumber2 == 0)
                {
                    instructedBlendArray = new List<int> { (int)CocktailName.DryGin, (int)CocktailName.LimeJuice };
                    instructedCocktail = "ギムレット";
                    instructedCocktailImage = gimletColorImage;
                    break;
                }
                else
                {
                    instructedBlendArray = new List<int> { (int)CocktailName.Vodka, (int)CocktailName.LimeJuice, (int)CocktailName.GingerAle };
                    instructedCocktail = "モスコミュール";
                    instructedCocktailImage = moscowMuleColorImage;
                    break;
                }
            case CharacterName.Suspect:
                instructedBlendArray = new List<int> { 100 };
                instructedCocktail = "ニコラシカ";
                instructedCocktailImage = nikolaschkaColorImage;
                break;
        }
    }

    //キャラ名を受け取って会話を生成する関数
    public void ActivateConversation(CharacterName name)
    {
        int random = UnityEngine.Random.Range(0, 8);
        switch(name)
        {
            case CharacterName.Man:
                Man.SetActive(true);
                currentCustomerUI = Man;
                if(random == 0)
                {
                    Man.GetComponentInChildren<Text>().text = $"{instructedCocktail}を頼むよ。\nあ、大きめのサイズでね。";
                }
                else if (random == 1)
                {
                    Man.GetComponentInChildren<Text>().text = $"大きいサイズの{instructedCocktail}\nをお願いします。";
                }
                else if (random == 2)
                {
                    Man.GetComponentInChildren<Text>().text = $"ラージサイズの{instructedCocktail}はあるか？\nちょっと会社の同僚がさ…";
                }
                else if (random == 3)
                {
                    Man.GetComponentInChildren<Text>().text = $"オススメのカクテルはなんだ？\n{instructedCocktail}？\nじゃあそれにするよ。Lサイズで頼む。";
                }
                else if (random == 4)
                {
                    Man.GetComponentInChildren<Text>().text = $"{instructedCocktail}\nという名前の酒があるんだ。\nなんかカッコいいな…じゃあこれにします。\n奮発して大きめで飲もうかな。";
                }
                else if (random == 5)
                {
                    Man.GetComponentInChildren<Text>().text = $"聞いてくれよ、やっと昇進できたんだ！\n大きめの{instructedCocktail}で祝酒したいな！";
                }
                else if (random == 6)
                {
                    Man.GetComponentInChildren<Text>().text = $"アレキサンダーを…いや、ジンフィズ…\nやっぱり{instructedCocktail}\n大サイズの{instructedCocktail}にしよう！";
                }
                else if (random == 7)
                {
                    Man.GetComponentInChildren<Text>().text = $"お酒って正直よく分からないんだよな。\n適当に{instructedCocktail}にしようかな。";
                }
                break;
            case CharacterName.Woman:
                Woman.SetActive(true);
                currentCustomerUI = Woman;
                if (random == 0)
                {
                    Woman.GetComponentInChildren<Text>().text = $"{instructedCocktail}をお願いできますか？";
                }
                else if (random == 1)
                {
                    Woman.GetComponentInChildren<Text>().text = $"今日は{instructedCocktail}が\n飲みたい気分かもー！";
                }
                else if (random == 2)
                {
                    Woman.GetComponentInChildren<Text>().text = $"{instructedCocktail}\nっていうカクテルがあるの？\nじゃあそれにしようかな。";
                }
                else if (random == 3)
                {
                    Woman.GetComponentInChildren<Text>().text = $"ちょっとマスター！\n彼氏にデートドタキャンされてさー！\nでさー、…え、早く注文しろって？\n…{instructedCocktail}。";
                }
                else if (random == 4)
                {
                    Woman.GetComponentInChildren<Text>().text = $"「あちらのお客様からです」\nって本当にあるのかしら？ま、いいや\n{instructedCocktail}をお願いね。";
                }
                else if (random == 5)
                {
                    Woman.GetComponentInChildren<Text>().text = $"{instructedCocktail}をお願い、\nマスターの奢りでね！\nやだ、冗談よ。";
                }
                else if (random == 6)
                {
                    Woman.GetComponentInChildren<Text>().text = $"最近マティーニばっかり飲んでるのよね、\n今日は{instructedCocktail}にします。";
                }
                else if (random == 7)
                {
                    Woman.GetComponentInChildren<Text>().text = $"ジンを飲むとさ、喉がジンジンしない？\n…何でもないわ。\n{instructedCocktail}をお願い。";
                }
                break;
            case CharacterName.Suspect:
                Suspect.SetActive(true);
                currentCustomerUI = Suspect;
                if (random == 0)
                {
                    Suspect.GetComponentInChildren<Text>().text = $"シケた店だなあ、オイ！";
                }
                else if (random == 1)
                {
                    Suspect.GetComponentInChildren<Text>().text = $"酒？んなもんいいから金出せよ！";
                }
                else if (random == 2)
                {
                    Suspect.GetComponentInChildren<Text>().text = $"今、無一文なんだよな-。\nどっかにタダ酒出してくれるバーテンさんは\nいねえかなー。";
                }
                else if (random == 3)
                {
                    Suspect.GetComponentInChildren<Text>().text = $"ヒャッハーーー！！\n今日は気分がいい、ひと暴れしてやるか！！";
                }
                else if (random == 4)
                {
                    Suspect.GetComponentInChildren<Text>().text = $"田舎の祖母が重篤なんです！\n９割引で酒を譲ってください！！";
                }
                else if (random == 5)
                {
                    Suspect.GetComponentInChildren<Text>().text = $"あぁー？お客様は神様だろ？\nやんのかコラ？！";
                }
                else if (random == 6)
                {
                    Suspect.GetComponentInChildren<Text>().text = $"ヨットクラブパンチが飲みてえなぁ！\nあ？当店ではご用意しておりません？\nやる気あんのか？！";
                }
                else if (random == 7)
                {
                    Suspect.GetComponentInChildren<Text>().text = $"強盗ごっこしようぜ！\nお前人質な！";
                }
                return;
        }
        instructedCocktailImage.SetActive(true);
    }
    //シェーククリア後、失敗後に会話を消す。
    public void InactivateConversation()
    {
        currentCustomerUI.SetActive(false);
        instructedCocktailImage.SetActive(false);
        UIDelete();
    }

    //ゲーム画面の最初のコルーチン
    IEnumerator PrestartPhase()
    {
        Time.timeScale = 0;
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        BGM.Play();
        audioSource.PlayOneShot(audioClips[0]);
        Time.timeScale = 1;
        prestartPanel.SetActive(false);
        phase = "ConversationPhase";
        StartCoroutine("ConversationPhase");
    }

    //会話フェーズのコルーチン
    IEnumerator ConversationPhase()
    {
        yield return new WaitUntil(() => phase != "BonusPhase");
        UIDelete();
        Debug.Log("会話フェーズに入りました");
        instructedBlendArray = new List<int>();
        currentCharacterName = RandomGenerateCharacterName();
        Debug.Log($"{currentCharacterName.ToString()}との会話を生成しました。");
        SetInstructedBlendArray(currentCharacterName);
        ActivateConversation(currentCharacterName);
        Image currentCustomerImage = currentCustomerUI.GetComponent<Image>();
        currentCustomerImage.color = new Color(0, 0, 0, 0.5f);
        currentCustomerUI.GetComponent<Animator>().SetTrigger("FadeInTrigger");
        yield return new WaitForSeconds(0.33f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        audioSource.PlayOneShot(audioClips[0]);
        phase = "MixPhase";   
        StartCoroutine("MixPhase");
    }

    //ミックスフェーズのコルーチン
    IEnumerator MixPhase()
    {
        UIActivate();
        if(currentCharacterName == CharacterName.Suspect)
        {
            policeLump.SetActive(true);
            policeLump.GetComponent<Animator>().SetTrigger("MoveUpTrigger");
        }
        currentBlendArray = new List<int>();
        Debug.Log("ミックスフェーズに入りました。");
        while (phase == "MixPhase")
        {
            foreach (GameObject cocktailButton in CocktailButtons)
            {
                cocktailButton.transform.localScale = new Vector3(1, 1, 1);
                cocktailButton.transform.Find("Text").transform.gameObject.SetActive(true);
            }
            isMixed = false;
            yield return new WaitUntil(() => isMixed);
            if (IsMatchedCocktail())
            {
                audioSource.PlayOneShot(audioClips[0]);
                Debug.Log("ミックスが成功しました。");
                IsPoliceCame = false;
                phase = "ShakePhase";
                break;
            }
            else if(currentCharacterName == CharacterName.Suspect)
            {
                if(IsPoliceCame)
                {
                    audioSource.PlayOneShot(audioClips[4]);
                    policeLump.GetComponent<Animator>().SetTrigger("PoliceLumpTrigger");
                    policeBackground.SetActive(true);
                    policeBackground.GetComponent<Animator>().SetTrigger("EffectTrigger");
                    IsPoliceCame = false;
                    Debug.Log("容疑者が確保されました。");
                    ScorePlus();
                    comboCount += 1;
                    comboCountText.text = comboCount.ToString();
                    Image currentCustomerImage = currentCustomerUI.GetComponent<Image>();
                    currentCustomerUI.GetComponent<Animator>().SetTrigger("SuspectFadeOutTrigger");
                    policePanel.GetComponent<Animator>().SetTrigger("FadeInTrigger");
                    yield return new WaitForSeconds(1.05f);
                    InactivateConversation();
                    policeLump.GetComponent<Animator>().SetTrigger("PoliceLumpTrigger");
                    policeBackground.GetComponent<Animator>().SetTrigger("EffectTrigger");
                    policeLump.SetActive(false);
                    policeBackground.SetActive(false);
                    policePanel.color = new Color(0.4f, 0, 0, 0);
                    if(comboCount >= 5)
                    {
                        phase = "BonusPhase";
                        break;
                    }
                    phase = "ConversationPhase";
                    break;
                }
                else
                {
                    audioSource.PlayOneShot(audioClips[3]);
                    comboCount = 0;
                    comboCountText.text = comboCount.ToString();
                    currentBlendArray = new List<int>();
                    Debug.Log("警察を呼んでください");
                }
                IsPoliceCame = false;
            }
            else
            {
                audioSource.PlayOneShot(audioClips[3]);
                Debug.Log("ミックスが失敗しました。");
                comboCount = 0;
                comboCountText.text = comboCount.ToString();
                IsPoliceCame = false;
                currentBlendArray = new List<int>();
            }
            yield return null;
        }
        if (phase == "ShakePhase")
        {
            StartCoroutine("ShakePhase");
        }
        else if(phase == "ConversationPhase")
        {
            StartCoroutine("ConversationPhase");
        }
        else
        {
            StartCoroutine("BonusPhase");
        }
    }

    //シェイクが成功した時に呼び出される
    public void ShakeIsClear()
    {
        ScorePlus();
        isClear = true;
        comboCount += 1;
        comboCountText.text = comboCount.ToString();
        if (comboCount >= 5)
        {
            phase = "BonusPhase";
            return;
        }
        phase = "ConversationPhase";
    }

    //シェイクが失敗した時に呼び出される
    public void ShakeIsMiss()
    {
        comboCount = 0;
        comboCountText.text = comboCount.ToString();
        phase = "ConversationPhase";
    }

    //シェークフェーズのコルーチン
    IEnumerator ShakePhase()
    {
        Debug.Log("シェークフェーズに入りました。");
        shakeManager.gameObject.SetActive(true);
        shakeManager.GenerateCocktailShaker(instructedCocktail);
        shakeManager.ResetValues();
        yield return new WaitUntil(() => phase != "ShakePhase");
        UIDelete();
        GameObject cocktailPrefab = null;
        Image currentCustomerImage = currentCustomerUI.GetComponent<Image>();
        if(isClear)
        {
            shakeManager.cocktailShaker.SetActive(false);
            switch (instructedCocktail)
            {
                case "ニコラシカ":
                    cocktailPrefab = CocktailsPrefab[0];
                    break;
                case "スレッジハンマー":
                    cocktailPrefab = CocktailsPrefab[1];
                    break;
                case "ジン・バック":
                    cocktailPrefab = CocktailsPrefab[2];
                    break;
                case "ギムレット":
                    cocktailPrefab = CocktailsPrefab[3];
                    break;
                case "モスコミュール":
                    cocktailPrefab = CocktailsPrefab[4];
                    break;
            }
            GameObject cocktailPrefab_clone = Instantiate(cocktailPrefab);
            Instantiate(CocktailGenerateEffect,canvas.transform);
            currentCustomerUI.GetComponent<Animator>().SetTrigger("FadeOutTrigger");
            yield return new WaitForSeconds(0.6f);
            Destroy(cocktailPrefab_clone);
            shakeManager.InactivateCocktailShaler();
            isCocktailDisplayed = false;
            isClear = false;
        }
        else
        {
            shakeManager.InactivateCocktailShaler();
            currentCustomerUI.GetComponent<Animator>().SetTrigger("MissFadeOutTrigger");
            yield return new WaitForSeconds(0.35f);
        }
        InactivateConversation();
        if (phase == "ConversationPhase")
        {
            StartCoroutine("ConversationPhase");
        }
        else if(phase == "BonusPhase")
        {
            StartCoroutine("BonusPhase");
        }
    }

    IEnumerator BonusPhase()
    {
        Debug.Log("ボーナスフェーズに入りました。");
        phase = "BonusPhase";
        shakeManager.IsBonus = true;
        comboCount = 0;
        comboCountText.text = comboCount.ToString();
        shakeManager.gameObject.SetActive(true);
        shakeManager.GenerateCocktailShaker("ニコラシカ");
        shakeManager.ResetValues();
        yield return new WaitUntil(() => phase != "BonusPhase");
        StartCoroutine("ConversationPhase");
    }

    IEnumerator AfterGamePhase()
    {
        if(phase == "ShakePhase" || phase == "BonusPhase")
        {
            shakeManager.InactivateCocktailShaler();
            shakeManager.gameObject.SetActive(false);
            StopCoroutine("ShakePhase");
            StopCoroutine("BonusPhase");
        }
        else
        {
            StopCoroutine("ConversationPhase");
            StopCoroutine("MixPhase");
        }
        phase = "AfterGamePhase";
        leftTimeText.text = "0";
        leftTimeText.GetComponent<Animator>().SetTrigger("CountDownTrigger");
        GameObject fadeOutPanel_clone = timeUpPanel_clone.transform.Find("FadeOutPanel").gameObject;
        yield return new WaitForSeconds(1.5f);
        float afterGameTime = 0;
        while(afterGameTime < 1.5f)
        {
            afterGameTime += Time.deltaTime;
            fadeOutPanel_clone.GetComponent<Image>().color = new Color(0, 0, 0, afterGameTime / 1.5f);
            yield return null;
            continue;
        }
        GameObject backGroundSlide_clone = Instantiate(backGroundSlide, canvas.transform);
        yield return new WaitForSeconds(1.5f);
        foreach (GameObject resulsSceneUI in resulsSceneUIs)
        {
            Instantiate(resulsSceneUI, canvas.transform);
        }
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Result");
    }

}
