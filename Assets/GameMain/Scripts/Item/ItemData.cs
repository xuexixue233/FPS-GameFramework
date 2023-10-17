namespace FPS
{
    public class ItemData
    {
        private int _id;

        private int _typeId;

        private string _name;

        public ItemData(int itemId,int typeId)
        {
            _id = itemId;
            _typeId = typeId;
        }

        public int ID => _id;

        public int TypeId => _typeId;

        public string Name => _name;
        
    }
}