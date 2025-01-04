using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.ProjectWindowCallback;

public class CreateWebSocketEvent : EditorWindow
{
    [MenuItem("Assets/Create/GF/Event", false, 80)]
    public static void CreateEventScript()
    {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
             ScriptableObject.CreateInstance<CreateScriptAssetAction>(),
             GetSelectedPathOrFallback() + "/Event.cs",
             null,
             "Assets/UnityGameFramework/MetaDL/Editor/GFEventTemplate.txt");

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
class CreateScriptAssetAction : EndNameEditAction
{
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        //創建資源
        UnityEngine.Object obj = CreateAssetFromTemplate(pathName, resourceFile);
        //高亮顯示該資源
        ProjectWindowUtil.ShowCreatedAsset(obj);
    }
    internal static UnityEngine.Object CreateAssetFromTemplate(string pahtName, string resourceFile)
    {
        //獲取要創建的資源的絕對路徑
        string fullName = Path.GetFullPath(pahtName);
        //讀取本地模板文件
        StreamReader reader = new StreamReader(resourceFile);
        string content = reader.ReadToEnd();
        reader.Close();

        //獲取資源的文件名
         string fileName = Path.GetFileNameWithoutExtension(pahtName);
        //替換默認的文件名
        content = content.Replace("#ClassName#", fileName);




        //寫入新文件
        StreamWriter writer = new StreamWriter(fullName, false, System.Text.Encoding.UTF8);
        writer.Write(content);
        writer.Close();

        //刷新本地資源
        AssetDatabase.ImportAsset(pahtName);
        AssetDatabase.Refresh();

        return AssetDatabase.LoadAssetAtPath(pahtName, typeof(UnityEngine.Object));
    }
}
