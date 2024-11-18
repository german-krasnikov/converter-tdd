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

        [TestCaseSource(nameof(AddItemSourceSource))]
        public void AddItemSourceEvent(bool isEnabled, int size, ConvertReceipt receipt, ItemType itemType, int addCount,
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

        private static IEnumerable RemoveItemTargetSource() => new object[]
        {
            new object[] { 5, DefaultWoodPlankReceipt, ItemType.Wood, 5, ItemType.Wood, 5, 0, true, true },
            new object[] { 5, DefaultWoodPlankReceipt, ItemType.Wood, 5, ItemType.Wood, 6, 5, false, false },
            new object[] { 5, DefaultWoodPlankReceipt, ItemType.Wood, 0, ItemType.Wood, 1, 0, false, false },
            new object[] { 5, DefaultWoodPlankReceipt, ItemType.Wood, 5, ItemType.Plank, 1, 0, false, false },
        }.FormatAsObjects(args =>
            $"Size: {args[0]}; SourceItemType: {args[2]}; SourceCount: {args[3]}; RemoveCount: {args[4]}; " +
            $"ExpectedItemCount: {args[5]}; ExpectedResult: {args[6]}, EventTriggered: {args[7]}");

        [TestCaseSource(nameof(RemoveItemTargetSource))]
        public void RemoveItemTargetSource(int size, ConvertReceipt receipt, ItemType sourceItemType, int sourceCount, ItemType removeItemType,
            int removeCount, int expectedItemCount, bool expectedResult, bool expectedEventTriggered)
        {
            var converter = new Converter(size, DefaultWoodPlankReceipt);
            var eventTriggered = false;
            converter.SetEnabled(false);
            converter.OnTargetRemoved += (_, _) => { eventTriggered = true; };
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
            converter.SetEnabled(true);
            converter.AddSourceItem(sourceType, addSourceCount);
            var eventTriggered = false;
            converter.OnConverted += _ => { eventTriggered = true; };

            var result = converter.Convert();

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedEventTriggered, eventTriggered);
            Assert.AreEqual(expectedTargetCount, converter.GetTargetItemCount(targetType));
        }

        private static IEnumerable NeedStartConvertingSource() => new object[]
        {
            new object[] { 5, 1, false, false, false, false },
            new object[] { 10, 5, false, false, false, false },
            new object[] { 10, 5, true, false, false, true },
        }.FormatAsObjects(args =>
            $"Size: {args[0]}; addSourceCount: {args[1]}; isEnabled: {args[2]}; isInProgress: {args[3]}; expectedResult: {args[4]}");

        [TestCaseSource(nameof(NeedStartConvertingSource))]
        public void NeedStartConverting(int size, int addSourceCount, bool isEnabled, bool isInProgress, bool expectedResult,
            bool expectedIsInProgress)
        {
            //ARRANGE
            var converter = new Converter(size, DefaultWoodPlankReceipt);
            converter.SetEnabled(isEnabled);

            if (isInProgress)
            {
                converter.AddSourceItem(DefaultSourceType, DefaultSourceCount);
            }

            converter.AddSourceItem(DefaultSourceType, addSourceCount);

            //ACT
            var result = converter.NeedStartConverting();
            //ASSERT
            Assert.AreEqual(expectedResult, result);
        }

        private static IEnumerable CheckStartConvertingSource() => new object[]
        {
            new object[] { 5, 1, false, false, false, 1, 0, false },
            new object[] { 10, 5, false, false, false, 5, 0, false },
            new object[] { 10, 5, true, false, false, 0, 5, true },
            new object[] { 10, 5, true, true, false, 5, 5, false },
        }.FormatAsObjects(args =>
            $"Size: {args[0]}; addSourceCount: {args[1]}; isEnabled: {args[2]}; isInProgress: {args[3]}; " +
            $"expectedResult: {args[4]}; expectedConvertingCount: {args[5]}, eventTriggered: {args[6]}");

        [TestCaseSource(nameof(CheckStartConvertingSource))]
        public void CheckStartConverting(int size, int addSourceCount, bool isEnabled, bool isInProgress, bool expectedResult,
            int expectedSourceCount, int expectedConvertingCount, bool expectedEventTriggered)
        {
            //ARRANGE
            var converter = new Converter(size, DefaultWoodPlankReceipt);
            converter.SetEnabled(isEnabled);

            if (isInProgress)
            {
                converter.AddSourceItem(DefaultSourceType, DefaultSourceCount);
            }

            var eventTriggered = false;
            converter.OnStartConverting += _ => { eventTriggered = true; };
            converter.AddSourceItem(DefaultSourceType, addSourceCount);

            //ACT
            var result = converter.NeedStartConverting();
            //ASSERT
            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedEventTriggered, eventTriggered);
            Assert.AreEqual(expectedSourceCount, converter.GetSourceItemCount());
            Assert.AreEqual(expectedConvertingCount, converter.ConvertingCount);
        }

        private static IEnumerable StopSource() => new object[]
        {
            new object[] { 5, 0, 0, true, 0, false },
            new object[] { 5, 5, 0, true, 5, true },
            new object[] { 5, 5, 5, true, 5, true },
            new object[] { 5, 0, 5, false, 5, false },
        }.FormatAsObjects(args =>
            $"Size: {args[0]}; converting: {args[1]}; Source: {args[2]}; isEnabled: {args[3]}; expectedSourceCount: {args[4]}; eventTriggered: {args[5]}");

        [TestCaseSource(nameof(StopSource))]
        public void Stop(int size, int convertingCount, int sourceCount, bool isEnabled, int expectedSourceCount, bool expectedEventTriggered)
        {
            var sourceStorage = new Storage(size);
            var targetStorage = new Storage(size);
            var converter = new Converter(size, DefaultWoodPlankReceipt, sourceStorage, targetStorage, convertingCount);
            converter.SetEnabled(true);
            sourceStorage.AddItem(DefaultSourceType, sourceCount);
            var eventTriggered = false;
            converter.OnStopConverting += _ => { eventTriggered = true; };

            converter.SetEnabled(false);

            Assert.AreEqual(expectedSourceCount, converter.GetSourceItemCount());
            Assert.AreEqual(expectedEventTriggered, eventTriggered);
            Assert.AreEqual(0, converter.ConvertingCount);
        }

        private static IEnumerable CanConvertSource() => new object[]
        {
            new object[] { 5, 0, 0, true, false },
            new object[] { 5, 0, 5, true, true },
            new object[] { 5, 0, 5, false, false },
            new object[] { 5, 5, 5, true, false },
        }.FormatAsObjects(args =>
            $"Size: {args[0]}; target: {args[1]}; converting: {args[2]}; isEnabled: {args[3]}; expectedResult: {args[4]}");

        [TestCaseSource(nameof(CanConvertSource))]
        public void CanConvert(int size, int targetCount, int convertingCount, bool isEnabled, bool expectedResult)
        {
            var sourceStorage = new Storage(size);
            var targetStorage = new Storage(size);
            var converter = new Converter(size, DefaultWoodPlankReceipt, sourceStorage, targetStorage, convertingCount);
            converter.SetEnabled(isEnabled);
            targetStorage.AddItem(DefaultTargetType, targetCount);

            var result = converter.CanConvert();

            Assert.AreEqual(expectedResult, result);
        }

        private static IEnumerable CanConvertNextSource() => new object[]
        {
            new object[] { 5, 0, 0, 5, true, true },
            new object[] { 5, 0, 0, 4, true, false },
            new object[] { 5, 0, 5, 5, true, false },
            new object[] { 5, 0, 0, 5, false, false },
            new object[] { 5, 5, 0, 5, true, false },
            new object[] { 5, 4, 0, 5, true, true },
        }.FormatAsObjects(args =>
            $"Size: {args[0]}; target: {args[1]}; converting: {args[2]}; source: {args[3]}; isEnabled: {args[4]}; expectedResult: {args[5]}");

        [TestCaseSource(nameof(CanConvertNextSource))]
        public void CanConvertNext(int size, int targetCount, int convertingCount, int addSourceCount, bool isEnabled, bool expectedResult)
        {
            var sourceStorage = new Storage(size);
            var targetStorage = new Storage(size);
            var converter = new Converter(size, DefaultWoodPlankReceipt, sourceStorage, targetStorage, convertingCount);
            converter.SetEnabled(isEnabled);
            sourceStorage.AddItem(DefaultSourceType, addSourceCount);
            targetStorage.AddItem(DefaultTargetType, targetCount);

            var result = converter.CanConvertNext();

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void ConvertOneItemPositiveCase()
        {
            var converter = new Converter(10, DefaultWoodPlankReceipt);
            converter.SetEnabled(true);
            var onStartConvertingTriggered = false;
            converter.OnStartConverting += _ => { onStartConvertingTriggered = true; };
            var onConvertedTriggered = false;
            converter.OnConverted += _ => { onConvertedTriggered = true; };
            var onSourceAddedTriggered = false;
            converter.OnSourceAdded += (_, _) => { onSourceAddedTriggered = true; };
            converter.AddSourceItem(DefaultSourceType, DefaultSourceCount);

            converter.Update(DefaultTime);

            Assert.AreEqual(converter.GetSourceItemCount(), 0);
            Assert.AreEqual(converter.GetTargetItemCount(), DefaultTargetCount);
            Assert.AreEqual(converter.ConvertingCount, 0);
            Assert.AreEqual(converter.IsInProgress, false);
            Assert.AreEqual(onStartConvertingTriggered, true);
            Assert.AreEqual(onSourceAddedTriggered, true);
            Assert.AreEqual(onConvertedTriggered, true);
        }

        [Test]
        public void ConvertTwoItemPositiveCase()
        {
            var converter = new Converter(10, DefaultWoodPlankReceipt);
            converter.SetEnabled(true);
            var onStartConvertingTriggered = false;
            converter.OnStartConverting += _ => { onStartConvertingTriggered = true; };
            var onConvertedTriggered = false;
            converter.OnConverted += _ => { onConvertedTriggered = true; };
            var onSourceAddedTriggered = false;
            converter.OnSourceAdded += (_, _) => { onSourceAddedTriggered = true; };
            converter.AddSourceItem(DefaultSourceType, DefaultSourceCount);
            converter.AddSourceItem(DefaultSourceType, DefaultSourceCount);

            converter.Update(DefaultTime);
            converter.Update(DefaultTime);

            Assert.AreEqual(converter.GetSourceItemCount(), 0);
            Assert.AreEqual(converter.GetTargetItemCount(), DefaultTargetCount * 2);
            Assert.AreEqual(converter.ConvertingCount, 0);
            Assert.AreEqual(converter.IsInProgress, false);
            Assert.AreEqual(onStartConvertingTriggered, true);
            Assert.AreEqual(onSourceAddedTriggered, true);
            Assert.AreEqual(onConvertedTriggered, true);
        }
    }
}