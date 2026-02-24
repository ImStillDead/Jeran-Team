using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "NewBoost", menuName = "Boost/Boost Data")]
public class boostscript : ScriptableObject
{

    public int healthrefill;
    public int ammorefill;
    public int shield;  //maybe? not sure yet.
    public float dmgBoost;
    public float speedBoost;

}

