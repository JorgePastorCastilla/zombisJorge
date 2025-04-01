using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour, IOnEventCallback
{
    
    public TMP_InputField inputField;
    public GameObject messagePrefab;

    public Transform Content;
    
    public PhotonView photonView;

    public PlayerMovement playerMovement;
    
    public const byte SEND_MESSAGE_EVENT = 1;

    private void Update()
    {
        if (photonView.IsMine && Input.GetKeyUp(KeyCode.Return))
        {
            
            if (inputField.gameObject.activeSelf)
            {
                //Desactivar y enviar
                playerMovement.enabled = true;
                if (inputField.text.Length > 0)
                {
                    SendMessage();    
                }
                inputField.gameObject.SetActive(false);
            }
            else
            {
                //Activar y focus
                playerMovement.enabled = false;
                inputField.gameObject.SetActive(true);
                inputField.Select();
                inputField.ActivateInputField();
            }
        }
        
    }

    public void SendMessage()
    {
        // photonView.RPC("GetMessage", RpcTarget.All, inputField.text);
        RaiseEventOptions raiseOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };

        PhotonNetwork.RaiseEvent(SEND_MESSAGE_EVENT, inputField.text, raiseOptions, sendOptions);
        inputField.text = "";
    }
    
    public void GetMessage(string RecieveMessage)
    {
        GameObject newMessage = Instantiate(messagePrefab, Vector3.zero, Quaternion.identity, Content);
        newMessage.transform.localPosition = Vector3.zero;
        newMessage.transform.localRotation = new Quaternion(0,0,0,0);
        newMessage.GetComponent<TextMeshProUGUI>().text = RecieveMessage;
        newMessage.transform.SetAsFirstSibling();
    }
    
    void IOnEventCallback.OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == SEND_MESSAGE_EVENT)
        {
            string messageText = photonEvent.CustomData.ToString();
            GetMessage(messageText);
            
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
