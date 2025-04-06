using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    void Start()
    {
        inputReader.MoveEvent += HandleMove;
    }

    private void HandleMove(Vector2 move)
    {
        Debug.Log(move);
    }

    private void OnDestroy()
    {
        inputReader.MoveEvent -= HandleMove;
    }
}
