using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.ProjectWindowCallback;

public class CreateGFVar : EditorWindow
{
    [MenuItem("Assets/Create/GF/GFVar", false, 81)]
    public static void CreateEventScript()
    {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
             ScriptableObject.CreateInstance<CreateScriptAsset_GFVar_Action>(),
             GetSelectedPathOrFallback() + "/Var.cs",
             null,
             "Assets/UnityGameFramework/MetaDL/Editor/GFVarTemplate.txt");

    }

    public static string GetSelectedPathOrFallback()
    {
        string path = "Assets";
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }

}
class CreateScriptAsset_GFVar_Action : EndNameEditAction
{
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        //������Դ
        UnityEngine.Object obj = CreateAssetFromTemplate(pathName, resourceFile);
        //������ʾ����Դ
        ProjectWindowUtil.ShowCreatedAsset(obj);
    }
    internal static UnityEngine.Object CreateAssetFromTemplate(string pahtName, string resourceFile)
    {
        //��ȡҪ��������Դ�ľ���·��
        string fullName = Path.GetFullPath(pahtName);
        //��ȡ����ģ���ļ�
        StreamReader reader = new StreamReader(resourceFile);
        string content = reader.ReadToEnd();
        reader.Close();

        //��ȡ��Դ���ļ���
        string fileName = Path.GetFileNameWithoutExtension(pahtName);
        //�滻Ĭ�ϵ��ļ���
        content = content.Replace("#ClassName#", fileName);




        //д�����ļ�
        StreamWriter writer = new StreamWriter(fullName, false, System.Text.Encoding.UTF8);
        writer.Write(content);
        writer.Close();

        //ˢ�±�����Դ
        AssetDatabase.ImportAsset(pahtName);
        AssetDatabase.Refresh();

        return AssetDatabase.LoadAssetAtPath(pahtName, typeof(UnityEngine.Object));
    }
}