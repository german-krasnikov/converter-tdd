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
            new object[] { 5, DefaultWoodPlankReceipt, ItemType.Wood, 5, 5, 0 },
            new object[] { 5, DefaultWoodPlankReceipt, ItemType.Wood, 6, 5, 1 },
            new object[] { 5, DefaultWoodPlankReceipt, ItemType.Plank, 1, 0, 1 },
        }.FormatAsObjects(args => $"Size: {args[0]}; AddCount: {args[1]}; ExpectedItemCount: {args[2]}; ExpectedReturnCount: {args[3]}");


        [TestCaseSource(nameof(AddItemSourceSource))]
        public void AddItemSource(int size, ConvertReceipt receipt, ItemType itemType, int addCount, int expectedItemCount, int expectedReturnCount)
        {
            var converter = new Converter(size, DefaultWoodPlankReceipt);

            var actualReturnCount = converter.AddSourceItem(itemType, addCount);

            Assert.AreEqual(expectedItemCount, converter.GetSourceItemCount(itemType));
            Assert.AreEqual(expectedReturnCount, actualReturnCount);
        }
    }
}