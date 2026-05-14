using HarmonyLib;
using UnityEngine;
using Verse;

namespace PE_SemesterGoalTweak;

/// <summary>
/// Mod 入口：初始化设置和 Harmony 补丁，绘制设置 UI
/// </summary>
public class SemesterGoalMod : Mod
{
    public static SemesterGoalSettings settings;

    /// <summary>
    /// 输入框缓存字符串，避免每帧重置导致无法编辑
    /// </summary>
    private string inputBuffer;

    public SemesterGoalMod(ModContentPack content) : base(content)
    {
        settings = GetSettings<SemesterGoalSettings>();
        new Harmony("fishundbug.PE_SemesterGoalTweak").PatchAll();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var listing = new Listing_Standard();
        listing.Begin(inRect);

        // 当前值显示
        listing.Label(
            "学期目标最大 XP: "
            + settings.maxSemesterGoalXP.ToString("N0"));

        // 滑块：10,000 ~ 2,500,000，步长 10,000
        var sliderVal = (int)Mathf.Round(
            listing.Slider(settings.maxSemesterGoalXP, 10000f, 2500000f)
            / 10000f) * 10000;

        // 滑块值变化时同步更新输入框缓存
        if (sliderVal != settings.maxSemesterGoalXP)
        {
            settings.maxSemesterGoalXP = sliderVal;
            inputBuffer = sliderVal.ToString();
        }

        // 手动输入框
        listing.Gap(6f);
        listing.Label("手动输入（范围 10,000 ~ 2,500,000）：");
        var inputRect = listing.GetRect(28f);
        inputRect.width = 200f;

        // 初始化缓存
        inputBuffer ??= settings.maxSemesterGoalXP.ToString();

        inputBuffer = Widgets.TextField(inputRect, inputBuffer);

        // 尝试解析输入值并限制范围
        if (int.TryParse(inputBuffer, out var parsed))
        {
            parsed = Mathf.Clamp(parsed, 10000, 2500000);
            settings.maxSemesterGoalXP = parsed;
        }

        listing.Gap();
        listing.Label("修改后，下次打开创建/编辑课程界面即生效。");

        listing.End();
    }

    public override string SettingsCategory()
    {
        return "PE: 学期目标调整";
    }
}
