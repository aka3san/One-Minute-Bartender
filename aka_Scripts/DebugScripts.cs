using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScripts : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    //キャラクターとの会話を始める。
    

    //材料を入れた後のシェークを生成する。
    public void　GenerateAfterCocktail()
    {
        Debug.Log("材料を入れた後のカクテルを生成しました。");
        gameManager.ShakeIsClear();
        gameManager.phase = "ConversationPhase";
    }

}
