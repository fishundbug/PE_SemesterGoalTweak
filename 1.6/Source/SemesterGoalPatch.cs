using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using ProgressionEducation;

namespace PE_SemesterGoalTweak;

/// <summary>
/// Harmony Transpiler 补丁：
/// 将 SkillClassLogic.DrawSemesterGoalUI 中硬编码的 100000 替换为
/// 从 ModSettings 动态读取的值。
/// </summary>
[HarmonyPatch]
public static class SemesterGoalPatch
{
    /// <summary>
    /// 指定要补丁的目标方法（private 方法需手动指定）
    /// </summary>
    static MethodBase TargetMethod()
    {
        return AccessTools.Method(typeof(SkillClassLogic), "DrawSemesterGoalUI");
    }

    /// <summary>
    /// Transpiler：遍历 IL 指令，将 100000 常量替换为读取设置的方法调用
    /// </summary>
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var getMaxFloat = AccessTools.Method(
            typeof(SemesterGoalSettings), nameof(SemesterGoalSettings.GetMaxXPFloat));
        var getMaxInt = AccessTools.Method(
            typeof(SemesterGoalSettings), nameof(SemesterGoalSettings.GetMaxXPInt));

        foreach (var instruction in instructions)
        {
            // 替换 float 100000f（HorizontalSlider 的 max 参数）
            if (instruction.opcode == OpCodes.Ldc_R4
                && instruction.operand is float floatVal
                && floatVal == 100000f)
            {
                yield return new CodeInstruction(OpCodes.Call, getMaxFloat);
                continue;
            }

            // 替换 int 100000（ToString("N0") 的标签值）
            if (instruction.opcode == OpCodes.Ldc_I4
                && instruction.operand is int intVal
                && intVal == 100000)
            {
                yield return new CodeInstruction(OpCodes.Call, getMaxInt);
                continue;
            }

            yield return instruction;
        }
    }
}
