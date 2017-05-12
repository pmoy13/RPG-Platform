using UnityEngine;
using UnityEngine.UI;

public class ChatInputField : MonoBehaviour
{

    public Text PlaceholderText;

    [SerializeField] private InputField _inputField;
    [SerializeField] private ChatHandler _chatHandler;

	// Use this for initialization
	private void Start ()
    {

    }
	
	// Update is called once per frame
	private void Update ()
	{

	}

    public void OnValueChanged(string input)
    {
        // Clear the placeholder text so that our new message
        // is the only text the user can see. If we don't clear
        // the placeholder text, then the placeholder text is rendered
        // underneath the text the user is trying to write.
        PlaceholderText.text = "";
    }

    public void OnEndEdit(string input)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // See if there is a message to send.
            if (_inputField.text != "")
            {
                // Send the message.
                // :TODO: Actually send the message to the next box!
                Debug.Log(input);
                _chatHandler.SendChatMessage(input);

                // Delete the message text that we just sent.
                _inputField.text = "";
            }
        }

        // The user ended the focus, either because a message was sent,
        // the user decided not to send anything, or the user is not
        // finished composing the message. Check to see if the input
        // field is empty. If so, return it to the default.
        if (_inputField.text == "")
        {
            // Reset the placeholder text.
            PlaceholderText.text = "Enter message...";
        }
    }
}
