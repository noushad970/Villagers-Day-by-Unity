using UnityEngine;
using System.Collections;

public class LoadingInitialUI : MonoBehaviour
{
    [Header("Straight Directions")]
    public RectTransform[] moveLeft;
    public RectTransform[] moveRight;
    public RectTransform moveUp;
    public RectTransform moveDown;

    [Header("Diagonal Directions")]
    public RectTransform upperLeft;
    public RectTransform lowerLeft;
    public RectTransform upperRight;
    public RectTransform lowerRight;

    [Header("Settings")]
    public float duration = 1f;
    public float moveX = 1800f;
    public float moveY = 1200f;

    private int completed = 0;
    private int totalAnimations;
    private void Start()
    {
        StartAnimation();
    }
    public void StartAnimation()
    {
        completed = 0;

        // Count total animations dynamically
        totalAnimations = moveLeft.Length + moveRight.Length + 6;

        // LEFT
        foreach (var item in moveLeft)
        {
            StartCoroutine(MoveUI(item, new Vector2(-moveX, 0)));
        }

        // RIGHT
        foreach (var item in moveRight)
        {
            StartCoroutine(MoveUI(item, new Vector2(moveX, 0)));
        }

        // UP
        StartCoroutine(MoveUI(moveUp, new Vector2(0, moveY)));

        // DOWN
        StartCoroutine(MoveUI(moveDown, new Vector2(0, -moveY)));

        // DIAGONALS
        StartCoroutine(MoveUI(upperLeft, new Vector2(-moveX, moveY)));
        StartCoroutine(MoveUI(lowerLeft, new Vector2(-moveX, -moveY)));
        StartCoroutine(MoveUI(upperRight, new Vector2(moveX, moveY)));
        StartCoroutine(MoveUI(lowerRight, new Vector2(moveX, -moveY)));
    }

    IEnumerator MoveUI(RectTransform rect, Vector2 offset)
    {
        Vector2 startPos = rect.anchoredPosition;
        Vector2 targetPos = startPos + offset;

        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            // Smooth animation
            t = Mathf.SmoothStep(0, 1, t);

            rect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);

            time += Time.deltaTime;
            yield return null;
        }

        rect.anchoredPosition = targetPos;

        completed++;

        if (completed >= totalAnimations)
        {
            OnAllComplete();
        }
    }

    void OnAllComplete()
    {
        Debug.Log("All 8-direction animations finished!");
        AfterAnimation();
    }

    void AfterAnimation()
    {
        Debug.Log("Next function running...");
        // 👉 Put your next logic here
        this.gameObject.SetActive(false);
    }
}