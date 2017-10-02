using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Chat : MonoBehaviour
{
    public Text chat;
    public InputField inputUser;

    private SocketIOComponent socket;

    public void SendMessage()
    {
        string from = "Unity Chat";
        string text = inputUser.text;

        EmitMessage(from, text);

        inputUser.text = "";
    }

    private void Awake()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Socket");
        socket = go.GetComponent<SocketIOComponent>();
    }

    private void Start()
    {
        socket.On("connect", (SocketIOEvent e) =>
        {
            Debug.Log(string.Format("[name: {0}, data: {1}]", e.name, e.data));
        });

        socket.On("newMessage", (SocketIOEvent e) =>
        {
            string aux = chat.text;

            chat.text = e.data["from"] + ": " + e.data["text"] + "\r\n" + aux;
        });
    }

    private void EmitMessage(string from, string text)
    {
        JSONObject obj;

        Dictionary<string, string> data = new Dictionary<string, string>();
        data["from"] = from;
        data["text"] = text;

        obj = new JSONObject(data);

        socket.Emit("createMessage", obj, OnMessage);
    }

    private void OnMessage(JSONObject obj)
    {
        
    }
}
