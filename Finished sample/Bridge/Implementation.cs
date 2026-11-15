// Bridge: split one concept into two independent parts (here: menus and coupons)
// so each can vary on its own, and any menu works with any coupon.
namespace Bridge;

/// <summary>Abstraction</summary>
public abstract class Menu(ICoupon coupon)
{
    // the "bridge" to the other part
    protected ICoupon Coupon { get; } = coupon;

    public abstract int CalculatePrice();
}

/// <summary>RefinedAbstraction</summary>
public class VegetarianMenu(ICoupon coupon) : Menu(coupon)
{
    public override int CalculatePrice() => 20 - Coupon.CouponValue;
}

/// <summary>RefinedAbstraction</summary>
public class MeatBasedMenu(ICoupon coupon) : Menu(coupon)
{
    public override int CalculatePrice() => 30 - Coupon.CouponValue;
}

/// <summary>Implementor</summary>
public interface ICoupon
{
    int CouponValue { get; }
}

/// <summary>ConcreteImplementor</summary>
public class NoCoupon : ICoupon
{
    public int CouponValue => 0;
}

/// <summary>ConcreteImplementor</summary>
public class OneEuroCoupon : ICoupon
{
    public int CouponValue => 1;
}

/// <summary>ConcreteImplementor</summary>
public class TwoEuroCoupon : ICoupon
{
    public int CouponValue => 2;
}
