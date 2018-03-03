using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class Login_to_PlayFab : MonoBehaviour {


    public Canvas Canvas_login;

    public Canvas Canvas_chooseVillage;

    public Canvas Canvas_createCharacter;

    public Canvas Canvas_gotCharacter;

    public PlayerData PlayerData;

    public InputField IF_username;

    public InputField IF_password;

    public InputField IE_characterName;

    public Text Text_characterName;

    public Text Text_villageName;

    public Button Button_createCharacter;

    List<string> Villages = new List<string>();

    private int villageID = 0;


    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////
    /// </summary>


    private void Start()
    {
        Canvas_login.enabled = true;

        Canvas_createCharacter.enabled = false;

        Canvas_gotCharacter.enabled = false;

        Canvas_chooseVillage.enabled = false;
        
        Villages.Add("Iwa");
        Villages.Add("Kiri");
        Villages.Add("Konoha");
        Villages.Add("Suna");
        Villages.Add("Kumo");
    }

    public void LoginPlayFab()
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest()
        {
            Username = IF_username.text,
            Password = IF_password.text
        },

        result => {

           Debug.Log("Successfully logged!");

            GetPlayerCharacters(result.PlayFabId);
        },

        error => {

           Debug.Log(error.GenerateErrorReport());
        });
    }

    public void RegisterPlayFab()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest()
        {
            Username = IF_username.text,

            Password = IF_password.text,

            Email = IF_username.text + "@gmail.com"
        },

        result => {

            Debug.Log("Successfully registered!");

            Konoha();
            Iwa();
            Suna();
            Kiri();
            Kumo();
        },

        error => {

           Debug.Log(error.GenerateErrorReport());
        });
    }

    public void CreateCharacter()
    {
        Button_createCharacter.interactable = false;

        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest()
        {
            CharacterName = IE_characterName.text,
            ItemId = Villages[villageID]
        },

       result => {

           Debug.Log("Successfully created character!");

           Canvas_createCharacter.enabled = false;

           Canvas_gotCharacter.enabled = true;

           Text_characterName.text = IE_characterName.text;

           PlayerData.characterID = result.CharacterId;                    //przypisanie Character Id do DataCore
           PlayerData.characterName = IE_characterName.text;               //przypisanie Character Name do DataCore
           PlayerData.characterVillage = result.CharacterType;             //przypisanie Character Type do DataCore      
       },

       error => {

           Debug.Log(error.GenerateErrorReport());
       });
    }

    public void DeleteCharacter()
    {
        Button_createCharacter.interactable = true;

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "deleteCharacter", // Arbitrary function name (must exist in your uploaded cloud.js file)
            FunctionParameter = new { characterID = PlayerData.characterID }, // The parameter provided to your function
            GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
        },

       result => {

           Debug.Log("Character deleted");

           Canvas_login.enabled = false;

           Canvas_createCharacter.enabled = false;

           Canvas_gotCharacter.enabled = false;

           Canvas_chooseVillage.enabled = true;
       },

       error => {
           Debug.Log(error.GenerateErrorReport());
       });
    }

    public void BackToLogin()
    {
        villageID = 0;

        Canvas_login.enabled = true;

        Canvas_createCharacter.enabled = false;

        Canvas_gotCharacter.enabled = false;

        Canvas_chooseVillage.enabled = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void GetPlayerCharacters(string _playFabID)
    {
        PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest()
        {
            PlayFabId = _playFabID
        },

        result => {

            if (result.Characters.Count == 1)
            {
                GetCharacterData(result.Characters[0].CharacterId);

                Debug.Log("Got player character!");

                PlayerData.characterID = result.Characters[0].CharacterId;                    //przypisanie Character Id do DataCore
                PlayerData.characterName = result.Characters[0].CharacterName;                //przypisanie Character Name do DataCore
                PlayerData.characterVillage = result.Characters[0].CharacterType;             //przypisanie Character Type do DataCore      

                Text_characterName.text = result.Characters[0].CharacterName;
            }

            else
            {
                Debug.Log("No character created");

                Canvas_login.enabled = false;

                Canvas_createCharacter.enabled = false;

                Canvas_gotCharacter.enabled = false;

                Canvas_chooseVillage.enabled = true;
            }
        },

        error => {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    private void GetCharacterData(string characterID)
    {
        PlayFabClientAPI.GetCharacterData(new GetCharacterDataRequest()
        {
            CharacterId = characterID
        },

        result => {

            Canvas_login.enabled = false;

            Canvas_createCharacter.enabled = false;

            Canvas_gotCharacter.enabled = true;

            Canvas_chooseVillage.enabled = false;

            /*  string[] index = (result.Data["look_code"].Value).Split(","[0]);
              string[] index_stats = (result.Data["stats_code"].Value).Split(","[0]);

              playerData.sex = byte.Parse(index[0]);                          
              playerData.hair_id = byte.Parse(index[1]);
              playerData.hair_color_id = byte.Parse(index[2]);
              playerData.eyes_color_id = byte.Parse(index[3]);

              playerData.STR = int.Parse(index_stats[0]);
              playerData.INTL = int.Parse(index_stats[1]);
              playerData.DEX = int.Parse(index_stats[2]);
              playerData.exp = float.Parse(index_stats[4]);
              playerData.lvl = int.Parse(index_stats[3]);
              playerData.gold = int.Parse(index_stats[5]);

              playerData.items_code = result.Data["items_code"].Value;
              playerData.quests_code = result.Data["quests_code"].Value;

              canvas_login.SetActive(false);
              canvas_character.SetActive(true);
              background.SetActive(false);
              rain.SetActive(false);*/
        },

        error => {
            Debug.Log(error.GenerateErrorReport());
        });
    }


    // Buy Character Tokens
    #region
    private void Konoha()
    {
        PurchaseItemRequest request = new PurchaseItemRequest();
        request.ItemId = "Konoha";
        request.VirtualCurrency = "DM";
        request.Price = 0;
        PlayFabClientAPI.PurchaseItem(request, OnPurchaseSuccess, OnPlayfabError);
    }
    private void Iwa()
    {
        PurchaseItemRequest request = new PurchaseItemRequest();
        request.ItemId = "Iwa";
        request.VirtualCurrency = "DM";
        request.Price = 0;
        PlayFabClientAPI.PurchaseItem(request, OnPurchaseSuccess, OnPlayfabError);
    }
    private void Suna()
    {
        PurchaseItemRequest request = new PurchaseItemRequest();
        request.ItemId = "Suna";
        request.VirtualCurrency = "DM";
        request.Price = 0;
        PlayFabClientAPI.PurchaseItem(request, OnPurchaseSuccess, OnPlayfabError);
    }
    private void Kiri()
    {
        PurchaseItemRequest request = new PurchaseItemRequest();
        request.ItemId = "Kiri";
        request.VirtualCurrency = "DM";
        request.Price = 0;
        PlayFabClientAPI.PurchaseItem(request, OnPurchaseSuccess, OnPlayfabError);
    }
    private void Kumo()
    {
        PurchaseItemRequest request = new PurchaseItemRequest();
        request.ItemId = "Kumo";
        request.VirtualCurrency = "DM";
        request.Price = 0;
        PlayFabClientAPI.PurchaseItem(request, OnPurchaseSuccess, OnPlayfabError);
    }

    private void OnPurchaseSuccess(PurchaseItemResult result) { }
    private void OnPlayfabError(PlayFabError error) { }
    #endregion


    // Character Creator Functions
    /////////////////////////////////////////////////////////////////

    public void NextVillage()
    {
        villageID++;

        if (villageID == 5) villageID = 0;

        Text_villageName.text = Villages[villageID];
    }

    public void PreviousVillage()
    {
        villageID--;

        if (villageID == -1) villageID = 4;

        Text_villageName.text = Villages[villageID];        
    }

    public void SelectVillage()
    {
        Canvas_login.enabled = false;

        Canvas_createCharacter.enabled = true;

        Canvas_gotCharacter.enabled = false;

        Canvas_chooseVillage.enabled = false;
    }
}
