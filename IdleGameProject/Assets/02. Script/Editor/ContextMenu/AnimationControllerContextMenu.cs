#if UNITY_EDITOR
using System;
using Sirenix.Utilities;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace IdleProject.EditorClass.ContextMenu
{
    public static class AnimationControllerContextMenu
    {
        [MenuItem("CONTEXT/AnimatorOverrideController/Set Default BattleCharacter AnimationEvent")]
        private static void SetDefaultBattleCharacterAnimationEvent(MenuCommand command)
        {
            var controller = (AnimatorOverrideController)command.context;

            foreach (var clip in controller.animationClips)
            {
                if (clip.name.Contains("Death"))
                {
                    var length = clip.length;
                    AnimationUtility.SetAnimationEvents(clip, new[]
                    {
                        new AnimationEvent
                        {
                            functionName = "DeathEnd",
                            time = length,
                            messageOptions = SendMessageOptions.DontRequireReceiver
                        }
                    });
                }
                else if (clip.name.Contains("Attack"))
                {
                    var length = clip.length;
                    AnimationUtility.SetAnimationEvents(clip, new[]
                    {
                        new AnimationEvent
                        {
                            functionName = "AttackStart",
                            time = 0,
                            messageOptions = SendMessageOptions.DontRequireReceiver
                        },
                        new AnimationEvent
                        {
                            functionName = "AttackEnd",
                            time = length,
                            messageOptions = SendMessageOptions.DontRequireReceiver
                        }
                    });
                }
                else if (clip.name.Contains("Skill") && clip.name.Contains("Default") is false)
                {
                    var length = clip.length;
                    AnimationUtility.SetAnimationEvents(clip, new[]
                    {
                        new AnimationEvent
                        {
                            functionName = "SkillStart",
                            time = 0,
                            messageOptions = SendMessageOptions.RequireReceiver
                        },
                        new AnimationEvent
                        {
                            functionName = "SkillEnd",
                            time = length,
                            messageOptions = SendMessageOptions.RequireReceiver
                        }
                    });
                }
            }
        }
    }
}
#endif