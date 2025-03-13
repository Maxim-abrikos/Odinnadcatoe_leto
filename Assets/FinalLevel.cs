using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalLevel : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField]
    //GameObject SlideDoor;

    //[SerializeField]
    //Level_completed Self;
    //private float speed = 1.0f, progress = 0.0f;
    //private Vector3 fromPosition = Vector3.zero;
    //private Vector3 toPosition = Vector3.one;
    ////void Start()
    ////{

    ////}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Self.Stars >=3 )
    //    {
    //        progress += Time.deltaTime * speed;
    //        transform.position = Vector3.Lerp(fromPosition, toPosition, progress);
    //    }
    //}

    //IEnumerator Slide()
    //{

    //    yield return null;
    //}

    [SerializeField]
    GameObject SlideDoor;
    [SerializeField]
    Level_completed Self;

    public int value = 10; // значение для проверки
    public float moveSpeed = 2f; // скорость движения объекта
    public float moveDistance = 1f; // расстояние, на которое смещается объект
    public float moveDuration = 2f; // длительность сдвига объекта

    private bool isMoving = false;

    void Start()
    {
        StartCoroutine(CheckValue());
    }

    IEnumerator CheckValue()
    {
        while (true)
        {
            if (Self.Stars>=3 && !isMoving)
            {
                isMoving = true;
                StartCoroutine(MoveObjectCoroutine());
            }
            yield return null;
        }
    }

    IEnumerator MoveObjectCoroutine()
    {
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + new Vector3(moveDistance, 0, 0);

        while (elapsedTime < moveDuration)
        {
            SlideDoor.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        isMoving = false;
    }


}
