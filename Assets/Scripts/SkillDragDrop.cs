using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDragDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private GameObject dragObject;
    private Vector3 originalPosition;
    private GraphicRaycaster raycaster;

    private void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerId == -1)
        {
            List<RaycastResult> results = new List<RaycastResult>();

            raycaster.Raycast(eventData, results);

            if (results.Count > 0 && results[0].gameObject == gameObject)
            {
                dragObject = gameObject;
                originalPosition = transform.position;

                EventSystem.current.SetSelectedGameObject(gameObject);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragObject != null && eventData.pointerId == -1)
        {
            transform.position = eventData.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == -1)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = eventData.position;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            bool isSkillPanel = false;
            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject.CompareTag("Skill Panel"))
                {
                    isSkillPanel = true;
                    break;
                }
            }

            if (isSkillPanel)
            {
                transform.position = originalPosition;
            }
            else
            {
                Destroy(gameObject);
            }

            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}

