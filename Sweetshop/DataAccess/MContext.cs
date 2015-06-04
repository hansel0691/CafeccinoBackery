using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using DataAccess.SchemaConfiguration;
using SupplyStock;
using SupplyStock.Utils;

namespace DataAccess
{
    public class MContext : DbContext
    {
        #region Contructor

        public MContext()
        {
            Supplies.Load();
            CostTemplates.Load();
            Productions.Load();
            LocalSupplies = Supplies.Local;
            LocalTemplates = CostTemplates.Local;
            LocalProductions = Productions.Local;
        }

        #endregion
        #region Properties

        public DbSet<Supply> Supplies { get; set; }
        public DbSet<CostTemplate> CostTemplates { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<SupplyAmount> SupplyAmounts { get; set; }
        public DbSet<Production> Productions { get; set; }
        public DbSet<TemplateAmount> TemplateAmounts { get; set; }

        public ObservableCollection<CostTemplate> LocalTemplates { get; set; }
        public ObservableCollection<Supply> LocalSupplies { get; set; }
        public ObservableCollection<Production> LocalProductions { get; set; }

        #endregion
        #region Methods

        public Supply GetSupplyFromDb(int supplyId)
        {
            return Supplies.FirstOrDefault(s => s.SupplyId == supplyId);
            
        }
        public Supply GetSupply(Supply supply)
        {
            if (supply.SupplyId != 0)
                return LocalSupplies.FirstOrDefault(s => s.SupplyId == supply.SupplyId);
            return LocalSupplies.FirstOrDefault(s => s.ToString() == supply.ToString());
        }
        public void AddSupply(Supply supply)
        {
            var supp = GetSupply(supply);
            if (supp != null) return; //there is a supply with the same name and format in the db.

            supply.Name = supply.Name.Trim(' ');
            LocalSupplies.Add(supply);
            Supplies.Add(supply);
            SaveChanges();
        }
        public List<string> RemoveSupply(Supply supplyInfo, bool force = false)
        {
            var toDelete = GetSupply(supplyInfo);
            var affected = new List<string>(RemoveAffected(supplyInfo, force));

            if (affected.Count == 0 || force)
            {
                var removed = Supplies.Remove(toDelete);
                if (removed == null) throw new Exception("An error ocurred while removing the supply from the database.");
            }
            else
                return affected;
            SaveChanges();

            foreach (var template in LocalTemplates)
                template.ItemChange("Cost");
           
            return new List<string>();
        }
        public void UpdateSupply(Supply newSupply)
        {
            SaveChanges();
            UpdateRelatedFromSupply(newSupply);
        }

        private void UpdateRelatedFromSupply(Supply supply)
        {
            var supp = GetSupplyFromDb(supply.SupplyId);
            if (supp.SupplyAmounts == null)return;
            foreach (var supplyAmount in supp.SupplyAmounts)
            {
                supplyAmount.Template = GetTemplateFromDb(supplyAmount.Template.CostTemplateId);
                supplyAmount.Template.ItemChange("Cost");
                UpdateRelatedFromSupply(supplyAmount.Template.UnderlyingSupply);
            }
        }

        private CostTemplate GetTemplateFromDb(int templateId)
        {
            return CostTemplates.FirstOrDefault(t => t.CostTemplateId == templateId);
        }

        public CostTemplate GetTemplate(CostTemplate template)
        {
            if (template.CostTemplateId != 0)
                return LocalTemplates.FirstOrDefault(t => t.CostTemplateId == template.CostTemplateId);
            return LocalTemplates.FirstOrDefault(t => t.ToString() == template.ToString());
        }
        public void AddTemplate(CostTemplate newTemp)
        {
            var temp = GetTemplate(newTemp);
            if (temp != null) return; //there is a template with the same format in the db.

            newTemp.Name = newTemp.Name.Trim();
            LocalTemplates.Add(newTemp);
            CostTemplates.Add(newTemp);
            SaveChanges();
        }
        public List<string> RemoveTemplate(CostTemplate tempInfo, bool force = false)
        {
            var result = new List<string>();
            var toDelete = GetTemplate(tempInfo);
            var underlyingSupply = toDelete.UnderlyingSupply;
            result = RemoveSupply(underlyingSupply, force);
            
            return result;
        }
        public void UpdateTemplate(CostTemplate newTemp)
        {
            SaveChanges();
            UpdateRelatedFromTemplate(newTemp);
        }
        public void AddSupplyToTemplate(CostTemplate newTemp, List<SupplyAmount> supplyAmount)
        {
            if (supplyAmount.Count == 0) return;
            var toUpdate = GetTemplate(newTemp);
            foreach (var sa in supplyAmount)
            {
                var supply = GetSupply(sa.Supply);
                if (supply == null) throw new Exception("One of the supplies you are trying to add is not in the database.");
                if (toUpdate.SupplyAmounts == null) toUpdate.SupplyAmounts = new List<SupplyAmount>();
                toUpdate.SupplyAmounts.Add(new SupplyAmount(supply, sa.StartAmount, sa.EndAmount, sa.NonDefaultMeasuremnt));
                SaveChanges();
            }
        }


        private void UpdateRelatedFromTemplate(CostTemplate template)
        {
            var temp = GetTemplateFromDb(template.CostTemplateId);
            temp.ItemChange("Cost");
            if (temp.UnderlyingSupply.SupplyAmounts == null) return;
            foreach (var supplyAmount in temp.UnderlyingSupply.SupplyAmounts)
            {
                supplyAmount.Template.ItemChange("Cost");
                UpdateRelatedFromTemplate(supplyAmount.Template);
            }
        }
        public void RemoveSupplyAmount(int supplyAmountId)
        {
            SupplyAmounts.Remove(SupplyAmounts.Find(supplyAmountId));    
            SaveChanges();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SupplyConfiguration());
            modelBuilder.Configurations.Add(new CostTemplateConfiguration());
            modelBuilder.Configurations.Add(new MeasurementConfiguration());
            modelBuilder.Configurations.Add(new SupplyAmountConfiguration());
            modelBuilder.Configurations.Add(new ProductionConfiguration());
            modelBuilder.Configurations.Add(new TemplateAmountConfiguration());
        }
        private IEnumerable<string> RemoveAffected(Supply supplyInfo, bool force)
        {
            var toRemoveIds = new List<int>();
            foreach (var template in CostTemplates)
            {
                if (template.SupplyAmounts == null) yield break;
                foreach (var supplyAmount in template.SupplyAmounts)
                    if (supplyAmount.Supply.ToString() == supplyInfo.ToString())
                    {
                        if (force)
                            toRemoveIds.Add(supplyAmount.SupplyAmountId);
                        else
                            yield return template.ToString();
                    }
            }
            foreach (var removeId in toRemoveIds)
                SupplyAmounts.Remove(SupplyAmounts.Find(removeId));
            SaveChanges();
        }


