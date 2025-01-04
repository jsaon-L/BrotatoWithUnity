using System;
using GameFramework;
using UnityEngine.UI;
using System.Collections.Generic;
using Skyunion;
using System.Text;
using System.Text.RegularExpressions;
using ArabicSupport;

namespace UnityEngine.UI
{
    public class LanguageText : Text
    {
//        protected char LineEnding = '\n';
//        [SerializeField] private int m_languageId = -1;
//        public int languageId
//        {
//            get { return m_languageId; }
//            set { m_languageId = value; }
//        }
//        protected override void Start()
//        {
//            UpdateLanguage();
//        }
//        public void UpdateLanguage()
//        {
//            if (m_languageId != -1)
//            {
//                text = LanguageUtils.getText(m_languageId);
//            }
//            AutoArabicByText = AutoArabicByText;
//        }

//        [SerializeField]
//        private string m_editorText = string.Empty;
//        public bool isArabicText = false;
//        [SerializeField] private bool m_bAutoArabicByText = true;
//        public bool AutoArabicByText
//        {
//            get
//            {
//                return m_bAutoArabicByText;
//            }
//            set
//            {
//                m_bAutoArabicByText = value;
//                if (m_bAutoArabicByText == false)
//                {
//                    isArabicText = LanguageUtils.IsArabic();
//                }
//                else
//                {
//                    isArabicText = ArabicFixerTool.IsRtl(BaseText);
//                }
//                SetAllDirty();
//            }
//        }
//        protected override void OnEnable()
//        {
//            base.OnEnable();
//            UpdateLanguage();
//            isArabicText = LanguageUtils.IsArabic();
//            AutoArabicByText = m_bAutoArabicByText;
//            if (transform.parent != null && transform.parent.GetComponent<InputField>())
//            {
//                supportRichText = false;
//            }
//        }

//        public string BaseText
//        {
//            get
//            {
//#if UNITY_EDITOR
//                if(Application.isPlaying == false && !string.IsNullOrEmpty(m_editorText))
//                {
//                    return m_editorText;
//                }
//#endif
//                return base.text;
//            }
//        }

//        private StringBuilder stringBuilder = new StringBuilder();
//        public override string text
//        {
//            get
//            {
//                if (isArabicText)
//                {
//                    string baseText = ArabicFixer.FixWithoutRtl(base.text, false, false, false);

//                    // 取出所有的标签
//                    stringBuilder.Length = 0;
//                    int nPos = 0;
//                    MatchCollection matches = Regex.Matches(baseText, @"<[^>]+>");
//                    for (int j = 0; j < matches.Count; j++)
//                    {
//                        var tag = matches[j].Value;
//                        if (tag[1] == 'h')
//                        {
//                            if (matches[j].Index > nPos)
//                            {
//                                stringBuilder.Append(baseText.Substring(nPos, matches[j].Index - nPos));
//                            }
//                            stringBuilder.Append("<color=#00ff00>");
//                            nPos = matches[j].Index + tag.Length;
//                        }
//                        else if(tag[1] == '/' && tag[2] == 'h')
//                        {
//                            if (matches[j].Index > nPos)
//                            {
//                                stringBuilder.Append(baseText.Substring(nPos, matches[j].Index - nPos));
//                            }
//                            stringBuilder.Append("</color>");
//                            nPos = matches[j].Index + tag.Length;
//                        }
//                    }
//                    stringBuilder.Append(baseText.Substring(nPos));

//                    var newText = stringBuilder.ToString();
//                    cachedTextGenerator.Populate(newText, GetGenerationSettings(rectTransform.rect.size));
//                    List<UILineInfo> lines = cachedTextGenerator.lines as List<UILineInfo>;
//                    if (lines == null) return null;

//                    stringBuilder.Length = 0;
//                    for (int i = 0; i < lines.Count; i++)
//                    {
//                        if (i < lines.Count - 1)
//                        {
//                            int startIndex = lines[i].startCharIdx;
//                            int length = lines[i + 1].startCharIdx - lines[i].startCharIdx;
//                            stringBuilder.Append(newText.Substring(startIndex, length));
//                            if (stringBuilder.Length > 0 &&
//                                stringBuilder[stringBuilder.Length - 1] != '\n' &&
//                                stringBuilder[stringBuilder.Length - 1] != '\r')
//                            {
//                                if (stringBuilder[stringBuilder.Length - 1] == ' ')
//                                {
//                                    stringBuilder.Remove(stringBuilder.Length - 1, 1);// 防止阿语因为 空格跑到前面去，后续会变成换行，导致错误。
//                                }
//                                stringBuilder.Append(LineEnding);
//                            }
//                        }
//                        else
//                        {
//                            stringBuilder.Append(newText.Substring(lines[i].startCharIdx));
//                            if(stringBuilder.Length>0 && stringBuilder[stringBuilder.Length-1] == ' ')
//                            {
//                                stringBuilder.Remove(stringBuilder.Length - 1, 1);// 防止阿语因为 空格跑到前面去，后续会变成换行，导致错误。
//                            }
//                        }
//                    }
//                    string linedText = stringBuilder.ToString();

//                    // 还原超链接标签
//                    stringBuilder.Length = 0;
//                    nPos = 0;
//                    MatchCollection matches2 = Regex.Matches(linedText, @"<[^>]+>");
//                    for (int j = 0; j < matches.Count; j++)
//                    {
//                        var tag = matches[j].Value;
//                        if (tag[1] == 'h')
//                        {
//                            if (matches2[j].Index > nPos)
//                            {
//                                stringBuilder.Append(linedText.Substring(nPos, matches2[j].Index - nPos));
//                            }
//                            stringBuilder.Append(tag);
//                            nPos = matches2[j].Index + matches2[j].Value.Length;
//                        }
//                        else if (tag[1] == '/' && tag[2] == 'h')
//                        {
//                            if (matches2[j].Index > nPos)
//                            {
//                                stringBuilder.Append(linedText.Substring(nPos, matches2[j].Index - nPos));
//                            }
//                            stringBuilder.Append(tag);
//                            nPos = matches2[j].Index + matches2[j].Value.Length;
//                        }
//                    }
//                    stringBuilder.Append(linedText.Substring(nPos));
//                    linedText = stringBuilder.ToString();

//                    var fixText = ArabicFixer.FixRtl(linedText, false, false, false);
//                    // 需要把标签提取出来。
//                    if (supportRichText)
//                    {
//                        fixText = FixRichTextTag(fixText);
//                    }
//                    return fixText;
//                }
//                else
//                {
//                    return BaseText;
//                }
//            }
//            set
//            {
//                if(Application.isPlaying)
//                {
//                    m_languageId = -1;
//                }
//                if (value == null)
//                {
//                    value = String.Empty;
//                }
//                base.text = value;
//                // 如果不是阿语但是内容有阿语需要显示阿语
//                if (m_bAutoArabicByText)
//                {
//                    isArabicText = ArabicFixerTool.IsRtl(value);
//                }
//                else
//                {
//                    isArabicText = LanguageUtils.IsArabic();
//                }
//            }
//        }

//        struct TextBlock
//        {
//            public TextBlock(string text, string[] tags = null)
//            {
//                this.tags = tags;
//                this.text = text;
//            }
//            public string[] tags;
//            public string text;
//        }

//        private string FixRichTextTag(string str)
//        {
//            string[] stringSeparators = new string[] { Environment.NewLine };
//            string[] strSplit = str.Split(stringSeparators, StringSplitOptions.None);
//            Stack<string> tagStack = new Stack<string>();
//            List<List<TextBlock>> lineBlocks = new List<List<TextBlock>>();
//            for (int i = 0; i < strSplit.Length; i++)
//            {
//                List<TextBlock> lineBlock = new List<TextBlock>();
//                if (ArabicFixerTool.IsRtl(strSplit[i]))
//                {
//                    MatchCollection matches = Regex.Matches(strSplit[i], @"<[^>]+>");
//                    int cPos = strSplit[i].Length;
//                    for (int j = matches.Count - 1; j >= 0; j--)
//                    {
//                        var tag = matches[j].Value;
//                        int idx = strSplit[i].LastIndexOf(matches[j].Value, cPos);
//                        int idxStart = idx + matches[j].Value.Length;
//                        var text = strSplit[i].Substring(idxStart, cPos - idxStart);
//                        cPos = idx;
//                        TextBlock block;
//                        if (tag[tag.Length - 2] == '/')
//                        {
//                            block = new TextBlock(text, tagStack.ToArray());
//                            if (tagStack.Count > 0)
//                            {
//                                tag = tagStack.Pop();
//                            }
//                            else
//                            {
//                                Debug.LogError($"Tag not pair: {str}");
//                            }
//                        }
//                        else
//                        {
//                            char chTag = tag[tag.Length - 2];
//                            switch (chTag)
//                            {
//                                case 'r':
//                                    {
//                                        int n = tag.IndexOf("=color");
//                                        var color = tag.Substring(1, n - 1);
//                                        tag = $"<color={color}>";
//                                    }
//                                    break;
//                                case 'e':
//                                    {
//                                        int n = tag.IndexOf("=size");
//                                        var size = tag.Substring(1, n - 1);
//                                        tag = $"<size={size}>";
//                                    }
//                                    break;
//                                case 'f':
//                                    {
//                                        int n = tag.IndexOf("=href");
//                                        var href = tag.Substring(1, n - 1);
//                                        tag = $"<href={href}>";
//                                    }
//                                    break;
//                                case 'i':
//                                    tag = "<i>";
//                                    break;
//                                case 'b':
//                                    tag = "<b>";
//                                    break;
//                            }

//                            if (tagStack.Count == 0)
//                            {
//                                block = new TextBlock(text);
//                            }
//                            else
//                            {
//                                block = new TextBlock(text, tagStack.ToArray());
//                            }
//                            tagStack.Push(tag);
//                        }
//                        if (text.Length > 0)
//                        {
//                            lineBlock.Add(block);
//                        }
//                    }

//                    if (cPos != 0)
//                    {
//                        var text = strSplit[i].Substring(0, cPos);

//                        TextBlock block;
//                        if (tagStack.Count == 0)
//                        {
//                            block = new TextBlock(text);
//                        }
//                        else
//                        {
//                            block = new TextBlock(text, tagStack.ToArray());
//                        }
//                        lineBlock.Add(block);
//                    }
//                }
//                else
//                {
//                    MatchCollection matches = Regex.Matches(strSplit[i], @"<[^>]+>");
//                    int cPos = 0;
//                    for (int j = 0; j < matches.Count; j++)
//                    {
//                        var tag = matches[j].Value;
//                        int idx = strSplit[i].IndexOf(matches[j].Value, cPos);
//                        var text = strSplit[i].Substring(cPos, idx - cPos);
//                        cPos = idx + matches[j].Value.Length;
//                        TextBlock block;
//                        if (tag[1] == '/')
//                        {
//                            block = new TextBlock(text, tagStack.ToArray());
//                            tag = tagStack.Pop();
//                        }
//                        else
//                        {
//                            if (tagStack.Count == 0)
//                            {
//                                block = new TextBlock(text);
//                            }
//                            else
//                            {
//                                block = new TextBlock(text, tagStack.ToArray());
//                            }
//                            tagStack.Push(tag);
//                        }
//                        //if (text.Length > 0)
//                        {
//                            lineBlock.Insert(0, block);
//                        }
//                    }

//                    if (cPos != strSplit[i].Length)
//                    {
//                        var text = strSplit[i].Substring(cPos);

//                        TextBlock block;
//                        if (tagStack.Count == 0)
//                        {
//                            block = new TextBlock(text);
//                        }
//                        else
//                        {
//                            block = new TextBlock(text, tagStack.ToArray());
//                        }
//                        lineBlock.Insert(0, block);
//                    }
//                }

//                lineBlocks.Add(lineBlock);
//            }


//            StringBuilder fixTest = new StringBuilder();
//            for (int l = 0; l < lineBlocks.Count; l++)
//            {
//                var lines = lineBlocks[l];
//                for (int i = lines.Count - 1; i >= 0; i--)
//                {
//                    var bolck = lines[i];
//                    if (bolck.tags != null)
//                    {
//                        for (int j = bolck.tags.Length - 1; j >= 0; j--)
//                        {
//                            fixTest.Append(bolck.tags[j]);
//                        }
//                    }
//                    fixTest.Append(bolck.text);
//                    if (bolck.tags != null)
//                    {
//                        for (int j = 0; j < bolck.tags.Length; j++)
//                        {
//                            char cTag = bolck.tags[j][1];
//                            switch (cTag)
//                            {
//                                case 'c':
//                                    fixTest.Append("</color>");
//                                    break;
//                                case 's':
//                                    fixTest.Append("</size>");
//                                    break;
//                                case 'h':
//                                    fixTest.Append("</href>");
//                                    break;
//                                case 'b':
//                                    fixTest.Append("</b>");
//                                    break;
//                                case 'i':
//                                    fixTest.Append("</i>");
//                                    break;
//                            }
//                        }
//                    }
//                }
//                if(l != lineBlocks.Count-1)
//                    fixTest.Append(Environment.NewLine);
//            }

//            return fixTest.ToString();
//        }

//        public override float preferredWidth
//        {
//            get
//            {
//                if (isArabicText)
//                {
//                    var settings = GetGenerationSettings(Vector2.zero);
//                    return cachedTextGeneratorForLayout.GetPreferredWidth(ArabicFixer.FixWithoutRtl(BaseText, false, false, false), settings) / pixelsPerUnit;
//                }
//                else
//                {
//                    return base.preferredWidth;
//                }
//            }
//        }

//        public override float preferredHeight
//        {
//            get
//            {
//                if (isArabicText)
//                {
//                    var settings = GetGenerationSettings(new Vector2(GetPixelAdjustedRect().size.x, 0.0f));
//                    //zj  tip的高度获取错误  BaseText 改为text
//                    return cachedTextGeneratorForLayout.GetPreferredHeight(ArabicFixer.FixWithoutRtl(BaseText, false, false, false), settings) / pixelsPerUnit;
//                }
//                else
//                {
//                    return base.preferredHeight;
//                }
//            }
//        }
    }
}
