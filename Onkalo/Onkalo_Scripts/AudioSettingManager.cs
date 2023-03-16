using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSettingManager : MonoBehaviour
{
    public static float BGMVolume=0.9f;
    public static float SEVolume=0.9f;

    // [SerializeField] AudioSource BGMintro;
    // [SerializeField] AudioSource BGMloop;
    [SerializeField] AudioSource BGM;

    [SerializeField] AudioSource SE;

    //スライダー
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SESlider;

    [SerializeField] Button backbutton;

    //どこのシーンから来たか確認
    public string from="Title";


    //ゲッター
    public static float Audio_BGM(){
        return BGMVolume;
    }

    
    public static float Audio_SE(){
        return SEVolume;
    }


    //メソッド-------------------------------------------------------------------

    void Start(){
        // this.AudioM = FindObjectOfType<AudioSettingManager>(); // インスタンス化コメントアウト

        //データの引き継ぎ
        // BGMintro.volume = AudioSettingManager.Audio_BGM();
        // BGMloop.volume = AudioSettingManager.Audio_BGM();

        BGM.volume = AudioSettingManager.Audio_BGM();
        SE.volume = AudioSettingManager.Audio_SE();

        //UIの更新
        BGMSlider.value=BGM.volume;
        SESlider.value=SE.volume;

        //ボタンのOnclickの変更
        if(from.Equals("Title")){//タイトルからきた
            backbutton.onClick.AddListener (MovingTitle);
        }else if(from.Equals("Result")){//リザルトから来た
            backbutton.onClick.AddListener (MovingResult);
        }
        
    }



    public void MovingTitle(){
        //移動時にstaticに代入して保存
        // BGMVolume=BGMintro.volume;
        // BGMVolume=BGMloop.volume;
        BGMVolume=BGM.volume;
        SE.Play();
        StartCoroutine("waitTitle");
    }
    IEnumerator waitTitle(){
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene("Title");
    }


    public void MovingResult(){
        //音量を取得
        BGMVolume=BGM.volume;
        SEVolume=SE.volume;
        SE.Play();
        StartCoroutine("waitResult");
    }
    IEnumerator waitResult(){
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene("Result");
    }
}