        public Production GetProductionFromDb(int productionId)
        {
            return Productions.FirstOrDefault(p => p.ProductionId == productionId);

        }
        public Production GetProduction(Production production)
        {
            if (production.ProductionId != 0)
                return LocalProductions.FirstOrDefault(p => p.ProductionId == production.ProductionId);
            return LocalProductions.FirstOrDefault(p => p.ToString() == production.ToString());
        }
        public void AddProduction(Production production)
        {
            var prod = GetProduction(production);
            if (prod != null) return;

            production.Name = production.Name.Trim(' ');
            LocalProductions.Add(production);
            Productions.Add(production);
            SaveChanges();
        }
        private void AddTemplateToProduction(Production production, IEnumerable<TemplateAmount> templateAmounts)
        {
            if (!templateAmounts.Any()) return;
            var toUpdate = GetProduction(production);
            foreach (var ta in templateAmounts)
            {
                var template = GetTemplate(ta.Template);
                if (template == null) throw new Exception("One of the templates you are trying to add is not in the database.");
                if (toUpdate.TemplateAmounts == null) toUpdate.TemplateAmounts = new List<TemplateAmount>();
                toUpdate.TemplateAmounts.Add(new TemplateAmount(template, ta.Amount));
            }
        }
        public void UpdateProduction()
        {
            SaveChanges();
        }
        public void RemoveProduction(int productionId)
        {
            var toDelete = GetProduction(new Production { ProductionId = productionId });
            var removed = Productions.Remove(toDelete);
            if (removed == null) throw new Exception("An error ocurred while removing the production from the database.");
            SaveChanges();
        }
        public void RemoveTemplateAmount(int id)
        {
            TemplateAmounts.Remove(TemplateAmounts.Find(id));
            SaveChanges();
        }

        #endregion

    }
}
