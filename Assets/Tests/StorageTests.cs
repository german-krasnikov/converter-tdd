using System.Collections;
using NUnit.Framework;
using Tests.Editor;

namespace Homework
{
    public class StorageTests
    {
        private static IEnumerable AddItemSource() => new object[]
        {
            new object[] { 5, 5, 5, 0 },
            new object[] { 5, 6, 5, 1 },
        }.FormatAsObjects(args => $"Size: {args[0]}; AddCount: {args[1]}; Return: {args[2]}");

        [TestCaseSource(nameof(AddItemSource))]
        public void AddItem(int size, int addCount, int expectedItemCount, int expectedReturnCount)
        {
            var storage = new Storage(size);

            var actualReturnCount = storage.AddItem(ItemType.Wood, addCount);

            Assert.AreEqual(storage.GetItemCount(ItemType.Wood), expectedItemCount);
            Assert.AreEqual(actualReturnCount, expectedReturnCount);
        }
    }
}