using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WitchRequest : MonoBehaviour
{
    [SerializeField] private string _middleNameConvention = "Mid";

    private List<Transform> _dialogueMids = new List<Transform>();

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

        gameObject.SetActive(false);
    }

    public void UpdateRequest(IngredientsScriptableObject[] ingredients)
    {
        var ingredientsCount = ingredients.Length;
        if (ingredientsCount > 0)
        {
            gameObject.SetActive(true);

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
        } else
        {
            gameObject.SetActive(false);
        }
    }
}
