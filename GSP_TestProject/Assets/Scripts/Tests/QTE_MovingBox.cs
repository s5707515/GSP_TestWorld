using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTE_MovingBox : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;

    [SerializeField] private Slider slider;

    private RectTransform goalRect;

    private float time = 0;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //time += Time.deltaTime * moveSpeed;

        //float t = Mathf.PingPong(0, 1);

        if(slider.handleRect.transform.position.x < goalRect.position.x + goalRect.rect.width /2 && slider.handleRect.transform.position.x > goalRect.position.x - goalRect.rect.width / 2)
        {
            Debug.Log("Goal");
        }

               
    }

    
}
