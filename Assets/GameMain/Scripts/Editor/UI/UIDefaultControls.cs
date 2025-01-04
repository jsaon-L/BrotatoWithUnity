using UnityEngine;
using System.Collections.Generic;
using System;
using Client;
using UnityEditor;

namespace UnityEngine.UI
{
    public static class UIDefaultControls
    {
        #region code from DefaultControls.cs
        public struct Resources
        {
            public Sprite standard;
            public Sprite background;
            public Sprite inputField;
            public Sprite knob;
            public Sprite checkmark;
            public Sprite dropdown;
            public Sprite mask;
        }

        private const float kWidth = 160f;
        private const float kThickHeight = 30f;
        private const float kThinHeight = 20f;
        private static Vector2 s_ThickElementSize = new Vector2(kWidth, kThickHeight);
        private static Vector2 s_ThinElementSize = new Vector2(kWidth, kThinHeight);
        private static Vector2 s_ImageElementSize = new Vector2(100f, 100f);
        private static Color s_DefaultSelectableColor = new Color(1f, 1f, 1f, 1f);
        //private static Color s_PanelColor = new Color(1f, 1f, 1f, 0.392f);
        private static Color s_TextColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);

        private static Vector2 s_ThinGUIElementSize = new Vector2(kWidth, kThinHeight);

        // Helper methods at top

        private static GameObject CreateUIElementRoot(string name, Vector2 size)
        {
            GameObject child = new GameObject(name);
            RectTransform rectTransform = child.AddComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            return child;
        }

        static GameObject CreateUIObject(string name, GameObject parent)
        {
            GameObject go = new GameObject(name);
            go.AddComponent<RectTransform>();
            SetParentAndAlign(go, parent);
            return go;
        }

        private static void SetDefaultTextValues(Text lbl)
        {
            // Set text values we want across UI elements in default controls.
            // Don't set values which are the same as the default values for the Text component,
            // since there's no point in that, and it's good to keep them as consistent as possible.
            lbl.color = s_TextColor;
        }

        private static void SetDefaultColorTransitionValues(Selectable slider)
        {
            ColorBlock colors = slider.colors;
            colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
            colors.pressedColor = new Color(0.698f, 0.698f, 0.698f);
            colors.disabledColor = new Color(0.521f, 0.521f, 0.521f);
        }

        private static void SetParentAndAlign(GameObject child, GameObject parent)
        {
            if (parent == null)
                return;

            child.transform.SetParent(parent.transform, false);
            SetLayerRecursively(child, parent.layer);
        }

        private static void SetLayerRecursively(GameObject go, int layer)
        {
            go.layer = layer;
            Transform t = go.transform;
            for (int i = 0; i < t.childCount; i++)
                SetLayerRecursively(t.GetChild(i).gameObject, layer);
        }
        #endregion

        public static GameObject CreateUIPolygonImage(DefaultControls.Resources resources)
        {
            GameObject uiElementRoot = CreateUIElementRoot("img_polygonImage", s_ImageElementSize);
            PolygonImage image = uiElementRoot.AddComponent<PolygonImage>();
            image.sprite = resources.standard;
            image.type = Image.Type.Sliced;
            image.color = s_DefaultSelectableColor;
            image.raycastTarget = false;
            return uiElementRoot;
        }


        public static GameObject CreateGameView(DefaultControls.Resources resources)
        {
            GameObject uiElementRoot = CreateUIElementRoot("pl_view", s_ThickElementSize);


            // Set RectTransform to stretch
            //RectTransform rectTransform = uiElementRoot.GetComponent<RectTransform>();
            //rectTransform.anchorMin = Vector2.zero;
            //rectTransform.anchorMax = Vector2.one;
            //rectTransform.anchoredPosition = Vector2.zero;
            //rectTransform.sizeDelta = Vector2.zero;

            return uiElementRoot;
        }

        public static GameObject CreateLanguageText(DefaultControls.Resources resources)
        {
            GameObject uiElementRoot = CreateUIElementRoot("lbl_languageText", s_ThickElementSize);
            LanguageText lbl = uiElementRoot.AddComponent<LanguageText>();
            lbl.text = "Language Text";
            lbl.alignment = TextAnchor.MiddleLeft;
            lbl.raycastTarget = false;
            SetDefaultTextValues(lbl);
            return uiElementRoot;
        }

