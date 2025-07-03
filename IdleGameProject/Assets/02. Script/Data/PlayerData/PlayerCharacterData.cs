namespace IdleProject.Data.Player
{
    [System.Serializable]
    public class PlayerCharacterData
    {
        public string characterName;
        public int level = 1;
        public int exp;

        public string equipmentWeaponIndex;
        public string equipmentHelmetIndex;
        public string equipmentArmorIndex;
        public string equipmentGloveIndex;
        public string equipmentBootsIndex;
        public string equipmentAccessoryIndex;
    }
}
