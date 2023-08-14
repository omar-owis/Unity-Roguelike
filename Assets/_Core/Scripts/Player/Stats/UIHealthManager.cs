using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHealthManager : MonoBehaviour
{
    public Character character;

    public GameObject heart;
    public List<Image> hearts;


    private void Awake()
    {
        if(character != null && character.CharacterData != null)
            DisplayHearts((int)character.CharacterData.MaxHealth);
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        if (character != null && character.CharacterData != null)
            character.CharacterData.OnCharacterDamage += UpdateHealth;
    }
    void OnDisable()
    {
        if (character != null && character.CharacterData != null)
            character.CharacterData.OnCharacterDamage -= UpdateHealth;
    }

    void UpdateHealth(float currentHealth)
    {
        float heartFill = currentHealth;

        foreach(Image i in hearts)
        {
            i.fillAmount = heartFill;
            heartFill -= 1;
        }

    }

    void DisplayHearts(int numOfHearts)
    {
        foreach (Image i in hearts)
        {
            Destroy(i.gameObject);
        }

        hearts.Clear();

        for (int i = 0; i < numOfHearts; i++)
        {
            GameObject h = Instantiate(heart, this.transform);
            hearts.Add(h.GetComponent<Image>());
        }
    }
}
