using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    public FishType type;
    public bool isMove;
    public bool isBottom;
    public Transform initParent;

    private void Start()
    {
        initParent = transform.parent;
    }

    private void OnMouseDown()
    {
        Trigger();
    }

    public void Trigger()
    {
        if (isBottom) 
        {
            if (GameSystem.Instance.mode == 1)
            {
                GameSystem.Instance.FromBottomToBoard(this);
                isMove = true;
                isBottom = false;
            }
        }
        else
        {
            GameSystem.Instance.FromBoardToBottom(this);
            isMove = true;
            isBottom = true;
        }
    }

    private void Update()
    {
        if (isMove)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed * Time.deltaTime);

            if (transform.localPosition == Vector3.zero)
            {
                isMove = false;
            }
        }
    }
}
