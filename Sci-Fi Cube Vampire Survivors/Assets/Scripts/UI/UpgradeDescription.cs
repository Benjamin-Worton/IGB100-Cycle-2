using UnityEngine;
using UnityEngine.EventSystems;

public class ShowOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject prefabToShow;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (prefabToShow != null)
        {
            prefabToShow.SetActive(true);
            prefabToShow.transform.SetAsLastSibling(); // Brings to front
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (prefabToShow != null)
        {
            prefabToShow.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && prefabToShow != null)
        {
            prefabToShow.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (prefabToShow != null)
        {
            prefabToShow.SetActive(false);
        }
    }
}