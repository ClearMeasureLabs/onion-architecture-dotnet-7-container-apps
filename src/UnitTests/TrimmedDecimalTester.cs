using ProgrammingWithPalermo.ChurchBulletin.Core.Model;
using Shouldly;

namespace ProgrammingWithPalermo.ChurchBulletin.UnitTests;

[TestFixture]
public class TrimmedDecimalTester
{
    [Test]
    public void Scratch()
    {
        var myDecimal = 77.2300m;
        TrimmedDecimal val = myDecimal;
        UseObject(val);
    }

    private void UseObject(object val)
    {
        Console.WriteLine(val);
        Console.WriteLine(val);
        Console.WriteLine(val);
        Console.WriteLine(val);
    }

    [Test]
    public void ShouldRenderWithoutTrailingZeroes()
    {
        TrimmedDecimal[] numbers =
        {
            1.123456000000000000m,
            2.000000m,
            3.100000m,
            4.120000m,
            5.123000m,
            6.123400m,
            7.123450m,
            8.123456m
        };
        AssertDecimalIsShortened(numbers);
    }

    [Test]
    public void ShouldCompareOtherDecimalIdNumber()
    {
        TrimmedDecimal baseNumber = 1.123400m;
        TrimmedDecimal copyOfBase = 1.1234m;
        TrimmedDecimal largerThanBase = 9.999956m;

        //same value
        baseNumber.Equals(copyOfBase).ShouldBeTrue();
        baseNumber.CompareTo(copyOfBase).ShouldBe(0);
        (baseNumber == copyOfBase).ShouldBeTrue();
        (baseNumber != copyOfBase).ShouldBeFalse();
        (baseNumber >= copyOfBase).ShouldBeTrue();
        (baseNumber <= copyOfBase).ShouldBeTrue();
        (baseNumber > copyOfBase).ShouldBeFalse();
        (baseNumber < copyOfBase).ShouldBeFalse();

        //different value
        baseNumber.Equals(largerThanBase).ShouldBeFalse();
        baseNumber.CompareTo(largerThanBase).ShouldBeLessThan(0);
        largerThanBase.CompareTo(baseNumber).ShouldBeGreaterThan(0);
        (baseNumber == largerThanBase).ShouldBeFalse();
        (baseNumber != largerThanBase).ShouldBeTrue();
        (baseNumber >= largerThanBase).ShouldBeFalse();
        (baseNumber > largerThanBase).ShouldBeFalse();
        (baseNumber <= largerThanBase).ShouldBeTrue();
        (baseNumber < largerThanBase).ShouldBeTrue();
        (largerThanBase >= baseNumber).ShouldBeTrue();
        (largerThanBase > baseNumber).ShouldBeTrue();
        (largerThanBase <= baseNumber).ShouldBeFalse();
        (largerThanBase < baseNumber).ShouldBeFalse();
    }

    [Test]
    public void ShouldCompareDecimalValue()
    {
        var baseAsValue = 1.1234m;
        TrimmedDecimal baseNumber = baseAsValue;
        var largerThanBaseAsValue = 9.9995m;
        TrimmedDecimal largerThanBase = largerThanBaseAsValue;

        //same value
        (baseNumber == baseAsValue).ShouldBeTrue();
        (baseNumber != baseAsValue).ShouldBeFalse();
        (baseNumber >= baseAsValue).ShouldBeTrue();
        (baseNumber <= baseAsValue).ShouldBeTrue();
        (baseNumber > baseAsValue).ShouldBeFalse();
        (baseNumber < baseAsValue).ShouldBeFalse();
        (baseAsValue == baseNumber).ShouldBeTrue();
        (baseAsValue != baseNumber).ShouldBeFalse();
        (baseAsValue >= baseNumber).ShouldBeTrue();
        (baseAsValue <= baseNumber).ShouldBeTrue();
        (baseAsValue > baseNumber).ShouldBeFalse();
        (baseAsValue < baseNumber).ShouldBeFalse();

        //different value
        (baseNumber == largerThanBaseAsValue).ShouldBeFalse();
        (baseNumber != largerThanBaseAsValue).ShouldBeTrue();
        (baseNumber >= largerThanBaseAsValue).ShouldBeFalse();
        (baseNumber > largerThanBaseAsValue).ShouldBeFalse();
        (baseNumber <= largerThanBaseAsValue).ShouldBeTrue();
        (baseNumber < largerThanBaseAsValue).ShouldBeTrue();
        (largerThanBase == baseAsValue).ShouldBeFalse();
        (largerThanBase != baseAsValue).ShouldBeTrue();
        (largerThanBase >= baseAsValue).ShouldBeTrue();
        (largerThanBase > baseAsValue).ShouldBeTrue();
        (largerThanBase <= baseAsValue).ShouldBeFalse();
        (largerThanBase < baseAsValue).ShouldBeFalse();

        (baseAsValue == largerThanBase).ShouldBeFalse();
        (baseAsValue != largerThanBase).ShouldBeTrue();
        (baseAsValue >= largerThanBase).ShouldBeFalse();
        (baseAsValue > largerThanBase).ShouldBeFalse();
        (baseAsValue <= largerThanBase).ShouldBeTrue();
        (baseAsValue < largerThanBase).ShouldBeTrue();
        (largerThanBaseAsValue == baseNumber).ShouldBeFalse();
        (largerThanBaseAsValue != baseNumber).ShouldBeTrue();
        (largerThanBaseAsValue >= baseNumber).ShouldBeTrue();
        (largerThanBaseAsValue > baseNumber).ShouldBeTrue();
        (largerThanBaseAsValue <= baseNumber).ShouldBeFalse();
        (largerThanBaseAsValue < baseNumber).ShouldBeFalse();
    }

