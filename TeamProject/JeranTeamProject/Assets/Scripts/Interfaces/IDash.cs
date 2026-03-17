using UnityEngine;

public interface IDash
{
    void StartDash();
    bool IsDashing();
    float GetDashRemainingCooldown();
}
