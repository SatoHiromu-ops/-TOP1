
using UnityEngine;
using UnityEngine.InputSystem;
public class ItemPickup : MonoBehaviour
{
    public GameObject pickupHint;

    private GameObject currentItem;

    void Start()
    {
        pickupHint.SetActive(false);
    }

    void Update()
    {
        if (currentItem != null && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Destroy(currentItem);
            currentItem = null;
            pickupHint.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            currentItem = other.gameObject;
            pickupHint.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentItem)
        {
            currentItem = null;
            pickupHint.SetActive(false);
        }
    }
}
