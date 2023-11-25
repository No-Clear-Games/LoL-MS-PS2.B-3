using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoClearGames
{
    [Serializable]
    public class HintData
    {
        public enum SlotState
        {
            SlotNotFound,
            Blank,
            Wrong,
            Correct
        }
        
        [SerializeField] private List<HintItem> sortedHintItems;

        [SerializeField] private float hintIntervalSeconds = 60;
        [SerializeField] private bool showOneSlotAtATime;
        [SerializeField] private bool showHintOnWrong;
        [SerializeField] private bool showDescriptionOnCorrect;

        private Dictionary<MagnetSlot, HintItem> _slotHintsDictionary;
        private List<MagnetSlot> _sortedSlotsList;

        public float HintIntervalSeconds => hintIntervalSeconds;
        public bool ShowOneSlotAtATime => showOneSlotAtATime;
        public bool ShowHintOnWrong => showHintOnWrong;
        public bool ShowDescriptionOnCorrect => showDescriptionOnCorrect;
        public List<MagnetSlot> SortedSlotsList => _sortedSlotsList;

        public void Initialize()
        {
            _sortedSlotsList = GetSortedSlotsList();
            _slotHintsDictionary = new Dictionary<MagnetSlot, HintItem>();
            
            foreach (HintItem item in sortedHintItems)
            {
                _slotHintsDictionary[item.MagnetSlot] = item;
            }
        }

        private List<MagnetSlot> GetSortedSlotsList()
        {
            List<MagnetSlot> slotList = new List<MagnetSlot>();
            foreach (HintItem item in sortedHintItems)
            {
                slotList.Add(item.MagnetSlot);
            }

            return slotList;
        }
        
        public HintItem GetFirstHintItemBlank()
        {
            foreach (HintItem item in sortedHintItems)
            {
                if (!item.MagnetSlot.Occupied)
                {
                    return item;
                }
            }

            return null;
        }

        public MagnetSlot GetLastCorrectSlot()
        {
            MagnetSlot lastCorrect = null;
            foreach (HintItem item in sortedHintItems)
            {
                SlotState slotState = GetSlotState(item.MagnetSlot);
                if (slotState == SlotState.Correct)
                {
                    lastCorrect = item.MagnetSlot;
                }
                else
                {
                    return lastCorrect;
                }
            }

            return lastCorrect;
        }
        
        public HintItem GetFirstHintItemWrong()
        {
            foreach (HintItem item in sortedHintItems)
            {
                if (item.MagnetSlot.Occupied && (Math.Abs(item.MagnetSlot.PlacedMagnet.Power - item.CorrectMagnet.Power) > 0.01f || item.MagnetSlot.PlacedMagnet.Sign != item.CorrectMagnet.Sign))
                {
                    return item;
                }
            }

            return null;
        }

        public HintItem GetSlotHintItem(MagnetSlot slot)
        {
            if (_slotHintsDictionary.TryGetValue(slot, out HintItem hint))
            {
                return hint;
            }
            return null;
        } 

        public SlotState GetSlotState(MagnetSlot slot)
        {
            if(_slotHintsDictionary.TryGetValue(slot, out HintItem hintItem))
            {
                if(slot.Equals(hintItem.MagnetSlot))
                {
                    if (!slot.Occupied)
                    {
                        return SlotState.Blank;
                    }
                    
                    if (slot.Occupied && (Math.Abs(slot.PlacedMagnet.Power - hintItem.CorrectMagnet.Power) > 0.01f || slot.PlacedMagnet.Sign != hintItem.CorrectMagnet.Sign))
                    {
                        return SlotState.Wrong;
                    }
                    
                    if (slot.Occupied && Math.Abs(slot.PlacedMagnet.Power - hintItem.CorrectMagnet.Power) < 0.01f && slot.PlacedMagnet.Sign == hintItem.CorrectMagnet.Sign)
                    {
                        return SlotState.Correct;
                    }
                }
                
            }

            return SlotState.SlotNotFound;
        }
    }
}
