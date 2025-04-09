using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] private int damage = 20;

    private ulong ownerClientId;

    public void SetOwner(ulong ownerClientId)
    {
        this.ownerClientId = ownerClientId;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody == null)
        {
            return;
        }
        if (other.attachedRigidbody.TryGetComponent<NetworkObject>(out NetworkObject netObj))
        {
            if (ownerClientId == netObj.OwnerClientId)
            {
                return;
            }
        }

        if (other.attachedRigidbody.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damage);
        }
    }
}
