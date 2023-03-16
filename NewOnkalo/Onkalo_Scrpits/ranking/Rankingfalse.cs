using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rankingfalse : MonoBehaviour
{

    //どこのシーンから来たか確認
    public static int NameInputONOFF=0;

    public GameObject mybest;
    public GameObject mybestforT;

    public GameObject fromresult;
    public GameObject fromttitle;


    // Start is called before the first frame update
    void Start()
    {
        if(NameInputONOFF==0){
            fromresult.SetActive(false);
            fromttitle.SetActive(true);

            mybest.SetActive(false);
            mybestforT.SetActive(true);
        }
    }
}
