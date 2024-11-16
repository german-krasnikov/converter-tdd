using NUnit.Framework;

namespace Homework
{
    public class StorageTests
    {
        [Test]
        public void AddItem()
        {
            var storage = new Storage(5);
            
            storage.AddItem(ItemType.Wood, 5);
            
            Assert.AreEqual(5, storage.GetItemCount(ItemType.Wood));
        }
    }
}