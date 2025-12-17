using UnityEngine;

public class GloveEquip : MonoBehaviour
{
    public ArmorEquipper equipper;
    public GameObject glove;

    private void Start()
    {
        equipper.Equip(glove);
    }
}
