using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fruit", menuName = "Fruit Type")]
public class FruitType : ScriptableObject
{
    public string fruitName;
    public int fruitValue;
    public Sprite fruitSprite;
    public int stacks;
}
