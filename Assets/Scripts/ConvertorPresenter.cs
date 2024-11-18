using Modules.Converter;
using UnityEngine;

public class ConvertorPresenter : MonoBehaviour
{
    private void Awake()
    {
        var recept = new ConvertReceipt(ItemType.Wood, ItemType.Plank, 5, 1, 2);
        var convertor = new Converter(5, recept);
        var view = GetComponent<ConvertorView>();
    }
}