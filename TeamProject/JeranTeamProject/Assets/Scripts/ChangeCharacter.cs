using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{
    [SerializeField] CharacterSelect character;

    public void OnTriggerEnter(Collider other)
    {
        PlayerController characterPick = other.GetComponent<PlayerController>();

        if (characterPick != null)
        {
            characterPick.SwapCharacter(character);
            Destroy(gameObject);
        }
    }
}
