using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UILogin : MonoBehaviour
{
    [SerializeField] private Button loginButton;

    //[SerializeField] private TMP_Text userIdText;
    [SerializeField] private TMP_Text userNameText;

    [SerializeField] private Transform loginPanel, userPanel;

    [SerializeField] private LoginController loginController;

    private PlayerProfile playerProfile;

    private async void Start()
    {
        loginButton.onClick.AddListener(LoginButtonPressed);
        loginController.OnSignedIn += LoginController_OnSignedIn;

        // If already signed in, immediately update the UI
        if (Unity.Services.Authentication.AuthenticationService.Instance.IsSignedIn)
        {
            var profile = loginController.PlayerProfile;
            // If name is not yet populated, wait for the name to be fetched
            if (string.IsNullOrEmpty(profile.Name))
            {
                var name = await Unity.Services.Authentication.AuthenticationService.Instance.GetPlayerNameAsync();
                profile.Name = name;
            }

            LoginController_OnSignedIn(profile);
        }
    }


    private void OnDisable()
    {
        loginButton.onClick.RemoveListener(LoginButtonPressed);
        loginController.OnSignedIn -= LoginController_OnSignedIn;
        //loginController.OnAvatarUpdate -= LoginController_OnAvatarUpdate;
    }

    private async void LoginButtonPressed()
    {
        await loginController.InitSignIn();
    }

    private void LoginController_OnSignedIn(PlayerProfile profile)
    {
        playerProfile = profile;
        loginPanel.gameObject.SetActive(false);
        userPanel.gameObject.SetActive(true);

        //userIdText.text = $"id_{playerProfile.playerInfo.Id}";
        userNameText.text = profile.Name;
    }
    private void LoginController_OnAvatarUpdate(PlayerProfile profile)
    {
        playerProfile = profile;
    }


}