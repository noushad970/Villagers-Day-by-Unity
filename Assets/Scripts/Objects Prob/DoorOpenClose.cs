using UnityEngine;
using System.Collections;

public class DoorOpenClose : MonoBehaviour
{
    [Header("Reference Objects")]
    public GameObject positive90Object; // rotates to +90
    public GameObject negative90Object; // rotates to -90

    [Header("Rotation Settings")]
    public float rotationDuration = 1.5f;

    private Coroutine posCoroutine;
    private Coroutine negCoroutine;
    private void Start()
    {
        RotateNegativeToMinus90();
        RotatePositiveTo90();
        StartCoroutine(wait2());
    }
    // ---------- POSITIVE 90 ----------
    public void RotatePositiveTo90()
    {
        StartRotation(positive90Object, 0f, 90f, ref posCoroutine);
    }
    IEnumerator wait2()
    {
               yield return new WaitForSeconds(2f);
         backToZeroAll();
    }
    public void backToZeroAll()
    {
       RotateNegativeToZero();
       RotatePositiveToZero();
    }
    public void RotatePositiveToZero()
    {
        StartRotation(positive90Object, 90f, 0f, ref posCoroutine);
    }

    // ---------- NEGATIVE 90 ----------
    public void RotateNegativeToMinus90()
    {
        if(negative90Object == null) return;
        StartRotation(negative90Object, 0f, -90f, ref negCoroutine);
    }

    public void RotateNegativeToZero()
    {
        if(negative90Object == null) return;
        StartRotation(negative90Object, -90f, 0f, ref negCoroutine);
    }

    // ---------- CORE ROTATION ----------
    private void StartRotation(GameObject obj, float fromY, float toY, ref Coroutine coroutine)
    {
        if (obj == null) return;

        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(RotateYRoutine(obj, toY));
    }

    private IEnumerator RotateYRoutine(GameObject obj, float targetY)
    {
        float elapsed = 0f;

        Vector3 startEuler = obj.transform.localEulerAngles;
        float startY = NormalizeAngle(startEuler.y);

        while (elapsed < rotationDuration)
        {
            float t = elapsed / rotationDuration;
            float y = Mathf.Lerp(startY, targetY, t);

            obj.transform.localEulerAngles =
                new Vector3(startEuler.x, y, startEuler.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.localEulerAngles =
            new Vector3(startEuler.x, targetY, startEuler.z);
    }

    // Fixes 0–360 to -180–180 range
    private float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}
