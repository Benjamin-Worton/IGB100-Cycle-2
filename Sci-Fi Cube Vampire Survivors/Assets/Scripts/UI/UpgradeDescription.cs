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
    Debug.Log("In");
}

    public void OnPointerExit(PointerEventData eventData)
    {
        if (prefabToShow != null)
        {
            prefabToShow.SetActive(false);
        }

        Debug.Log("Out");
    }
}