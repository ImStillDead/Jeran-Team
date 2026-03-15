using UnityEngine;


[CreateAssetMenu]
public class Bullet : ScriptableObject
{
    [Range(1 , 100)] public int damageAmount;
    [Range(1, 10)] public float damageRate;
    public int speed;
    [Range(1, 500)] public float destroyTime;
    public ParticleSystem hitEffect;
}
