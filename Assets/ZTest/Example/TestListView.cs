using Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestListView : MonoBehaviour
{
    public GameObject prefabTest;

    private ListView m_sv_list_view_ListView;
    // Start is called before the first frame update
    void Start()
    {
        m_sv_list_view_ListView = GetComponent<ListView>();

        ListView.FuncTab funcTab = new ListView.FuncTab();

        funcTab.ItemEnter = ListViewItemByIndex;

        Dictionary<string, GameObject> list = new Dictionary<string, GameObject>
        {
            { prefabTest.name, prefabTest }
        };
        m_sv_list_view_ListView.SetInitData(list, funcTab);

        m_sv_list_view_ListView.FillContent(10);
    }

    private void ListViewItemByIndex(ListView.ListItem listItem)
    {
        listItem.go.GetComponent<Image>().color = UnityEngine.Random.ColorHSV();
        listItem.go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = listItem.index.ToString();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            var item = m_sv_list_view_ListView.GetItemByIndex(5);
            m_sv_list_view_ListView.ScrollToPos(1);
            
        }

    }
}
