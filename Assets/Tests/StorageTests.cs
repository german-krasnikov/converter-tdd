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
        }.FormatAsObjects(args => $"Size: {args[0]}; AddCount: {args[1]}; ExpectedItemCount: {args[2]}; ExpectedReturnCount: {args[3]}");

        private static IEnumerable RemoveItemSource() => new object[]
        {
            new object[] { 5, 5, 5, 0, true },
            new object[] { 5, 5, 6, 5, false },
        }.FormatAsObjects(args => $"Size: {args[0]}; itemCount: {args[1]}; RemoveCount: {args[2]}; ExpectedItemCount: {args[3]}; Result: {args[4]}");

        [TestCaseSource(nameof(AddItemSource))]
        public void AddItem(int size, int addCount, int expectedItemCount, int expectedReturnCount)
        {
            var storage = new Storage(size);

            var actualReturnCount = storage.AddItem(ItemType.Wood, addCount);

            Assert.AreEqual(storage.GetItemCount(ItemType.Wood), expectedItemCount);
            Assert.AreEqual(actualReturnCount, expectedReturnCount);
        }

        [TestCaseSource(nameof(RemoveItemSource))]
        public void RemoveItem(int size, int itemCount, int removeCount, int expectedItemCount, bool expectedResult)
        {
            var storage = new Storage(size);
            storage.AddItem(ItemType.Wood, itemCount);

            var result = storage.RemoveItem(ItemType.Wood, removeCount);

            Assert.AreEqual(expectedItemCount, storage.GetItemCount(ItemType.Wood));
            Assert.AreEqual(expectedResult, result);
        }
    }
}