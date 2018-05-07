using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonCollision : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        UnityEngine.Debug.Log("ENTER");
        Button collidedButton = other.gameObject.GetComponent<Button>();
        collidedButton.onClick.Invoke();
    }
}