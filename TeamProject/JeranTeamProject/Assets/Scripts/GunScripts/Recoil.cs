using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    //Hipfire
    public RecoilScriptable recoil;
    void Start()
    {
        recoil = ScriptableObject.CreateInstance<RecoilScriptable>();
        recoil.X = 2;
        recoil.Y = 2;
        recoil.Z = 0;
        recoil.returnSpeed = 6;
        recoil.snappiness = 8;
    }
    public void UpdateRecoil(RecoilScriptable recoilCall)
    {
        if (recoilCall != null)
        {
            recoil = recoilCall;
        }
    }
    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, recoil.returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, recoil.snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire()
    {
        targetRotation += new Vector3(recoil.X, Random.Range(-recoil.Y, recoil.Y), Random.Range(-recoil.Z, recoil.Z));
    }
}
