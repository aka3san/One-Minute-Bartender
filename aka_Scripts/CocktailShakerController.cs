using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocktailShakerController : MonoBehaviour
{
    [SerializeField] ShakeManager shakeManager;
    [SerializeField] GameManager gameManager;
    [SerializeField] AudioSource shakeSE;
    [SerializeField] float shakeDegreeMultiplier;
    [SerializeField] float value;
    bool IsFirstTime;
    Transform myTransform;
    Vector2 firstPosition;
    Vector2 lastTimePosition = new Vector2(0, 0);
    Vector2 lastTimeMoveDirection;
    Vector2 currentMoveDirection;
    Camera gameCamera;
    public float shakeDegree = 0.08f;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
        gameCamera = Camera.main;
        shakeSE.volume = gameManager.SEVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shakeSE.Play();
            IsFirstTime = true;
            StartCoroutine("CountShake");
            firstPosition = gameCamera.ScreenToWorldPoint(Input.mousePosition);
            
        }

        else if (Input.GetMouseButton(0))
        {
            Vector2 moveDistance = (Vector2)gameCamera.ScreenToWorldPoint(Input.mousePosition) - firstPosition;
            myTransform.position = moveDistance;
            myTransform.position = new Vector2(Mathf.Clamp(myTransform.position.x, -9, 9), Mathf.Clamp(myTransform.position.y, -5, 5));
        }

        else if (Input.GetMouseButtonUp(0))
        {
            StopCoroutine("CountShake");
            shakeSE.Stop();
        }

        else
        {
            lastTimePosition = new Vector2(0, 0);
            myTransform.position = new Vector3(0, 0, 0);
        }
    }

    IEnumerator CountShake()
    {
        while (true)
        {
            lastTimePosition = (Vector2)myTransform.position;
            yield return null;
            currentMoveDirection = (Vector2)myTransform.position-lastTimePosition;
            shakeManager.IncreaseShakeGauge(shakeDegree * currentMoveDirection.sqrMagnitude*shakeDegreeMultiplier);
        }
    }
}
