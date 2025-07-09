using IdleProject.Core;
using IdleProject.Lobby.UI.StagePanel;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using IdleProject.EditorClass;
#endif

namespace IdleProject.Data.StaticData.Skill
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
    public class StaticSkillData : StaticData
    {
        public string skillName;
        public string skillDesc;

        public string skillDirectingEffect;
        public string skillHitEffect;

        public SkillType skillType;
        public SkillRangeType skillRangeType;
        public float skillRange;
        public SkillTargetType targetType;
        
        public string skillValue;

#if UNITY_EDITOR
        [Button]
        private void CreateProjectileData()
        {
            var createData = StaticDataCreator.CreateStaticData<StaticSkillProjectileData>($"{name}_projectile");

            Selection.activeInstanceID = createData.GetInstanceID();
            
            SetPath(createData);
        }

        [Button]
        private void CreateBuffData()
        {
            var createData = StaticDataCreator.CreateStaticData<StaticSkillBuffData>($"{name}_buff");

            Selection.activeInstanceID = createData.GetInstanceID();
            
            SetPath(createData);
        }

        private void SetPath<T>(T createData) where T : StaticData
        {
            // 현재 스크립트 오브젝트 위치를 기준으로 경로 계산
            var currentAssetPath = AssetDatabase.GetAssetPath(this);
            var currentDirectory = Path.GetDirectoryName(currentAssetPath);

            // 생성된 오브젝트의 경로
            var newAssetPath = AssetDatabase.GetAssetPath(createData);
            var newFileName = Path.GetFileName(newAssetPath);

            // 새로운 경로로 이동
            var targetPath = Path.Combine(currentDirectory, newFileName);
            AssetDatabase.MoveAsset(newAssetPath, targetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }
}