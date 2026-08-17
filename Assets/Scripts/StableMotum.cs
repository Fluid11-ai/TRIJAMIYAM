using UnityEngine;

public class StableMotum : MonoBehaviour
{
    [Header("Visual Feedback")]
    public GameObject successEffect;   // Optional particle / glow

    private bool activated = false;

    void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;
            Stabilize();
        }
    }

    void Stabilize()
    {
        Debug.Log("STABLE MOTUM REACHED");

        if (successEffect != null)
        {
            Instantiate(successEffect, transform.position, Quaternion.identity);
        }

        // Disable visual to show merge
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        // Notify game manager or UI
        GameManager.Instance.LevelComplete();
    }
}
