using State;

var order = new Order();

// The same methods behave differently depending on the order's current state
order.Ship(); // Pending: blocked
order.Pay();  // Pending -> Paid
order.Pay();  // Paid: already paid
order.Ship(); // Paid -> Shipped
order.Ship(); // Shipped: already shipped
