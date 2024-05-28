using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private GameObject          _buttonPrefab;
    [SerializeField] private DispatchFlag        _dispatchFlag = DispatchFlag.Ordered;


    [Header("Debug")]
    [SerializeField] private List<GameObject> _gameObjectInteractablesList = new List<GameObject>();

    private List<BoardButton>   _buttonsList = new List<BoardButton>();
    private Room                _currentRoom;
    private List<IInteractable> _currentInteractablesList = new List<IInteractable>();

    public enum DispatchFlag
    {
        Random,
        Ordered,
        InverseOrdered,
    }

    private DispatchFlag _lastDispatchFlag;

    void Start()
    {
        _lastDispatchFlag = _dispatchFlag;

        if (_gameObjectInteractablesList != null)
        {
            foreach (GameObject go in _gameObjectInteractablesList)
            {
                IInteractable interactable = go.GetComponentInChildren<IInteractable>();

                if (interactable != null)
                {
                    _currentInteractablesList.Add(interactable);
                }
                else
                {
                    Debug.LogWarning($"[Board::Start] GameObject given in list of interactables is no of type IIteractable. name : {go.name}.");
                }
            }
        }

        _buttonsList = new List<BoardButton>();

        int idIndex = 0;
        foreach (Transform child in transform)
        {
            BoardButton buttonComponent = child.GetComponent<BoardButton>();

            if (buttonComponent != null)
            {
                buttonComponent.Activated = false;
                buttonComponent.Id = "BoardButton_" + (++idIndex).ToString();

                _buttonsList.Add(buttonComponent);

                Debug.Log($"[Board::Start] Button {buttonComponent.Id} added to Board.");
            }
            else
            {
                /*
                     * InteractableTest testInteraction = child.GetComponent<InteractableTest>();
                    if (testInteraction != null)
                    {
                        _interactableManager.AddInteractable(testInteraction);
                        Debug.Log("[Board::Start] InteractableTest added to interactable manager");
                    }
                    */
            }
        }

        DispatchInteractiblesToButtons();
    }

    void Update()
    {
        if (_lastDispatchFlag != _dispatchFlag)
        {
            DispatchInteractiblesToButtons();
            _lastDispatchFlag = _dispatchFlag;
        }
    }

    /// <summary>
    /// Distributes interactable IDs to buttons based on the specified dispatch strategy.
    /// This method determines how interactable IDs are assigned to buttons in the `buttonList`
    /// by evaluating the `dispatchFlag`. The assignment can be random, ordered, or inverse-ordered,
    /// enabling dynamic interaction setups for the buttons.
    /// </summary>
    public void DispatchInteractiblesToButtons()
    {
        if (_currentInteractablesList == null)
        {
            Debug.LogWarning($"[Board::AssignOrderedInteractibles] Interactable list null.");
            return;
        }

        foreach (BoardButton button in _buttonsList)
        {
            button.Activated = false;
        }

        switch (_dispatchFlag)
        {
            case DispatchFlag.Random:

                AssignRandomInteractibles();
                break;

            case DispatchFlag.Ordered:

                AssignOrderedInteractibles();
                break;

            case DispatchFlag.InverseOrdered:

                AssignInverseOrderedInteractibles();
                break;

            default:

                Debug.LogError("[Board::DispatchInteractiblesToButtons] Invalid dispatch flag set.");
                return;
        }

        Debug.Log($"[Board::DispatchInteractiblesToButtons] Interactibles dispatched to buttons : {_dispatchFlag.ToString()}.");
    }

    private void AssignRandomInteractibles()
    {
        System.Random rnd = new System.Random();
        List<IInteractable> shuffledInteractables = new List<IInteractable>(_currentInteractablesList);

        int n = shuffledInteractables.Count;

        while (n > 1)
        {
            int k = rnd.Next(n--);
            IInteractable value = shuffledInteractables[k];
            shuffledInteractables[k] = shuffledInteractables[n];
            shuffledInteractables[n] = value;
        }

        int buttonCount = _buttonsList.Count;

        for (int i = 0; i < shuffledInteractables.Count; i++)
        {
            if (i < buttonCount)
            {
                BoardButton button = _buttonsList[i];
                if (button != null)
                {
                    button.interactablesList = new List<IInteractable> { shuffledInteractables[i] };
                    button.Activated = true;
                }
            }
        }
    }

    private void AssignOrderedInteractibles()
    {
        int buttonCount = _buttonsList.Count;

        for (int i = 0; i < _currentInteractablesList.Count; i++)
        {
            if (i < buttonCount)
            {
                BoardButton button = _buttonsList[i];
                if (button != null)
                {
                    button.interactablesList = new List<IInteractable> { _currentInteractablesList[i] };
                    button.Activated = true;
                }
            }
        }
    }

    private void AssignInverseOrderedInteractibles()
    {
        List<IInteractable> tmpInteractablesList = _currentInteractablesList;
        tmpInteractablesList.Reverse();

        int buttonCount = _buttonsList.Count;

        for (int i = 0; i < tmpInteractablesList.Count; i++)
        {
            if (i < buttonCount)
            {
                BoardButton button = _buttonsList[i];
                if (button != null)
                {
                    button.interactablesList = new List<IInteractable> { tmpInteractablesList[i] };
                    button.Activated = true;
                }
            }
        }
    }

    public Room Room
    {
        get { return _currentRoom; }
        set
        {
            _currentRoom = value;
            _currentInteractablesList = _currentRoom.InteractablesList;
            DispatchInteractiblesToButtons();
        }
    }

    //public int CurrentRoomIndex
    //{
    //    get { return _currentRoomIndex; }
    //    set { _currentRoomIndex = value; }
    //}
}