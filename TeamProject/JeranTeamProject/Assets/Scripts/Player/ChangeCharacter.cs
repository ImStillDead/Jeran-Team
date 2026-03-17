using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{
    [SerializeField] CharacterSelect character;
    public void OnTriggerEnter(Collider other)
    {
        ICharacters characterPick = other.GetComponent<ICharacters>();

        if (character != null)
        {
            characterPick.SwapCharacter(character);
            Destroy(gameObject);
        }
    }
}
