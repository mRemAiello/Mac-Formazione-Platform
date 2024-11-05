using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleItemUI : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemNumber;

    public void SetSprite(Sprite sprite)
    {
        _itemImage.gameObject.SetActive(true);
        _itemImage.sprite = sprite;
    }

    public void SetItemNumber(int number)
    {
        SetItemNumber(number.ToString());
    }

    public void SetItemNumber(string number)
    {
        _itemNumber.gameObject.SetActive(true);
        _itemNumber.text = number;
    }
}