        public static GameObject CreateLinkImageText(DefaultControls.Resources resources)
        {
            GameObject uiElementRoot = CreateUIElementRoot("lbl_linkImageText", s_ThickElementSize);
            HrefText lbl = uiElementRoot.AddComponent<HrefText>();
            lbl.text = "Language Text";
            //lbl.alignment = TextAnchor.MiddleLeft;
            SetDefaultTextValues(lbl);
            return uiElementRoot;
        }

        public static GameObject CreateLanguageButton(DefaultControls.Resources resources)
        {
            GameObject buttonRoot = CreateUIElementRoot("btn_languageButton", s_ThickElementSize);

            GameObject childText = new GameObject("Text");
            SetParentAndAlign(childText, buttonRoot);

            PolygonImage image = buttonRoot.AddComponent<PolygonImage>();
            image.sprite = resources.standard;
            image.type = Image.Type.Sliced;
            image.color = s_DefaultSelectableColor;

            GameButton bt = buttonRoot.AddComponent<GameButton>();
            SetDefaultColorTransitionValues(bt);

            LanguageText text = childText.AddComponent<LanguageText>();
            text.alignment = TextAnchor.MiddleCenter;
            SetDefaultTextValues(text);

            RectTransform textRectTransform = childText.GetComponent<RectTransform>();
            textRectTransform.anchorMin = Vector2.zero;
            textRectTransform.anchorMax = Vector2.one;
            textRectTransform.sizeDelta = Vector2.zero;

            return buttonRoot;
        }

        public static GameObject CreateAnimationButton(DefaultControls.Resources resources)
        {
            GameObject buttonRoot = CreateUIElementRoot("btn_animButton", s_ThickElementSize);

            GameObject childText = new GameObject("Text");
            SetParentAndAlign(childText, buttonRoot);

            PolygonImage image = buttonRoot.AddComponent<PolygonImage>();
            image.sprite = resources.standard;
            image.type = Image.Type.Sliced;
            image.color = s_DefaultSelectableColor;

            GameButton bt = buttonRoot.AddComponent<GameButton>();
            SetDefaultColorTransitionValues(bt);

            LanguageText text = childText.AddComponent<LanguageText>();
            text.alignment = TextAnchor.MiddleCenter;
            SetDefaultTextValues(text);

            RectTransform textRectTransform = childText.GetComponent<RectTransform>();
            textRectTransform.anchorMin = Vector2.zero;
            textRectTransform.anchorMax = Vector2.one;
            textRectTransform.sizeDelta = Vector2.zero;

            BtnAnimation anim = buttonRoot.AddComponent<BtnAnimation>();
            AnimationClip downAnim = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/BundleAssets/UI/UIAnimation/CommonBtnDownAni.anim");
            anim.DownAnimation = downAnim;

            AnimationClip upAnim = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/BundleAssets/UI/UIAnimation/CommonBtnUpAni.anim");
            anim.UpAnimation = upAnim;

            return buttonRoot;
        }

        public static GameObject CreateNOTextButton(DefaultControls.Resources resources)
        {
            GameObject buttonRoot = CreateUIElementRoot("btn_noTextButton", s_ThickElementSize);

            PolygonImage image = buttonRoot.AddComponent<PolygonImage>();
            image.sprite = resources.standard;
            image.type = Image.Type.Sliced;
            image.color = s_DefaultSelectableColor;

            GameButton bt = buttonRoot.AddComponent<GameButton>();
            SetDefaultColorTransitionValues(bt);

            return buttonRoot;
        }

