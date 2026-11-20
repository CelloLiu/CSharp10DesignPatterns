using AbstractFactory;

// the same ShoppingCart code works for any country: only the factory differs
var shoppingCartForBelgium = new ShoppingCart(new BelgiumShoppingCartPurchaseFactory());
shoppingCartForBelgium.CalculateCosts();

var shoppingCartForFrance = new ShoppingCart(new FranceShoppingCartPurchaseFactory());
shoppingCartForFrance.CalculateCosts();
