using UnityEngine;
using TMPro;
using System.Collections.Generic;
namespace StylizedFonts
{
    public class TextFontManager : MonoBehaviour
    {
        public List<TMP_Text> texts;
        public List<Texture> textures;
        public List<TMP_FontAsset> fontAssets;

        private int currentFontIndex = 0;
        private int currentTextureIndex = 0;

        // 方法1：替换所有TMP_Text的字体为列表中的下一个字体
        public void ReplaceFonts()
        {
            currentFontIndex = (currentFontIndex + 1) % fontAssets.Count;
            foreach (TMP_Text text in texts)
            {
                text.font = fontAssets[currentFontIndex];
            }
            foreach (TMP_Text text in texts)
            {
                if (currentTextureIndex < textures.Count)
                {
                    if (text.fontSharedMaterial.HasProperty("_FaceTex"))
                    {
                        text.fontSharedMaterial.SetTexture("_FaceTex", textures[currentTextureIndex]);
                    }
                    if (text.fontSharedMaterial.HasProperty("_OutlineTex"))
                    {
                        text.fontSharedMaterial.SetTexture("_OutlineTex", textures[currentTextureIndex]);
                    }
                }
                text.gameObject.SetActive(false);
                text.gameObject.SetActive(true);
            }
        }

        // 方法2：替换所有TMP_Text的Outline贴图为列表中的下一个Texture
        public void ReplaceOutlineTextures()
        {
            currentTextureIndex = (currentTextureIndex + 1) % textures.Count;
            foreach (TMP_Text text in texts)
            {
                if (text.fontSharedMaterial.HasProperty("_OutlineTex"))
                {
                    text.fontSharedMaterial.SetTexture("_OutlineTex", textures[currentTextureIndex]);
                }
            }
        }
        public void ReplaceFontTextures()
        {
            currentTextureIndex = (currentTextureIndex + 1) % textures.Count;
            foreach (TMP_Text text in texts)
            {
                if (text.fontSharedMaterial.HasProperty("_FaceTex"))
                {
                    text.fontSharedMaterial.SetTexture("_FaceTex", textures[currentTextureIndex]);
                }
                if (text.fontSharedMaterial.HasProperty("_OutlineTex"))
                {
                    text.fontSharedMaterial.SetTexture("_OutlineTex", textures[currentTextureIndex]);
                }
                text.gameObject.SetActive(false);
                text.gameObject.SetActive(true);
            }

        }
    }
}