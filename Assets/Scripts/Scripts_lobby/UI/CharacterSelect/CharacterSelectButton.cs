using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject disabledOverlay;
    [SerializeField] private UnityEngine.UI.Button button;
    [SerializeField] private AudioSource audioSourceRobert;
    [SerializeField] private AudioSource audioSourceCamGuy;

    private CharacterSelectDisplay characterSelect;

    public Character Character { get; private set; }
    public bool IsDisabled { get; private set; }

    public void SetCharacter(CharacterSelectDisplay characterSelect, Character character)
    {
        iconImage.sprite = character.Icon;

        this.characterSelect = characterSelect;
        
        Character = character;
    }

    public void SelectCharacter()
    {
        characterSelect.Select(Character);

        if (audioSourceCamGuy == null || audioSourceRobert == null)
        {
            Debug.LogWarning($"[CharacterSelectButton::SelectCharacter] audio source is null.");
            return;
        }

        Debug.Log($"[CharacterSelection] id : {Character.Id}");

        if (Character.Id == 1)
        {
            audioSourceRobert.Play();
            audioSourceCamGuy.Pause();
        }
        if (Character.Id == 2)
        {
            audioSourceCamGuy.Play();
            audioSourceRobert.Pause();
        }
    }

    public void SetDisabled()
    {
        IsDisabled = true;
        disabledOverlay.SetActive(true);
        button.interactable = false;
    }

    public AudioSource CamGuyOST
    {
        get { return audioSourceCamGuy; }
        set { audioSourceCamGuy = value; }
    }

    public AudioSource RobertOST
    {
        get { return audioSourceRobert; }
        set { audioSourceRobert = value; }
    }
}
