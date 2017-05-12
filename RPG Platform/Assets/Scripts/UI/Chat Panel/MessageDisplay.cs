using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageDisplay : MonoBehaviour
{
    // The text field that the user sees received messages in.
    [SerializeField] private Text _text;

    private List<ChatFilter> _filters;
    private ChatFilter _selectedFilter;

	// Use this for initialization
	private void Start ()
    {
        _filters = new List<ChatFilter>();

        // :TODO: Remove this test code.
		_filters.Add(new ChatFilter("TEST", ChatFilter.CHANNEL_SYSTEM));
        _selectedFilter = _filters[0];
    }

    public void ReceiveMessage(ChatMessage message)
    {
        // Loop through each of the created filters and
        // see if the filter should receive the message.
        foreach (ChatFilter filter in _filters)
        {
            if (filter.ReceiveMessage(message))
            {
                // If the mssage was received by the selected
                // filter, redraw the messages. If it was
                // received by another filter, update that
                // filter.
                if (filter != _selectedFilter)
                {
                    filter.UpdateFilter();
                }
                else
                {
                    _text.text = filter.DisplayMessages();
                }
            }
        }
    }
}
