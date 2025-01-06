using Client;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GFExample
{
    public class ListViewExample : MonoBehaviour
    {
        //无限列表容器，PageView用法同理
        //使用使用几个GameObject 来展示无限多的数据，而不用真的实例化那么多GameObject

        public GameObject prefabTest;

        [ReadOnly]
        public List<string> ListViewDatas = new List<string>();

        private ListView m_sv_list_view_ListView;
        // Start is called before the first frame update
        void Start()
        {
            //初始化测试数据
            for (int i = 0; i < 100; i++)
            {
                ListViewDatas.Add(i.ToString());
            }

            m_sv_list_view_ListView = GetComponent<ListView>();

            ListView.FuncTab funcTab = new ListView.FuncTab();

            //假设你的容器内只有一种prefab 那么只需要关心这一个函数就行
            //由于只有少数几个GameObject重复使用，所以我们需要不断的给prefab设置新数据
            //每次如何将数据与GameObject对应显示就靠这个函数
            funcTab.ItemEnter = ItemEnter;

            //定义一个字典用来处理容器内可能有多个Prefab的情况
            Dictionary<string, GameObject> list = new Dictionary<string, GameObject>
            {
                { "Prefab1", prefabTest }
            };
            //初始化ListView 
            m_sv_list_view_ListView.SetInitData(list, funcTab);
            //告诉ListView 我们有多少个数据
            m_sv_list_view_ListView.FillContent(10);
        }

        private void ItemEnter(ListView.ListItem listItem)
        {
            //listItem.go.GetComponent<Image>().color = UnityEngine.Random.ColorHSV();

            //由于循环复用GameObject 所以当一个unity 物体以一个新数据身份出现的时候调用
            //这里我们应该保证 我们数据长度ListViewDatas与FillContent 应该一致
            //我们在这里将ListViewDatas的数据取出按照规则设置给Prefab
            //TODO:使用多态构造一个Item脚本，下面的操作放到Item下，并且在ListView.ListItem中缓存此脚本，通过接口以及创建子类来自定义这些操作
            listItem.go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = ListViewDatas[listItem.index];
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.A))
            {
                var item = m_sv_list_view_ListView.GetItemByIndex(5);
                m_sv_list_view_ListView.ScrollToPos(1);
                m_sv_list_view_ListView.ScrollPanelToItemIndex(1);

            }

        }
    }
}