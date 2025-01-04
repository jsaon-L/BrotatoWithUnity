
using GameFramework.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UGFExtensions.Await;
using UGFExtensions.Texture;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class TestWebRequest : MonoBehaviour
{
    public RawImage RawImage;
    public Transform child;

    [TextArea]
    public string englishWords;
    public List<string> words;
    // Start is called before the first frame update
    void Start()
    {

        GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, 
            (s, w) => {
                
                var e = (WebRequestSuccessEventArgs)w;
                Debug.Log(e.GetWebResponseText());
        
        });
        GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId,
       (s, w) => {

           var e = (WebRequestFailureEventArgs)w;
           Debug.Log(e.ErrorMessage);

       });


        //var matchs = System.Text.RegularExpressions.Regex.Matches(englishWords, "\\b[A-Za-z]+\\b");

        //foreach (Match item in matchs.Cast<Match>())
        //{
        //    if (!words.Contains(item.Value+",")) 
        //    {
        //        words.Add(item.Value+",");
        //    }
        //}

        ////File.WriteAllLines(@"H:\tmp\625µ•¥ À≥–Ú∞Ê±æ.txt", words.ToArray());

        //RandomList(words);

        //File.WriteAllLines(@"H:\tmp\625µ•¥ ¬“–Ú∞Ê±æ∂∫∫≈.txt", words.ToArray());
    }


    private void RandomList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(0, list.Count);

            (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
        }
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            GameEntry.WebRequest.AddWebRequest("http://192.168.50.145:2526/utils/test");
            RawImage.SetTextureByNetwork("https://ydlunacommon-cdn.nosdn.127.net/41ceb3f3e7710fa53d5257751f822543.png");
        }
    }


}


