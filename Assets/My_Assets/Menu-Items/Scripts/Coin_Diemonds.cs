using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_Diemonds : MonoBehaviour
{
    public enum DiemondType
    {
        tri,quad,coin
    }
    public enum DiemondColour
    {
        red,green,blue,pink,yellow,blue_dark
    }

    public DiemondType diemondType;
    public DiemondColour diemondColour;
}
