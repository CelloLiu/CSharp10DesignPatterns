using Strategy;

var order = new Order("Marvin Software", 5, "Visual Studio License");

// same order, different export strategies
order.Export(new CSVExportService());
order.Export(new JsonExportService());
order.Export(new XMLExportService());
