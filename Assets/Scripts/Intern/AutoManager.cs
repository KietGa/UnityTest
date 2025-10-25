using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameSystem;

public class AutoManager : MonoBehaviour
{
    [SerializeField] private FishType target;

    public void AutoWin()
    {
        StartCoroutine(AutoWinDelay());
    }

    public void AutoLose()
    {
        StartCoroutine(AutoLoseDelay());
    }

    private IEnumerator AutoWinDelay()
    {
        while (Instance.boardPoints.Count > 0 && !Instance.isEnd) 
        {
            bool isFound = false;

            foreach (Point point in Instance.boardPoints)
            {
                if (point.type == target)
                {
                    isFound = true;
                    point.Trigger();
                    yield return new WaitForSeconds(0.5f);
                    break;
                }
            }

            if (!isFound)
            {
                int newTargetIndex = (int)target + 1;
                target = (FishType)newTargetIndex;
            }
        }
    }

    private IEnumerator AutoLoseDelay()
    {
        while (Instance.boardPoints.Count > 0 && !Instance.isEnd)
        {
            bool isFound = false;

            foreach (Point point in Instance.boardPoints)
            {
                if (point.type == target)
                {
                    isFound = true;
                    point.Trigger();
                    yield return new WaitForSeconds(0.5f);
                    break;
                }
            }

            if (isFound)
            {
                int newTargetIndex = (int)target + 1;
                target = (FishType)newTargetIndex;
            }
        }
    }
}
