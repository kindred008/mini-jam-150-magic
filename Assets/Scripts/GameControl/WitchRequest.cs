using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WitchRequest : MonoBehaviour
{
    [SerializeField] private string _middleNameConvention = "Mid";
    
    private List<Transform> _dialogueMids = new List<Transform>();

    [SerializeField] private Transform _dialogueLeft;
    [SerializeField] private Transform _dialogueRight;

    private void Awake()
    {
        GameController.GameOver.AddListener(HandleGameOver);
        GameController.Pause.AddListener(HandlePaused);
    }

    private void OnDestroy()
    {
        GameController.GameOver.RemoveListener(HandleGameOver);
        GameController.Pause.RemoveListener(HandlePaused);
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.name.Contains(_middleNameConvention))
            {
                _dialogueMids.Add(child);
            }
        }

        EnableDialogue(false);
    }

    private void EnableDialogue(bool enable)
    {
        _dialogueLeft.gameObject.SetActive(enable);
        _dialogueRight.gameObject.SetActive(enable);
    }

    public void UpdateRequest(IngredientsScriptableObject[] ingredients)
    {
        var ingredientsCount = ingredients.Length;
        
        EnableDialogue(ingredientsCount > 0);

        for (int i = 0; i < _dialogueMids.Count; i++)
        {
            var dialogueMid = _dialogueMids[i];
            if (i + 1 <= ingredientsCount)
            {
                dialogueMid.gameObject.SetActive(true);

                dialogueMid.GetComponent<DialogueSection>().Image.sprite = ingredients[i].Graphic;
            } else
            {
                dialogueMid.gameObject.SetActive(false);
            }
        }
        
    }

    private void HandleGameOver()
    {
        gameObject.SetActive(false);
    }

    private void HandlePaused(bool paused)
    {
        gameObject.SetActive(!paused);
    }
}
