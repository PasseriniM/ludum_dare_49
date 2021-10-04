using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScript : MonoBehaviour
{
    public Image image;
    public Image thanksForPlaying;
    public GameObject playerObject;
    public float blendTime = 3f;
    
    private Transform playerTransform;
    private Collider2D collider2Dcustom;

    private void Awake()
    {
        collider2Dcustom = GetComponent<Collider2D>();
        playerTransform = playerObject.GetComponent<Transform>();
    }


    private void FixedUpdate()
    {
        if(collider2Dcustom.OverlapPoint(playerTransform.position))
        {
            playerObject.GetComponent<CharacterController2D>().End();
            StartCoroutine(FadeImageAndText());
        }
    }

    IEnumerator FadeImageAndText()
    {
        for (float i = 0; i <= blendTime; i += Time.deltaTime)
        {
            image.color = new Color(1, 1, 1, i/blendTime);
            yield return null;
        }

        for (float i = 0; i <= blendTime; i += Time.deltaTime)
        {
            thanksForPlaying.color = new Color(1, 1, 1, i / blendTime);
            yield return null;
        }
    }
}
