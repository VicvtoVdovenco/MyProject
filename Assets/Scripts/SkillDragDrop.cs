using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDragDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static UnityEvent<float> skillUsed = new UnityEvent<float>();

    private GameObject dragObject;
    private Vector3 originalPosition;
    private GraphicRaycaster raycaster;
    [SerializeField] bool isTowerTargeted = false;
    [SerializeField] bool isAboveTower = false;
    private Tower tower = null;



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
        
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Tower"))
            {
                isAboveTower = false;

                if (!isTowerTargeted)
                {
                    tower = hit.collider.GetComponent<Tower>();
                    tower.TargetTower();
                    isTowerTargeted = true;
                }
                break;
            }
        }

        if (isTowerTargeted && isAboveTower)
        {
            if (tower != null) tower.UntargetTower();
            tower = null;
            isTowerTargeted = false;
        }

        isAboveTower = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == -1)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = eventData.position;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            bool isSkillPanel = false;
            foreach (RaycastResult result in results)
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

            if (isTowerTargeted && Player.Instance.PlayerCurrentMana >= 2)
            {
                tower.BounceShoot();
                skillUsed.Invoke(2);
                tower.UntargetTower();
                Destroy(gameObject);
            }

            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}

