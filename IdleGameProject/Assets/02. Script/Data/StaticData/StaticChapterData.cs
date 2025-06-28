using System.Collections.Generic;
using IdleProject.Core;
using UnityEngine;

namespace IdleProject.Data.StaticData
{
    [CreateAssetMenu(fileName = "ChapterData", menuName = "Scriptable Objects/ChapterData")]
    public class StaticChapterData : StaticData
    {
        public int chapterIndex;
        public string chapterImage;

        public List<StageInfo> stageInfoList = new List<StageInfo>();
    }
}