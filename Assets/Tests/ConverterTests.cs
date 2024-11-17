using System.Collections;
using NUnit.Framework;
using Tests.Editor;

namespace Homework
{
    public sealed class ConverterTests
    {
        private static ConvertReceipt DefaultWoodPlankReceipt => new ConvertReceipt(ItemType.Wood, ItemType.Plank, 5, 1, 1);

        private static IEnumerable AddItemSourceSource() => new object[]
        {
            new object[] { 5, DefaultWoodPlankReceipt, ItemType.Wood, 5, 5, 0, true },
            new object[] { 5, DefaultWoodPlankReceipt, ItemType.Wood, 6, 5, 1, true },
            new object[] { 5, DefaultWoodPlankReceipt, ItemType.Plank, 1, 0, 1, false },
            new object[] { 5, DefaultWoodPlankReceipt, ItemType.Wood, 0, 0, 0, false },
        }.FormatAsObjects(args =>
            $"Size: {args[0]}; ItemType: {args[2]}; AddCount: {args[3]}; ExpectedItemCount: {args[4]}; ExpectedReturnCount: {args[5]}, EventTriggered: {args[6]}");

        [TestCaseSource(nameof(AddItemSourceSource))]
        public void AddItemSource(int size, ConvertReceipt receipt, ItemType itemType, int addCount, int expectedItemCount, int expectedReturnCount,
            bool expectedEventTriggered)
        {
            var converter = new Converter(size, DefaultWoodPlankReceipt);
            var eventTriggered = false;
            converter.OnSourceAdded += (_, _) => { eventTriggered = true; };

            var returnCount = converter.AddSourceItem(itemType, addCount);

            Assert.AreEqual(expectedItemCount, converter.GetSourceItemCount(itemType));
            Assert.AreEqual(expectedReturnCount, returnCount);
            Assert.AreEqual(expectedEventTriggered, eventTriggered);
        }

        private static IEnumerable RemoveItemSourceSource() => new object[]
        {
            new object[] { 5, DefaultWoodPlankReceipt, ItemType.Wood, 5, ItemType.Wood, 5, 0, true, true },
            new object[] { 5, DefaultWoodPlankReceipt, ItemType.Wood, 5, ItemType.Wood, 6, 5, false, false },
            new object[] { 5, DefaultWoodPlankReceipt, ItemType.Wood, 0, ItemType.Wood, 1, 0, false, false },
            new object[] { 5, DefaultWoodPlankReceipt, ItemType.Wood, 5, ItemType.Plank, 1, 0, false, false },
        }.FormatAsObjects(args =>
            $"Size: {args[0]}; SourceItemType: {args[2]}; SourceCount: {args[3]}; RemoveCount: {args[4]}; ExpectedItemCount: {args[5]}; ExpectedResult: {args[6]}, EventTriggered: {args[7]}");

        [TestCaseSource(nameof(RemoveItemSourceSource))]
        public void RemoveItemSource(int size, ConvertReceipt receipt, ItemType sourceItemType, int sourceCount, ItemType removeItemType,
            int removeCount, int expectedItemCount, bool expectedResult, bool expectedEventTriggered)
        {
            var converter = new Converter(size, DefaultWoodPlankReceipt);
            var eventTriggered = false;
            converter.OnSourceRemoved += (_, _) => { eventTriggered = true; };
            converter.AddSourceItem(sourceItemType, sourceCount);

            var result = converter.RemoveSourceItem(removeItemType, removeCount);

            Assert.AreEqual(expectedItemCount, converter.GetSourceItemCount(removeItemType));
            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedEventTriggered, eventTriggered);
        }
    }
}