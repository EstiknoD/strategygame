using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitrts : MonoBehaviour
{
    private GameObject selectedGameObject;
    private MovePositionDirect movePosition;

    private void Awake()
    {
        selectedGameObject = transform.GetChild(0).gameObject;
        SetSelectedVisible(false);
        movePosition = GetComponent<MovePositionDirect>();
    }

    public void SetSelectedVisible(bool visible)
    {
        selectedGameObject.SetActive(visible);
    }

    public void MoveTo(Vector3 targetPosition)
    {
        movePosition.SetMovePosition(targetPosition);
    }
}
