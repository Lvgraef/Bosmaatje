using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PasswordVisibilityHandler : MonoBehaviour
{
    public TMP_InputField TMPro_inputField;
    public Button button;
    public Sprite eyeOpen;
    public Sprite eyeClose;

    public void OnButtonClick()
    {
        if (TMPro_inputField.inputType == TMP_InputField.InputType.Password)
        {
            TMPro_inputField.inputType = TMP_InputField.InputType.Standard;
            TMPro_inputField.ForceLabelUpdate();
            button.image.sprite = eyeClose;
        }
        else
        {
            TMPro_inputField.inputType = TMP_InputField.InputType.Password;
            TMPro_inputField.ForceLabelUpdate();
            button.image.sprite = eyeOpen;
        }
    }
}