        public static GameObject CreateLanguageToggle(DefaultControls.Resources resources)
        {
            // Set up hierarchy
            GameObject toggleRoot = CreateUIElementRoot("ck_languageCheckBox", s_ThinElementSize);

            GameObject background = CreateUIObject("Background", toggleRoot);
            GameObject checkmark = CreateUIObject("Checkmark", background);
            GameObject childLabel = CreateUIObject("Label", toggleRoot);

            // Set up components
            GameToggle toggle = toggleRoot.AddComponent<GameToggle>();
            toggle.isOn = true;

            PolygonImage bgImage = background.AddComponent<PolygonImage>();
            bgImage.sprite = resources.standard;
            bgImage.type = Image.Type.Sliced;
            bgImage.color = s_DefaultSelectableColor;

            PolygonImage checkmarkImage = checkmark.AddComponent<PolygonImage>();
            checkmarkImage.sprite = resources.checkmark;

            LanguageText label = childLabel.AddComponent<LanguageText>();
            label.text = "Toggle";
            SetDefaultTextValues(label);

            toggle.graphic = checkmarkImage;
            toggle.targetGraphic = bgImage;
            SetDefaultColorTransitionValues(toggle);

            RectTransform bgRect = background.GetComponent<RectTransform>();
            bgRect.anchorMin = new Vector2(0f, 1f);
            bgRect.anchorMax = new Vector2(0f, 1f);
            bgRect.anchoredPosition = new Vector2(10f, -10f);
            bgRect.sizeDelta = new Vector2(kThinHeight, kThinHeight);

            RectTransform checkmarkRect = checkmark.GetComponent<RectTransform>();
            checkmarkRect.anchorMin = new Vector2(0.5f, 0.5f);
            checkmarkRect.anchorMax = new Vector2(0.5f, 0.5f);
            checkmarkRect.anchoredPosition = Vector2.zero;
            checkmarkRect.sizeDelta = new Vector2(20f, 20f);

            RectTransform labelRect = childLabel.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0f, 0f);
            labelRect.anchorMax = new Vector2(1f, 1f);
            labelRect.offsetMin = new Vector2(23f, 1f);
            labelRect.offsetMax = new Vector2(-5f, -2f);

