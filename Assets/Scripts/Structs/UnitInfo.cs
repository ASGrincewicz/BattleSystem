namespace Veganimus.BattleSystem
{
    [System.Serializable]
    public struct UnitInfo
    {
        public string unitName;
        public int hitPoints;
        public int speed;
        public int defense;
        public int accuracyMod;
        public ElementType elementType;

        public UnitInfo(string unitName, int hitPoints)
        {
            this.unitName = unitName;
            this.hitPoints = hitPoints;
            this.speed = default;
            this.defense = default;
            this.accuracyMod = default;
            this.elementType = default;
        }

        public UnitInfo(string unitName, int hitPoints, int speed, int defense, int accuracyMod)
        {
            this.unitName = unitName;
            this.hitPoints = hitPoints;
            this.speed = speed;
            this.defense = defense;
            this.accuracyMod = accuracyMod;
            this.elementType = default;
        }

        public UnitInfo(string unitName, int hitPoints, int speed, int defense, int accuracyMod,ElementType elementType)
        {
            this.unitName = unitName;
            this.hitPoints = hitPoints;
            this.speed = speed;
            this.defense = defense;
            this.accuracyMod = accuracyMod;
            this.elementType = elementType;
        }

       
    }
}