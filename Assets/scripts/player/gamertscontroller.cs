using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamertscontroller : MonoBehaviour
{

    [SerializeField] private Transform selectionAreaTransform;
    private Vector3 startPosition;
    private List<unitrts> selectedUnitRTSList;

    private void Awake()
    {
        selectedUnitRTSList = new List<unitrts>();
        selectionAreaTransform.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            selectionAreaTransform.gameObject.SetActive(true);
            startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 lowerleft = new Vector3(
                Mathf.Min(startPosition.x, currentMousePosition.x), 
                Mathf.Min(startPosition.y, currentMousePosition.y));
            Vector3 upperright = new Vector3(
                Mathf.Max(startPosition.x, currentMousePosition.x), 
                Mathf.Max(startPosition.y, currentMousePosition.y));
            selectionAreaTransform.position = lowerleft;  
            selectionAreaTransform.localScale = upperright - lowerleft;
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectionAreaTransform.gameObject.SetActive(false);

            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, Camera.main.ScreenToWorldPoint(Input.mousePosition));

            foreach(unitrts UnitRTS in selectedUnitRTSList)
            {
                UnitRTS.SetSelectedVisible(false);
            }

            selectedUnitRTSList.Clear();

            foreach(Collider2D collider2D in collider2DArray)
            {
                unitrts UnitRTS = collider2D.GetComponent<unitrts>();
                if(UnitRTS != null)
                {
                    UnitRTS.SetSelectedVisible(true);
                    selectedUnitRTSList.Add(UnitRTS);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 moveToPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            List<Vector3> targetPositionList = GetPositionListAround(moveToPosition, new float[] { 1.5f, 3f, 4.5f }, new int[] { 5, 10, 20 });

            int targetPositionListIndex = 0;
            foreach(unitrts unityRTS in selectedUnitRTSList)
            {
                unityRTS.MoveTo(targetPositionList[targetPositionListIndex]);
                targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
            }
        }
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float[] rigDistanceArray, int[] rigPositionCountArray)
    {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);
        for (int i = 0; i < rigDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPosition, rigDistanceArray[i], rigPositionCountArray[i]));
        }
        return positionList;
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount)
    {
        List<Vector3> positionList = new List<Vector3>();
        for(int i = 0; i < positionCount; i++)
        {
            float angle = i * (360f / positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = startPosition + dir * distance;
            positionList.Add(position);
        }
        return positionList;
    }

    private Vector3 ApplyRotationToVector(Vector3 vec, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vec;
    }
}