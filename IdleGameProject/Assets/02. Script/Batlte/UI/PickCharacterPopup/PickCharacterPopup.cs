using System;
using IdleProject.Battle.AI;
using IdleProject.Battle.Spawn;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IdleProject.Battle.UI
{
    public class PickCharacterPopup : UIPopup
    {
        [SerializeField] private ScrollRect pickCharacterScrollView;

        [SerializeField] private SelectSlot slotPrefab;
        [SerializeField] private SlotUI dorpSlot;

        private CharacterData _pickData;
        
        public override void Initialized()
        {
            foreach (var userHeroName in DataManager.Instance.DataController.userData.UserHeroList)
            {
                CreateSlot(DataManager.Instance.GetData<CharacterData>(userHeroName));
            }
            
            UIManager.Instance.GetUI<UIButton>("BattleStartButton").Button.onClick.AddListener(StartBattle);
            
            dorpSlot.gameObject.SetActive(false);
        }
        
        private void CreateSlot(CharacterData characterData)
        {
            var slot = Instantiate(slotPrefab, pickCharacterScrollView.content);
            slot.SetData(characterData);
            slot.beginDragEvent.AddListener(OnSlotDragBegin);
            slot.endDragEvent.AddListener(OnSlotDragEnd);
            slot.dragEvent.AddListener(OnSlotDrag);
        }

        private void OnSlotDragBegin(PointerEventData eventData)
        {
            dorpSlot.gameObject.SetActive(true);

            var selectCharacterData = ExecuteEvents.GetEventHandler<IDragHandler>(eventData.pointerCurrentRaycast.gameObject)
                .GetComponent<SlotUI>().GetData<CharacterData>();

            if (selectCharacterData)
            {
                _pickData = selectCharacterData;
            }
            
            dorpSlot.SetData(selectCharacterData);
        }
        
        private void OnSlotDragEnd(PointerEventData eventData)
        {
            if (_pickData is null) return;
            
            dorpSlot.gameObject.SetActive(false);

            var ray = Camera.main.ScreenPointToRay(eventData.position);
            var hits = Physics.RaycastAll(ray, 100f);

            foreach (var hit in hits)
            {
                var spawnPosition = hit.collider.gameObject.GetComponent<SpawnPosition>();
                if (!spawnPosition) 
                    continue;
                
                if (spawnPosition.SpawnAIType == CharacterAIType.Player)
                {
                    GameManager.GetCurrentSceneManager<BattleManager>().spawnController.SpawnCharacterBySpawnPosition(_pickData, spawnPosition).Forget();
                }
            }

            _pickData = null;
        }

        private void OnSlotDrag(PointerEventData eventData)
        {
            dorpSlot.transform.position = eventData.position;
        }
        
        
        private void StartBattle()
        {
            GameManager.GetCurrentSceneManager<BattleManager>().BattleStateEventBus.ChangeEvent(BattleStateType.Battle);
            GameManager.GetCurrentSceneManager<BattleManager>().GameStateEventBus.ChangeEvent(GameStateType.Play);
            
            ClosePopup();
        }
    }
}