    [Test]
    public void ShouldCompareDefaultValue()
    {
        TrimmedDecimal? nullNum1 = new TrimmedDecimal();
        TrimmedDecimal? nullNum2 = new TrimmedDecimal();
        TrimmedDecimal? setNum = 5.234m;
        (nullNum1 == nullNum2).ShouldBeTrue();
        (nullNum1 != nullNum2).ShouldBeFalse();
        (nullNum1 > nullNum2).ShouldBeFalse();
        (nullNum1 < nullNum2).ShouldBeFalse();
        (nullNum1 >= nullNum2).ShouldBeTrue();
        (nullNum1 <= nullNum2).ShouldBeTrue();

        (nullNum2 == nullNum1).ShouldBeTrue();
        (nullNum2 != nullNum1).ShouldBeFalse();
        (nullNum2 > nullNum1).ShouldBeFalse();
        (nullNum2 < nullNum1).ShouldBeFalse();
        (nullNum2 >= nullNum1).ShouldBeTrue();
        (nullNum2 <= nullNum1).ShouldBeTrue();

        (nullNum1 == setNum).ShouldBeFalse();
        (nullNum1 != setNum).ShouldBeTrue();
        (nullNum1 > setNum).ShouldBeFalse();
        (nullNum1 < setNum).ShouldBeTrue();
        (nullNum1 >= setNum).ShouldBeFalse();
        (nullNum1 <= setNum).ShouldBeTrue();


        (setNum == nullNum1).ShouldBeFalse();
        (setNum != nullNum1).ShouldBeTrue();
        (setNum > nullNum1).ShouldBeTrue();
        (setNum < nullNum1).ShouldBeFalse();
        (setNum >= nullNum1).ShouldBeTrue();
        (setNum <= nullNum1).ShouldBeFalse();
    }

    [Test]
    public void ShouldHaveSameHashCodeForSameValue()
    {
        TrimmedDecimal baseNum = 6.123000m;
        TrimmedDecimal copyOfBase = 6.123m;
        TrimmedDecimal differentNum = 1.87m;
        baseNum.GetHashCode().ShouldBe(copyOfBase.GetHashCode());
        baseNum.GetHashCode().ShouldNotBe(differentNum.GetHashCode());
    }

    [Test]
    public void ShouldConvertToFloat()
    {
        float number = new TrimmedDecimal(123m);
        number.ShouldBe<float>(new TrimmedDecimal(123m));
    }

    [Test]
    public void ShouldConvertFromFloat()
    {
        TrimmedDecimal number = 123f;
        number.ShouldBe<TrimmedDecimal>(123f);
    }

    [Test]
    public void ShouldConvertToInt32()
    {
        int number = new TrimmedDecimal(123);
        number.ShouldBe<int>(new TrimmedDecimal(123));
    }

    [Test]
    public void ShouldConvertFromInt32()
    {
        TrimmedDecimal number = 123;
        number.ShouldBe<TrimmedDecimal>(123);
    }

    [Test]
    public void ShouldConvertToDecimal()
    {
        decimal number = new TrimmedDecimal(123m);
        number.ShouldBe<decimal>(new TrimmedDecimal(123m));
    }

    [Test]
    public void ShouldConvertFromDecimal()
    {
        TrimmedDecimal number = 123m;
        number.ShouldBe<TrimmedDecimal>(123m);
    }

    private void AssertDecimalIsShortened(TrimmedDecimal[] numbers)
    {
        foreach (var number in numbers)
        {
            var combinedMessage = number + " did not trim trailing 0's";
            number.ToString().Last().ShouldNotBe('0', combinedMessage);
        }
    }
}

public class UIComponent
{
    private readonly string _val;

    public UIComponent(decimal val)
    {
        _val = $"{val:0.######}";
    }

    public override string ToString()
    {
        return _val;
    }
}