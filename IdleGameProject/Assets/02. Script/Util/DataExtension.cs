using IdleProject.Core.GameData;
using IdleProject.Data.StaticData;

namespace IdleProject.Util
{
    public static class DataExtension
    {

        public static StaticStageData GetFirstStage()
        {
            return DataManager.Instance.GetData<StaticStageData>("1-1");
        }
        
        public static bool TryGetNextStage(StaticStageData currentStage, out StaticStageData nextStage)
        {
            nextStage = null;
            
            var nextStageIndex = currentStage.stageIndex + 1;
            var currentChapter = DataManager.Instance.GetData<StaticChapterData>(currentStage.chapterIndex.ToString());
            if (currentChapter.stageInfoList.Count >= nextStageIndex)
                // 현재 챕터에 다음 스테이지 있음
            {
                nextStage = DataManager.Instance.GetData<StaticStageData>($"{currentChapter.chapterIndex}-{nextStageIndex}");
            }
            else
                // 다음 챕터 확인
            {
                var nextChapterIndex = currentChapter.chapterIndex + 1;
                if (DataManager.Instance.TryGetData(nextChapterIndex.ToString(), out StaticChapterData nextChapter))
                {
                    nextStage = DataManager.Instance.GetData<StaticStageData>($"{nextChapter.chapterIndex}-{1}");
                }
            }

            return nextStage is not null;
        }
    }
}