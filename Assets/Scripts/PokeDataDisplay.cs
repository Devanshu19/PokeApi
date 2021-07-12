using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokeDataDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text pokemonIdText;
    [SerializeField] TMP_Text pokemonNameText;
    [SerializeField] RawImage pokemonImage;
    [SerializeField] RectTransform typeHolder;
    [SerializeField] TMP_Text pokemonHeightText;
    [SerializeField] TMP_Text pokemonWeightText;

    private PokemonData fetchedData;
    private bool isFrontSpriteDisplayed = true;

    [SerializeField] List<GameObject> typesGO;
    [SerializeField] Dictionary<string, GameObject> typeDictionary = new Dictionary<string, GameObject>();

    [Space]

    [SerializeField] DisplayDataChannelSO displayDataChannelSO;

    private void Awake()
    {
        displayDataChannelSO.displayDataEvent += DisplayData;

        //Prepare Dictionary out of list
        MakeTypeDictionary(typesGO);
    }

    private void Start()
    {
        fetchedData = new PokemonData();
    }

    private void OnDisable()
    {
        displayDataChannelSO.displayDataEvent -= DisplayData;
    }

    //Makes a dictionary out of the list for faster object fetching
    private void MakeTypeDictionary(List<GameObject> typesGO)
    {
        foreach (GameObject g in typesGO)
        {
            typeDictionary.Add(g.name.ToLower(), g);
        }
    }

    //Display Data according to the pokemon data passed. If empty data is passed then reset the ui
    private void DisplayData(PokemonData pokemonData)
    {
        //Reset older data by using empty PokemonData;
        ResetUiDisplay(new PokemonData());

        //Display the fetched Data
        ResetUiDisplay(pokemonData);
    }

    private void ResetUiDisplay(PokemonData data)
    {
        fetchedData = data;

        pokemonIdText.text = fetchedData.id;
        pokemonNameText.text = fetchedData.name;
        pokemonImage.texture = fetchedData.frontSprite;
        pokemonHeightText.text = fetchedData.height;
        pokemonWeightText.text = fetchedData.weight;

        //If the pokemon data doesn't has any type badge then reset the older type badges
        if (data.types.Length <= 0)
        {
            for (int i = 0; i < typeHolder.childCount; i++)
            {
                GameObject.Destroy(typeHolder.GetChild(i).gameObject);
                Debug.Log("Removed Object");
            }
        }
        else
        {
            //Display the fetched type badges
            foreach (string typeName in fetchedData.types)
            {
                GameObject typeGO = typeDictionary[typeName];
                GameObject typeIns = Instantiate<GameObject>(typeGO);
                typeIns.transform.SetParent(typeHolder, false);
            }
        }
    }

    [ContextMenu("Switch Sprite")]
    public void SwitchSprite()
    {
        if(isFrontSpriteDisplayed == true)
        {
            pokemonImage.texture = fetchedData.backSprite;
            isFrontSpriteDisplayed = false;
        }
        else
        {
            pokemonImage.texture = fetchedData.frontSprite;
            isFrontSpriteDisplayed = true;
        }
    }
}
