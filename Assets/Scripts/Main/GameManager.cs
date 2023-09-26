using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BonusType
{
    SmallSize,
    WallSlide,
    AddTime
}

public class GameManager : MonoBehaviour
{
    public static BonusType starterBonus;
    public static string playerName;
}
