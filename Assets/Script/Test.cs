
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(onClick);
    }

    private void onClick()
    {
        this.gameObject.SetActive(false);
    }
}
