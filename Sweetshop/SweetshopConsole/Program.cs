using System;
using System.Data.Entity;
using System.Linq;
using DataAccess;
using SupplyStock;
using SupplyStock.Utils;

namespace SweetshopConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(
                new DropCreateDatabaseIfModelChanges<MContext>());

            var sup1 = new Supply("Azúcar", "", 24, MeasurementUnit.Kilogram, 24, CurrencyUnit.CUC);
            var sup2 = new Supply("Sal", "buena", 20, MeasurementUnit.Kilogram, 20, CurrencyUnit.CUC);
            var sup3 = new Supply("Egg", "", 48, MeasurementUnit.Unit, 48, CurrencyUnit.CUP);
            var sup4 = new Supply("Chocolate", "black", 50, MeasurementUnit.Kilogram, 150, CurrencyUnit.CUC);

            var temp1 = new CostTemplate("Panetela");
            var temp2 = new CostTemplate("Cake");

            using (var context = new MContext())
            {
                //Console.WriteLine("Inserting Supplies...");
                //context.AddSupply(sup1);
                //context.AddSupply(sup2);
                //context.AddSupply(sup3);
                //context.AddSupply(sup4);


                //Console.WriteLine("Adding templates...");
                //context.AddTemplate(temp1);
                //context.AddTemplate(temp2);


                //var supply = context.GetSupplyFromDb(sup1);
                //var supply = context.GetSupplyFromDb(new Supply(){Name = "Cake", FormatAmount = new Measurement(){Unit = MeasurementUnit.Unit}});
                //var supply = context.GetSupplyFromDb(new Supply() { Name = "Cake", FormatAmount = new Measurement() { Unit = MeasurementUnit.Unit } });

                //Console.WriteLine("Updating supply");
                //supply.Name = "Cake Imperial";
                //supply.Description = "Con glaceado.";
                ////supply.FormatCost = new Currency(1000, CurrencyUnit.CUP);
                //supply.FormatAmount = new Measurement(1000, MeasurementUnit.Unit);
                //context.UpdateSupply();


                //var template = context.GetTemplateFromDb(temp1);
                //var template = context.CostTemplates.Find(9);

                //context.RemoveTemplate(template);
                //context.RemoveSupply(supply, true).ForEach(Console.WriteLine);

                //Console.WriteLine("modifing the template");
                //template.Name = "Panetela Griega";
                //template.Description = "Otra descripcion";
                //template.SellingPrice = new Currency(5, CurrencyUnit.CUP);
                //template.ProducedUnits = 805;
                //context.UpdateTemplate();

                //context.AddSupplyToTemplate(template, new List<SupplyAmount> { new SupplyAmount(sup1, new Measurement(10, MeasurementUnit.Gram)), new SupplyAmount(sup4, new Measurement(5, MeasurementUnit.Kilogram)) });

                ////Print(supply);
                //Print(template);



                PrintSupplies(context);
                PrintTemplates(context);


                //CleanDB(context);
            }   
        }

        public static void PrintSupplies(MContext context)
        {
            Console.WriteLine("Supplies in db: ");
            var supplies = context.Supplies.ToList();
            if (supplies.Count == 0) Console.WriteLine("No supplies");
            supplies.Select(s => s.ToString() + "\n").ToList().ForEach(Console.Write);
            Console.WriteLine();
            Console.WriteLine("==================================");
        }

        public static void Print(Supply su)
        {
            using (var context = new MContext())
            {
                Console.WriteLine("==================================");
                Console.WriteLine("Supply Id: {0}", su.SupplyId);
                Console.WriteLine("Supply Name: {0}", su.Name);
                Console.WriteLine("Supply Description: {0}", su.Description);
                Console.WriteLine("Supply Format Amount: {0}", su.FormatAmount);
                Console.WriteLine("Supply Format Cost: {0}", su.FormatCost);
                Console.WriteLine("Supply Cost per Unit: {0}", su.CostPerUnit());
                Console.WriteLine("==================================");
            }
        }

        public static void PrintTemplates(MContext context)
        {
            Console.WriteLine("Templates in db: ");
            var templates = context.CostTemplates.ToList();
            if (templates.Count == 0) Console.WriteLine("No templpates");
            templates.Select(s => s.ToString() + "\n").ToList().ForEach(Console.Write);
            Console.WriteLine();
            Console.WriteLine("==================================");
        }

        public static void Print(CostTemplate co)
        {
            using (var context = new MContext())
            {
                Console.WriteLine("==================================");
                Console.WriteLine("Cost Template Id: {0}", co.CostTemplateId);
                Console.WriteLine("Cost Template Name: {0}", co.Name);
                Console.WriteLine("Cost Template Description: {0}", co.Description);
                Console.WriteLine("Cost Template Coust: {0}", co.Cost);
                Console.WriteLine("Cost Template Produced units: {0}", co.ProducedUnits);
                Console.WriteLine("Cost Template Price: {0}", co.SellingPrice);
                Console.WriteLine("Cost Template's Supplies: ");
                Console.WriteLine("Count of supplies: {0}: ", co.SupplyAmounts.Count);
                Console.WriteLine("Used Supplies");
                co.SupplyAmounts.Select(s => "\t" + s.Supply.Name + "\n").ToList().ForEach(Console.Write);
                Console.WriteLine();
                Console.WriteLine("==================================");
            }
        }
        public static void CleanDB(MContext context)
        {
            foreach (var temp in context.CostTemplates)
            {
                context.CostTemplates.Remove(temp);
            }
            foreach (var supp in context.Supplies)
            {
                context.Supplies.Remove(supp);
            }
            foreach (var measurement in context.Measurements)
            {
                context.Measurements.Remove(measurement);
            }
            context.SaveChanges();
            foreach (var supply in context.Supplies)
            {
                Console.WriteLine(supply.FormatAmount);
            }
        }
    }
}
