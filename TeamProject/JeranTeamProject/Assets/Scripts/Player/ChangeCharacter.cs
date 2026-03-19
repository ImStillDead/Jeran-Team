using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{
    [SerializeField] CharacterSelect character;

    private void Start()
    {
        GameManager.instance.pickUpObjects.Add(gameObject);
    }
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