            return toggleRoot;
        }

        public static GameObject CreateGameSlider(DefaultControls.Resources resources)
        {
            // Create GOs Hierarchy
            GameObject root = CreateUIElementRoot("sd_GameSlider", s_ThinElementSize);

            GameObject background = CreateUIObject("Background", root);
            GameObject fillArea = CreateUIObject("Fill Area", root);
            GameObject fill = CreateUIObject("Fill", fillArea);
            GameObject handleArea = CreateUIObject("Handle Slide Area", root);
            GameObject handle = CreateUIObject("Handle", handleArea);

            // Background
            PolygonImage backgroundImage = background.AddComponent<PolygonImage>();
            backgroundImage.sprite = resources.background;
            backgroundImage.type = Image.Type.Sliced;
            backgroundImage.color = s_DefaultSelectableColor;
            RectTransform backgroundRect = background.GetComponent<RectTransform>();
            backgroundRect.anchorMin = new Vector2(0, 0.25f);
            backgroundRect.anchorMax = new Vector2(1, 0.75f);
            backgroundRect.sizeDelta = new Vector2(0, 0);

            // Fill Area
            RectTransform fillAreaRect = fillArea.GetComponent<RectTransform>();
            fillAreaRect.anchorMin = new Vector2(0, 0.25f);
            fillAreaRect.anchorMax = new Vector2(1, 0.75f);
            fillAreaRect.anchoredPosition = new Vector2(-5, 0);
            fillAreaRect.sizeDelta = new Vector2(-20, 0);

            // Fill
            PolygonImage fillImage = fill.AddComponent<PolygonImage>();
            fillImage.sprite = resources.standard;
            fillImage.type = Image.Type.Sliced;
            fillImage.color = s_DefaultSelectableColor;

            RectTransform fillRect = fill.GetComponent<RectTransform>();
            fillRect.sizeDelta = new Vector2(10, 0);

            // Handle Area
            RectTransform handleAreaRect = handleArea.GetComponent<RectTransform>();
            handleAreaRect.sizeDelta = new Vector2(-20, 0);
            handleAreaRect.anchorMin = new Vector2(0, 0);
            handleAreaRect.anchorMax = new Vector2(1, 1);

            // Handle
            PolygonImage handleImage = handle.AddComponent<PolygonImage>();
            handleImage.sprite = resources.knob;
            handleImage.color = s_DefaultSelectableColor;

            RectTransform handleRect = handle.GetComponent<RectTransform>();
            handleRect.sizeDelta = new Vector2(20, 0);

            // Setup slider component
            GameSlider slider = root.AddComponent<GameSlider>();
            slider.fillRect = fill.GetComponent<RectTransform>();
            slider.handleRect = handle.GetComponent<RectTransform>();
            slider.targetGraphic = handleImage;
            slider.direction = Slider.Direction.LeftToRight;
            SetDefaultColorTransitionValues(slider);

            return root;
        }

        static public GameObject CreateProgressBar(DefaultControls.Resources resources)
        {
            // Create GOs Hierarchy
            GameObject root = CreateUIElementRoot("pb_rogressBar", s_ThinGUIElementSize);

            GameObject background = CreateUIObject("Background", root);
            GameObject fillArea = CreateUIObject("Fill Area", root);
            GameObject fill = CreateUIObject("Fill", fillArea);

            // Background
            PolygonImage backgroundImage = background.AddComponent<PolygonImage>();
            backgroundImage.sprite = resources.background;
            backgroundImage.type = Image.Type.Sliced;
            backgroundImage.color = s_DefaultSelectableColor;
            backgroundImage.raycastTarget = false;
            RectTransform backgroundRect = background.GetComponent<RectTransform>();
            backgroundRect.anchorMin = new Vector2(0, 0.25f);
            backgroundRect.anchorMax = new Vector2(1, 0.75f);
            backgroundRect.sizeDelta = new Vector2(0, 0);

            // Fill Area
            RectTransform fillAreaRect = fillArea.GetComponent<RectTransform>();
            fillAreaRect.anchorMin = new Vector2(0, 0.25f);
            fillAreaRect.anchorMax = new Vector2(1, 0.75f);
            fillAreaRect.anchoredPosition = Vector2.zero;
            fillAreaRect.sizeDelta = Vector2.zero;

            // Fill
            PolygonImage fillImage = fill.AddComponent<PolygonImage>();
            fillImage.sprite = resources.standard;
            fillImage.type = Image.Type.Sliced;
            fillImage.color = s_DefaultSelectableColor;
            fillImage.raycastTarget = false;

            RectTransform fillRect = fill.GetComponent<RectTransform>();
            fillRect.sizeDelta = Vector2.zero;

            // Setup slider component
            GameSlider slider = root.AddComponent<GameSlider>();
            slider.value = 0;
            slider.fillRect = fill.GetComponent<RectTransform>();
            slider.targetGraphic = fillImage;
            slider.direction = Slider.Direction.LeftToRight;
            SetDefaultColorTransitionValues(slider);
            return root;
        }

        public static GameObject CreateLanguageInputField(DefaultControls.Resources resources)
        {
            GameObject root = CreateUIElementRoot("ipt_languageInputField", s_ThickElementSize);

            GameObject childPlaceholder = CreateUIObject("Placeholder", root);
            GameObject childText = CreateUIObject("Text", root);

            PolygonImage image = root.AddComponent<PolygonImage>();
            image.sprite = resources.inputField;
            image.type = Image.Type.Sliced;
            image.color = s_DefaultSelectableColor;

            GameInput inputField = root.AddComponent<GameInput>();
            SetDefaultColorTransitionValues(inputField);

            Text text = childText.AddComponent<LanguageText>();
            text.text = "";
            text.supportRichText = false;
            SetDefaultTextValues(text);

            LanguageText placeholder = childPlaceholder.AddComponent<LanguageText>();
            placeholder.text = "Enter text...";
            placeholder.fontStyle = FontStyle.Italic;
            // Make placeholder color half as opaque as normal text color.
            Color placeholderColor = text.color;
            placeholderColor.a *= 0.5f;
            placeholder.color = placeholderColor;

            RectTransform textRectTransform = childText.GetComponent<RectTransform>();
            textRectTransform.anchorMin = Vector2.zero;
            textRectTransform.anchorMax = Vector2.one;
            textRectTransform.sizeDelta = Vector2.zero;
            textRectTransform.offsetMin = new Vector2(10, 6);
            textRectTransform.offsetMax = new Vector2(-10, -7);

            RectTransform placeholderRectTransform = childPlaceholder.GetComponent<RectTransform>();
            placeholderRectTransform.anchorMin = Vector2.zero;
            placeholderRectTransform.anchorMax = Vector2.one;
            placeholderRectTransform.sizeDelta = Vector2.zero;
            placeholderRectTransform.offsetMin = new Vector2(10, 6);
            placeholderRectTransform.offsetMax = new Vector2(-10, -7);

            inputField.textComponent = text;
            inputField.placeholder = placeholder;

            return root;
        }

        internal static GameObject CreateScrollView(DefaultControls.Resources resources)
        {
            GameObject go = DefaultControls.CreateScrollView(resources);
            Transform viewport = go.transform.Find("Viewport");

            Image goImage = go.GetComponent<Image>();
            GameObject.DestroyImmediate(go.GetComponent<Image>());
            PolygonImage bgImage = go.AddComponent<PolygonImage>();
            bgImage.sprite = goImage.sprite;
            bgImage.type = Image.Type.Sliced;
            bgImage.color = goImage.color;

            Image viewportImage = viewport.GetComponent<Image>();
            GameObject.DestroyImmediate(viewport.GetComponent<Image>());
            GameObject.DestroyImmediate(viewport.GetComponent<Mask>());
            PolygonImage viewportImage2 = viewport.gameObject.AddComponent<PolygonImage>();
            viewportImage2.sprite = null;
            viewportImage2.type = Image.Type.Sliced;
            viewportImage2.color = viewportImage.color;
            viewport.gameObject.AddComponent<Mask>();

            Transform content = viewport.transform.Find("Content");
            go.name = "sv_scroll_view";
            viewport.name = "v_scroll_view";
            content.name = "c_scroll_view";
            ScrollRect sr = go.GetComponent<ScrollRect>();
            sr.horizontal = false;
            sr.vertical = true;

            ScrollView scrollView = go.AddComponent<ScrollView>();
            scrollView.listContainer = content.GetComponent<RectTransform>();

            return go;
        }

        internal static GameObject CreateListView(DefaultControls.Resources resources)
        {
            GameObject go = DefaultControls.CreateScrollView(resources);
            Transform viewport = go.transform.Find("Viewport");

            Image goImage = go.GetComponent<Image>();
            GameObject.DestroyImmediate(go.GetComponent<Image>());
            PolygonImage bgImage = go.AddComponent<PolygonImage>();
            bgImage.sprite = goImage.sprite;
            bgImage.type = Image.Type.Sliced;
            bgImage.color = goImage.color;

            Image viewportImage = viewport.GetComponent<Image>();
            GameObject.DestroyImmediate(viewport.GetComponent<Image>());
            GameObject.DestroyImmediate(viewport.GetComponent<Mask>());
            PolygonImage viewportImage2 = viewport.gameObject.AddComponent<PolygonImage>();
            viewportImage2.sprite = null;
            viewportImage2.type = Image.Type.Sliced;
            viewportImage2.color = viewportImage.color;
            viewport.gameObject.AddComponent<Mask>();

            Transform content = viewport.transform.Find("Content");
            go.name = "sv_list_view";
            viewport.name = "v_list_view";
            content.name = "c_list_view";
            ScrollRect sr = go.GetComponent<ScrollRect>();
            sr.horizontal = false;
            sr.vertical = true;

            ListView scrollView = go.AddComponent<ListView>();
            scrollView.listContainer = content.GetComponent<RectTransform>();
            scrollView.autoPlaySound = false;

            return go;
        }

        internal static GameObject CreatePageView(DefaultControls.Resources resources)
        {
            GameObject go = DefaultControls.CreateScrollView(resources);
            Transform viewport = go.transform.Find("Viewport");

            Image goImage = go.GetComponent<Image>();
            GameObject.DestroyImmediate(go.GetComponent<Image>());
            PolygonImage bgImage = go.AddComponent<PolygonImage>();
            bgImage.sprite = goImage.sprite;
            bgImage.type = Image.Type.Sliced;
            bgImage.color = goImage.color;

            Image viewportImage = viewport.GetComponent<Image>();
            GameObject.DestroyImmediate(viewport.GetComponent<Image>());
            GameObject.DestroyImmediate(viewport.GetComponent<Mask>());
            PolygonImage viewportImage2 = viewport.gameObject.AddComponent<PolygonImage>();
            viewportImage2.sprite = null;
            viewportImage2.type = Image.Type.Sliced;
            viewportImage2.color = viewportImage.color;
            viewport.gameObject.AddComponent<Mask>();

            Transform content = viewport.transform.Find("Content");
            go.name = "sv_page_view";
            viewport.name = "v_page_view";
            content.name = "c_page_view";
            ScrollRect sr = go.GetComponent<ScrollRect>();
            sr.horizontal = true;
            sr.vertical = false;

            PageView scrollView = go.AddComponent<PageView>();
            scrollView.listContainer = content.GetComponent<RectTransform>();

            return go;
        }
    }
}
