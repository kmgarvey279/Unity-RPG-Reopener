using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBar : MonoBehaviour
{
    [SerializeField] private List<GameObject> points = new List<GameObject>();
    private int currentValue;
    private WaitForSeconds waitForPointUpdate = new WaitForSeconds(0.1f);

    public void SetValue(int newValue)
    {
        currentValue = Mathf.Clamp(newValue, 0, points.Count);
        for (int i = 0; i < points.Count; i++)
        {
            if (i < newValue)
            {
                points[i].SetActive(true);
            }
            else
            {
                points[i].SetActive(false);
            }
        }
    }

    public void UpdatePoints(int newValue)
    {
        if (newValue > currentValue)
        {
            currentValue = Mathf.Clamp(newValue, 0, points.Count);
            StartCoroutine(TickUpCo());
        }
        else if (newValue < currentValue)
        {
            currentValue = Mathf.Clamp(newValue, 0, points.Count);
            StartCoroutine(TickDownCo());
        }
    }

    private IEnumerator TickUpCo()
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (i < currentValue)
            {
                if (!points[i].activeInHierarchy)
                {
                    points[i].SetActive(true);
                    yield return waitForPointUpdate;
                }
            }
            else
            {
                if (points[i].activeInHierarchy)
                {
                    points[i].SetActive(false);
                    yield return waitForPointUpdate;
                }
            }
        }
        yield return null;
    }

    private IEnumerator TickDownCo()
    {
        for (int i = points.Count - 1; i >= 0; i--)
        {
            if (i < currentValue)
            {
                if (!points[i].activeInHierarchy)
                {
                    points[i].SetActive(true);
                    yield return waitForPointUpdate;
                }
            }
            else
            {
                if (points[i].activeInHierarchy)
                {
                    points[i].SetActive(false);
                    yield return waitForPointUpdate;
                }
            }
        }
        yield return null;
    }
}
