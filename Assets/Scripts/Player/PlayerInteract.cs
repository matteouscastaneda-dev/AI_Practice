using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance == null)
        {
            Debug.Log("GameManager null");
            return;
        }

        if (other.CompareTag("Key"))
        {
            GameManager.Instance.HasKey = true;
            other.gameObject.SetActive(false);
            return;
        }

        if (other.CompareTag("Exit"))
        {
            GameManager.Instance.TryExit();
        }
    }
}
