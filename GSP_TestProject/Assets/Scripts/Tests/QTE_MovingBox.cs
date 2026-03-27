using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QTE_MovingBox : MonoBehaviour
{
    [SerializeField] private float timeBetweenQTE = 1.0f;
    [SerializeField] private float moveSpeed = 2.0f;

    [SerializeField] private Slider slider;

    [SerializeField] private RectTransform goalRect;

    private Transform startPos;
    private Transform endPos;

    private float time = 0;

    bool goalFound = false;

   

    private void Start()
    {
        goalFound = false;

        SetNewGoalPosition();

        StartCoroutine(LerpSlider());
    }
    private void OnDisable()
    {
        StopCoroutine(LerpSlider());
    }
   


    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Space))
        {
            if (ISOverlappingUI(goalRect, slider.handleRect))
            {
                Debug.Log("YOU HIT");

                goalFound = true;
                //gameObject.SetActive(false);

                StartCoroutine(ResetQTE());

            }
            else
            {
                Debug.Log("YOU MISSED");
            }

        }


    }

    void SetNewGoalPosition()
    {
        RectTransform sliderRect = slider.GetComponent<RectTransform>();

        Vector3[] corners = new Vector3[4];
        sliderRect.GetWorldCorners(corners);

        float t = Random.Range(0.1f, 0.9f);

        float xPos = Mathf.Lerp(corners[0].x, corners[3].x, t);

        float yPos = (corners[0].y + corners[1].y) / 2f;

        goalRect.position = new Vector3(xPos, yPos, goalRect.position.z);
    }

    IEnumerator ResetQTE()
    {
        yield return new WaitForSeconds(timeBetweenQTE);
        goalFound = false;

        SetNewGoalPosition();

        StartCoroutine(LerpSlider());
    }
    IEnumerator LerpSlider()
    {

        while (!goalFound)
        {
            time += Time.deltaTime * moveSpeed;

            float t = Mathf.PingPong(time, 1);

            slider.value = Mathf.Lerp(0, 1, t);

            yield return new WaitForEndOfFrame();
        }
        
    }

    public bool ISOverlappingUI(RectTransform rectA , RectTransform rectB)
    {
        Vector3[] cornersA = new Vector3[4];
        Vector3[] cornersB = new Vector3[4];

        rectA.GetWorldCorners(cornersA);
        rectB.GetWorldCorners(cornersB);

        Rect rect1 = new Rect(cornersA[0], cornersA[2] - cornersA[0]);
        Rect rect2 = new Rect(cornersB[0], cornersB[2] - cornersB[0]);

        return rect1.Overlaps(rect2);

    }

    
}
