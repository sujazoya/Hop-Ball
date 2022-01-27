using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSelecter : MonoBehaviour
{
    [SerializeField] int ballIndex;
      private void OnMouseDown()
    {
        Game.BallIndex = ballIndex;
        PlayerManager.Instance.CloseBallSelecter();
    }
}
