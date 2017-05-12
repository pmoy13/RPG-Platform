using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChatFilter
{
    // Define the different channels available.
    public const int CHANNEL_COMBAT = 0;
    public const int CHANNEL_CHAT = 1;
    public const int CHANNEL_LOOT = 2;
    public const int CHANNEL_SYSTEM = 3;

    // Define the color of the update text - this is
    // the color the chat tab will be when the user
    // has received a message on the filter but not
    // checked the filter yet.
    public const string TEXT_UPDATE_COLOR = "<color=#ff0000ff>";

    // Define the color of the read text - the color of
    // the chat tab that the user will see normally,
    // when no new messages are waiting on the filter.
    public const string TEXT_NORMAL_COLOR = "<color=#ffffffff>";

    // A reference to the chat tab which corresponds to
    // this filter. When updating the filter, if the
    // filter is not the currently selected filter then
    // we need to change the color of its chat tab text.
    public Text ChatTabText;

    // Define which channels this filter accepts.
    private List<int> _acceptedChannels;

    // The name of the filter, which will be displayed
    // as the chat tab text.
    private string _name;

    // Allocate space for storing messages.
    private List<ChatMessage> _messagesReceived;

    public ChatFilter(string filterName, params int[] channels)
    {
        _acceptedChannels = new List<int>();
        _messagesReceived = new List<ChatMessage>();
        _name = filterName;

        for (int index = 0; index < channels.Length; index++)
        {
            _acceptedChannels.Add(channels[index]);
        }
    }

    /*
     * Method:
     *   DisplayMessages
     * 
     * Description:
     *   Returns a string with the concatenation of
     *   all of the stored messages this filter has
     *   received.
     */
    public string DisplayMessages()
    {
        string allMessages = "";

        foreach (ChatMessage message in _messagesReceived)
        {
            // Add the current message plus a newline so the
            // messages will be separated.
            allMessages += message.DisplayMessage() + "\n";
        }

        return allMessages;
    }

    /*
     * Method:
     *   ReceiveMessage
     * 
     * Description:
     *   Checks to see if the input message's Channel
     *   ID matches one of the filter's accepted
     *   Channel IDs. If so, saves the message. Returns
     *   true if the message was accepted and the filter
     *   needs to be updated, and false if the message
     *   was rejected.
     */
    public bool ReceiveMessage(ChatMessage message)
    {
        if (_acceptedChannels.Contains(message.ChannelID))
        {
            // The incoming message has a channel ID that
            // matches one of the channels this filter displays,
            // so save the message.
            _messagesReceived.Add(message);

            // Report that this filter needs to be updated.
            return true;
        }

        // The message was rejected, and we don't need to update
        // the filter.
        return false;
    }

    /*
     * Method:
     *   UpdateFilter
     * 
     * Description:
     *   Updates the color of chat tab corresponding to this channel,
     *   so that the user knows a message has been received on this
     *   channel that they have not seen yet.
     */
    public void UpdateFilter()
    {
        ChatTabText.text = TEXT_UPDATE_COLOR + _name;
    }
}
