using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishConveyor : MonoBehaviour
{
    public Vector3 conveyorDirection = Vector3.left;

    List<Rigidbody> onConveyourList = new List<Rigidbody>();
    List<Rigidbody> toBeRemove = new List<Rigidbody>();

    void CheckForNull()
    {
        foreach (Rigidbody rb in onConveyourList)
        {
            if (rb == null)
            {
                toBeRemove.Add(rb);
            }
        }

        if (toBeRemove.Count > 0)
        {
            for (int i = 0; i < toBeRemove.Count; i++)
            {
                onConveyourList.Remove(toBeRemove[i]);
            }

            toBeRemove.Clear();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var rb = collision.gameObject.GetComponent<Rigidbody>();

        if (!onConveyourList.Contains(rb))
            onConveyourList.Add(rb);

        CheckForNull();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (onConveyourList.Count > 0)
        {
            foreach (var rb in onConveyourList)
            {
                rb.velocity = conveyorDirection;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        var rb = collision.gameObject.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;

        if (onConveyourList.Contains(rb))
            onConveyourList.Remove(collision.gameObject.GetComponent<Rigidbody>());
    }

    
}
