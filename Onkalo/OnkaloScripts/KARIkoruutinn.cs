using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class KARIkoruutinn : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("Corou");
     }
 
     //コルーチン関数を定義
    private IEnumerator Corou() //コルーチン関数の名前
    {
        //コルーチンの内容
            yield return new WaitForSeconds(3.0f);
            SceneManager.LoadScene("Result");
         
    }
}
