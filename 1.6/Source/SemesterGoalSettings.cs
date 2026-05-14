using Verse;

namespace PE_SemesterGoalTweak;

/// <summary>
/// 模组设置：学期目标最大经验值
/// </summary>
public class SemesterGoalSettings : ModSettings
{
    /// <summary>
    /// 学期目标最大 XP，默认与原版一致
    /// </summary>
    public int maxSemesterGoalXP = 100000;

    /// <summary>
    /// 供 Transpiler 替换 float 常量时调用（滑块 max 参数）
    /// </summary>
    public static float GetMaxXPFloat()
    {
        return SemesterGoalMod.settings.maxSemesterGoalXP;
    }

    /// <summary>
    /// 供 Transpiler 替换 int 常量时调用（标签文本）
    /// </summary>
    public static int GetMaxXPInt()
    {
        return SemesterGoalMod.settings.maxSemesterGoalXP;
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref maxSemesterGoalXP, "maxSemesterGoalXP", 100000);
    }
}
