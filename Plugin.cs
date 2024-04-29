using BepInEx;
using BepInEx.Logging;
using BoplFixedMath;
using HarmonyLib;
using System;
using System.Reflection;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ReplaysLink
{
    [BepInPlugin("com.PizzaMan730.ReplaysLink", "ReplaysLink", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("ReplaysLink has loaded!");

            Harmony harmony = new("com.PizzaMan730.ReplaysLink");

            harmony.PatchAll(typeof(Patches));
        }

        //Credit where credit is due. A lot of the code for the text I learned from WackyModer.
        private void Update()
        {
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                try
                {
                    UnityEngine.GameObject.Destroy(GameObject.Find("replayText"));
                    return;
                }
                catch
                {
                    return;
                }
            }
            this.CreateText();
        }

        public void CreateText()
        {
            Canvas canvas = GameObject.Find("Canvas (1)").GetComponent<Canvas>();
            this.replayText = new GameObject("replayText", new Type[]
				{
					typeof(RectTransform),
					typeof(TextMeshProUGUI)
				});
            this.replayText.transform.SetParent(canvas.transform);

            TextMeshProUGUI text = this.replayText.GetComponent<TextMeshProUGUI>();
            text.text = "Replays";
            text.font = LocalizedText.localizationTable.GetFont(Settings.Get().Language, false);
			text.color = Color.black;
			text.raycastTarget = false;
			text.fontSize = 0.27f;
			text.alignment = TextAlignmentOptions.Left;
			RectTransform box = this.replayText.GetComponent<RectTransform>();
			float height = canvas.GetComponent<RectTransform>().rect.height;
			float width = canvas.GetComponent<RectTransform>().rect.width;
			box.anchorMin = new Vector2(1f, 0f);
			box.anchorMax = new Vector2(1f, 0f);
			box.sizeDelta = new Vector2(7f, 0.5f);
			box.anchoredPosition = new Vector2(-2450f, 180f);
			box.pivot = new Vector2(1f, 0f);

        }

        public GameObject replayText;
	}

	[HarmonyPatch]
	public class Patches
	{
		[HarmonyPatch(typeof(DiscordLink), "OnClick")]
        [HarmonyPrefix]

        public static bool OnClickPatch()
        {
            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string replayLocation = userProfile + @"\AppData\LocalLow\Johan Gronvall\BoplBattle\replays";
            System.Diagnostics.Process.Start("explorer.exe", replayLocation);
            return false;
        }
	}
            //%USERPROFILE%\AppData\LocalLow\Johan Gronvall\BoplBattle\replays
}


//System.Diagnostics.Process.Start("explorer.exe", @"c:\teste");

//Useful classes: MainMenu, DiscordLink, AnimateInOutUI, PlayButton, RectTransform, MenuControls

