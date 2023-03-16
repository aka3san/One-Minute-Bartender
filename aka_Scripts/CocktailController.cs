using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocktailController : MonoBehaviour
{
    Animator animator;
    float animationTime;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        animationTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        animationTime += Time.deltaTime;
        if(animationTime > 0.5f)
        {
            gameManager.isCocktailDisplayed = true;
            Destroy(gameObject);
        }
    }
}
