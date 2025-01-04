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
        //�����б�������PageView�÷�ͬ��
        //ʹ��ʹ�ü���GameObject ��չʾ���޶�����ݣ����������ʵ������ô��GameObject

        public GameObject prefabTest;

        [ReadOnly]
        public List<string> ListViewDatas = new List<string>();

        private ListView m_sv_list_view_ListView;
        // Start is called before the first frame update
        void Start()
        {
            //��ʼ����������
            for (int i = 0; i < 100; i++)
            {
                ListViewDatas.Add(i.ToString());
            }

            m_sv_list_view_ListView = GetComponent<ListView>();

            ListView.FuncTab funcTab = new ListView.FuncTab();

            //�������������ֻ��һ��prefab ��ôֻ��Ҫ������һ����������
            //����ֻ����������GameObject�ظ�ʹ�ã�����������Ҫ���ϵĸ�prefab����������
            //ÿ����ν�������GameObject��Ӧ��ʾ�Ϳ��������
            funcTab.ItemEnter = ItemEnter;

            //����һ���ֵ��������������ڿ����ж��Prefab�����
            Dictionary<string, GameObject> list = new Dictionary<string, GameObject>
            {
                { "Prefab1", prefabTest }
            };
            //��ʼ��ListView 
            m_sv_list_view_ListView.SetInitData(list, funcTab);
            //����ListView �����ж��ٸ�����
            m_sv_list_view_ListView.FillContent(10);
        }

        private void ItemEnter(ListView.ListItem listItem)
        {
            //listItem.go.GetComponent<Image>().color = UnityEngine.Random.ColorHSV();

            //����ѭ������GameObject ���Ե�һ��unity ������һ����������ݳ��ֵ�ʱ�����
            //��������Ӧ�ñ�֤ �������ݳ���ListViewDatas��FillContent Ӧ��һ��
            //���������ｫListViewDatas������ȡ�����չ������ø�Prefab
            //TODO��ʹ�ö�̬����һ��Item�ű�������Ĳ����ŵ�Item�£�������ListView.ListItem�л���˽ű���ͨ���ӿ��Լ������������Զ�����Щ����
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