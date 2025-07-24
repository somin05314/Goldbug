using UnityEngine;

public class NailManager : MonoBehaviour
{
    public static NailManager Instance;

    public GameObject nailPrefab;
    public int maxNails = 3;
    private int usedNails = 0;

    void Awake()
    {
        Instance = this;
    }

    public bool CanPlaceNail() => usedNails < maxNails;

    public void UseNail()
    {
        usedNails++;
        UIManager.Instance.UpdateNailUI(usedNails, maxNails);
    }

    public void ResetNails()
    {
        usedNails = 0;
        UIManager.Instance.UpdateNailUI(usedNails, maxNails);
    }
}
