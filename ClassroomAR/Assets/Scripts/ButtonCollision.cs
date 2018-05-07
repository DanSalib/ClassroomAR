using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonCollision : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Button collidedButton = other.gameObject.GetComponent<Button>();
        collidedButton.onClick.Invoke();
    }
}