using System.Collections;
using NUnit.Framework;
using Tests.Editor;

namespace Modules.Converter.Tests
{
    public sealed class ConverterTests
    {
        private static readonly ItemType DefaultSourceType = ItemType.Wood;
        private static readonly ItemType DefaultTargetType = ItemType.Plank;
        private static readonly int DefaultSourceCount = 5;
        private static readonly int DefaultTargetCount = 1;
        private static readonly int DefaultTime = 1;
        private static ConvertReceipt DefaultWoodPlankReceipt =>
            new ConvertReceipt(DefaultSourceType, DefaultTargetType, DefaultSourceCount, DefaultTargetCount, DefaultTime);

        private static IEnumerable AddItemSourceSource() => new object[]
        {
            new object[] { false, 5, DefaultWoodPlankReceipt, ItemType.Wood, 5, 5, 0, 0, true },
            new object[] { false, 5, DefaultWoodPlankReceipt, ItemType.Wood, 6, 5, 1, 0, true },
            new object[] { false, 5, DefaultWoodPlankReceipt, ItemType.Plank, 1, 0, 1, 0, false },
            new object[] { false, 5, DefaultWoodPlankReceipt, ItemType.Wood, 0, 0, 0, 0, false },
            new object[] { true, 5, DefaultWoodPlankReceipt, ItemType.Wood, 5, 0, 0, 5, true },
            //if can convert then do it first and over items added to source
            new object[] { true, 5, DefaultWoodPlankReceipt, ItemType.Wood, 6, 1, 0, 5, true },
            new object[] { true, 5, DefaultWoodPlankReceipt, ItemType.Wood, 11, 5, 1, 5, true },
            new object[] { true, 5, DefaultWoodPlankReceipt, ItemType.Plank, 1, 0, 1, 0, false },
        }.FormatAsObjects(args =>
            $"Enabled: {args[0]}; Size: {args[1]}; ItemType: {args[3]}; AddCount: {args[4]}; " +
            $"ExpectedItemCount: {args[5]}; ExpectedReturnCount: {args[6]}; ExpectedConvertingCount: {args[7]}; EventTriggered: {args[8]}");

        [TestCaseSource(nameof(AddItemSourceSource))]
        public void AddItemSource(bool isEnabled, int size, ConvertReceipt receipt, ItemType itemType, int addCount,
            int expectedSourceItemCount, int expectedReturnCount, int expectConvertingCount, bool expectedEventTriggered)
        {
            var converter = new Converter(size, DefaultWoodPlankReceipt);
            var eventTriggered = false;
            converter.SetEnabled(isEnabled);
            converter.OnSourceAdded += (_, _) => { eventTriggered = true; };

            var returnCount = converter.AddSourceItem(itemType, addCount);

            Assert.AreEqual(expectedSourceItemCount, converter.GetSourceItemCount(itemType));
            Assert.AreEqual(expectConvertingCount, converter.ConvertingCount);
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
            $"Size: {args[0]}; SourceItemType: {args[2]}; SourceCount: {args[3]}; RemoveCount: {args[4]}; " +
            $"ExpectedItemCount: {args[5]}; ExpectedResult: {args[6]}, EventTriggered: {args[7]}");

        [TestCaseSource(nameof(RemoveItemSourceSource))]
        public void RemoveItemSource(int size, ConvertReceipt receipt, ItemType sourceItemType, int sourceCount, ItemType removeItemType,
            int removeCount, int expectedItemCount, bool expectedResult, bool expectedEventTriggered)
        {
            var converter = new Converter(size, DefaultWoodPlankReceipt);
            var eventTriggered = false;
            converter.SetEnabled(false);
            converter.OnSourceRemoved += (_, _) => { eventTriggered = true; };
            converter.AddSourceItem(sourceItemType, sourceCount);

            var result = converter.RemoveSourceItem(removeItemType, removeCount);

            Assert.AreEqual(expectedItemCount, converter.GetSourceItemCount(removeItemType));
            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedEventTriggered, eventTriggered);
        }

        private static IEnumerable ConvertSource() => new object[]
        {
            new object[]
            {
                10, ItemType.Wood, 5, ItemType.Plank, 1, 1,
                5, 1, true, true
            },
            new object[]
            {
                10, ItemType.Wood, 5, ItemType.Plank, 1, 1,
                4, 0, false, false
            },
        }.FormatAsObjects(args =>
            $"Size: {args[0]}; Receipt: {args[1]}: {args[2]}; {args[3]}: {args[4]}, time: {args[5]}; " +
            $"addSourceCount: {args[6]}; expectedTargetCount: {args[7]}; expectedResult: {args[8]}; eventTriggered: {args[9]};");

        [TestCaseSource(nameof(ConvertSource))]
        public void Convert(int size, ItemType sourceType, int sourceCount, ItemType targetType, int targetCount, float time, int addSourceCount,
            int expectedTargetCount, bool expectedResult, bool expectedEventTriggered)
        {
            var converter = new Converter(size, new ConvertReceipt(sourceType, targetType, sourceCount, targetCount, time));
            converter.AddSourceItem(sourceType, addSourceCount);
            var eventTriggered = false;
            converter.OnConverted += _ => { eventTriggered = true; };

            var result = converter.Convert();

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedEventTriggered, eventTriggered);
            Assert.AreEqual(expectedTargetCount, converter.GetTargetItemCount(targetType));
        }
    }
}