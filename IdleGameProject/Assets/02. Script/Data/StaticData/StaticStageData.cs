using System.Collections.Generic;
using System.Linq;
using IdleProject.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Data.StaticData
{
    [CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/StageData")]
    public class StaticStageData : StaticData
    {
        public int chapterIndex = 1;
        public int stageIndex = 1;
        public string stageName;
        
        public FormationInfo stageFormation;

        public List<RewardInfo> rewardList;
        
        public override string Index => $"{chapterIndex}-{stageIndex}";
        
        [BoxGroup("Creator"), Button]
        private void CreateRewardConsumableItem()
        {
            rewardList.Add(new RewardInfo
            {
                rewardType = RewardType.ConsumableItem,
                count = 1
            });
            
            rewardList = rewardList.OrderBy(info => (int)info.rewardType).ToList();
        }

        [BoxGroup("Creator"), Button]
        private void CreateRewardEquipmentItem()
        {
            rewardList.Add(new RewardInfo
            {
                rewardType = RewardType.EquipmentItem,
                count = 1
            });
            
            rewardList = rewardList.OrderBy(info => (int)info.rewardType).ToList();
        }
    }
}
