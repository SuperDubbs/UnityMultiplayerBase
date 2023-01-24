using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class NetworkUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private TextMeshProUGUI joinCodeDisplay;
    [SerializeField] private TMP_InputField joinCodeInput;
    [SerializeField] private Relay relay;
    
    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            relay.CreateRelay();
        });

        joinButton.onClick.AddListener(() =>
        {
            string text = joinCodeInput.text;
            relay.JoinRelay(text);
            Debug.Log(text);
            Debug.Log(text.Length);
        });

        relay.onCreateRelay.AddListener(updateJoinCodeDisplay);
        //relay.onJoinRelay.AddListener(addClient);
    }
    
    public void updateJoinCodeDisplay()
    {
        joinCodeDisplay.text = relay.joinCode;
    }

    public void addClient()
    {
        NetworkManager.Singleton.StartClient();
    }

}
