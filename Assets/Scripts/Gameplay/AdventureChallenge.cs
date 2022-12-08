using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureChallenge : MonoBehaviour
{
  [Header("Flavor")]
  public string Name;
  public string Flavor;

  [Header("Combat")]
  public int MaxWounds;
  public int RangedAttack;
  public int RangedDamage;
  public int MeleeAttack;
  public int MeleeDamage;
  public int MagicAttack;
  public int MagicDamage;


  private int _currentWounds;
}
