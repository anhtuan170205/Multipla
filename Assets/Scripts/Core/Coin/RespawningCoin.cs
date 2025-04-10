using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RespawningCoin : Coin
{
    public event Action<RespawningCoin> OnCollected;
    Vector3 previousPosition;
    public override int Collect()
    {
        if (!IsServer)
        {
            Show(false);
            return 0;
        }
        if (isCollected)
        {
            return 0;
        }
        isCollected = true;
        OnCollected?.Invoke(this);
        return coinValue;
    }
    public void Reset()
    {
        isCollected = false;
    }

    private void Update()
    {
        if (transform.position != previousPosition)
        {
            Show(true);
        }
        previousPosition = transform.position;
    }
}
