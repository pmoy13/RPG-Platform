using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChatHandler : NetworkBehaviour
{
    // Each message needs an ID, so that the receiver knows which
    // handler to call.
    public static short CHAT_MESSAGE = MsgType.Highest + 1;

    [SerializeField] private MessageDisplay _messageDisplay;

    private NetworkClient _networkClient;

    // Use this for initialization
    void Start ()
    {
        NetworkManager networkManager = GameObject.FindObjectOfType<NetworkManager>();
        _networkClient = networkManager.client;

        if (_networkClient.isConnected)
        {
            _networkClient.RegisterHandler(CHAT_MESSAGE, ClientReceiveMessage);
        }
        if (isServer)
        {
            NetworkServer.RegisterHandler(CHAT_MESSAGE, ServerReceiveMessage);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SendChatMessage(string message)
    {
        ChatMessage chatMessage = ChatMessage.FromString(message);

        _networkClient.Send(CHAT_MESSAGE, chatMessage);
    }

    public void ClientReceiveMessage(NetworkMessage message)
    {
        // :TODO: Need to find a way to filter and only show the messages
        // directed to our Target ID.
        Debug.Log("ClientReceiveMessage");
        ChatMessage chatMessage = message.ReadMessage<ChatMessage>();

        _messageDisplay.ReceiveMessage(chatMessage);
    }

    public void ServerReceiveMessage(NetworkMessage message)
    {
        // All the server needs to do is turn around and
        // send the message to all of the clients.
        Debug.Log("ServerReceiveMessage");
        ChatMessage chatMessage = message.ReadMessage<ChatMessage>();
        NetworkServer.SendToAll(CHAT_MESSAGE, chatMessage);
    }
}

public class ChatMessage : MessageBase
{
    // Define the different types of messages.
    public const int TYPE_SYSTEM = 0;
    public const int TYPE_WHISPER = 1;
    public const int TYPE_PUBLIC = 2;

    // Define the rich text values for different colors.
    public const string TEXT_GREEN = "<color=#008000ff>";
    public const string TEXT_YELLOW = "<color=#ffff00ff>";
    public const string TEXT_PURPLE = "<color=#ff00ffff>";
    public const string END_COLOR = "</color>";

    public int ChannelID;

    private string _message;
    private int _messageType;
    private string _targetID;
    private string _sourceID;

    public ChatMessage() { }

    public ChatMessage(string message, int channelID,
                       int type, string to, string from)
    {
        // Assign all of the message fields.
        _message = message;
        ChannelID = channelID;
        _messageType = type;
        _targetID = to;
        _sourceID = from;
    }

    public static ChatMessage FromString(string message)
    {
        // Parse the string to determine the fields.

        // :TODO: Implement this method.
        return new ChatMessage(message, ChatFilter.CHANNEL_SYSTEM,
            TYPE_SYSTEM, "TO: ", "FROM: ");
    }

    /*
     * Method:
     *   DisplayMessage
     * 
     * Description:
     *   This takes the ChatMessage's text, and formats
     *   it appropriately for display on the screen. This
     *   formatting includes coloring the text, as well as
     *   displaying the player the message is to or from.
     */
    public string DisplayMessage()
    {
        string finalMessage;

        // Format the color and message appropriately, based on the
        // message type. Note that the cases are exhaustive, and the
        // default should never be reached.
        switch (_messageType)
        {
            case TYPE_PUBLIC:
                {
                    // Public messages appear green, and have no single
                    // recipient.
                    finalMessage = TEXT_GREEN + _sourceID + ": " + _message + END_COLOR;
                    break;
                }
            case TYPE_SYSTEM:
                {
                    // System messages appear yellow, and have no single
                    // recipient nor source.
                    finalMessage = TEXT_YELLOW + _message + END_COLOR;
                    break;
                }
            case TYPE_WHISPER:
                {
                    // Whispers appear purple, and are direct from one
                    // source to one destination.
                    finalMessage = TEXT_PURPLE + "[" + _sourceID + "]: " + _message + END_COLOR;
                    break;
                }
            default:
                {
                    // This case should never be reached. If it is, return
                    // a message with no content.
                    finalMessage = "";
                    break;
                }
        }

        return finalMessage;
    }

    // This method would be generated
    public override void Deserialize(NetworkReader reader)
    {
        ChannelID = reader.ReadInt32();
        _message = reader.ReadString();
        _messageType = reader.ReadInt32();
        _targetID = reader.ReadString();
        _sourceID = reader.ReadString();
    }

    // This method would be generated
    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(ChannelID);
        writer.Write(_message);
        writer.Write(_messageType);
        writer.Write(_targetID);
        writer.Write(_sourceID);
    }
}
