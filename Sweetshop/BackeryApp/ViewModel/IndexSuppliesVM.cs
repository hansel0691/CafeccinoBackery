using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using DataAccess;
using SupplyStock;

namespace BackeryApp.ViewModel
{
    public class IndexSuppliesVM
    {
        #region Constructor

        public IndexSuppliesVM(MContext context)
        {
            Context = context;
            Supplies = new ObservableCollection<SupplyVM>();
            SearchPattern = "Filtrar Insumo...";
            SelectedOption = 0;

            FilterSupplies();
            InitIndexTemplates();
        }

        #endregion
        #region Property

        public MContext Context { get; set; }
        public string SearchPattern { get; set; }
        public int SelectedOption { get; set; }
        public ObservableCollection<SupplyVM> Supplies { get; private set; }

        #endregion
        #region Methods

        private void InitIndexTemplates()
        {
            Context.LocalSupplies.CollectionChanged += (sender, args) =>
            {
                if (args.NewItems != null)
                {
                    foreach (var newItem in args.NewItems)
                        if (PassTheFilter((Supply)newItem))
                            Supplies.Add(new SupplyVM(Context, (Supply)newItem));
                }
                if (args.OldItems != null)
                {
                    FilterSupplies();
                }
            };
        }
        public void FilterSupplies()
        {
            Supplies.Clear();
            foreach (var supply in Context.LocalSupplies.Where(PassTheFilter))
                Supplies.Add(new SupplyVM(Context, supply));
        }
        private bool PassTheFilter(Supply supply)
        {
            var text = SearchPattern == "Filtrar Insumo..." ? "" : SearchPattern.ToLower();
            return supply.Name.ToLower().Contains(text) &&
                   ((SelectedOption == 0 && (supply.Template == null || !supply.Template.FinishedTemplate)) ||
                    (SelectedOption == 1 && !supply.IsTemplate) ||
                    (SelectedOption == 2 && supply.IsTemplate && !supply.Template.FinishedTemplate));
        }
        public void Sort(string header, ListSortDirection direction)
        {
            Comparison<SupplyVM> comparer = null;

            switch (header)
            {
                case "Nombre":
                    comparer = (x, y) => x.Name.CompareTo(y.Name);
                    break;
                case "Descripción":
                    comparer = (x, y) => x.Description == null ? 1 : y.Description == null ? -1 : x.Description.CompareTo(y.Description);
                    break;
                case "Cantidad del Formato":
                    comparer = (x, y) => x.Amount.AmountInUnit.CompareTo(y.Amount.AmountInUnit);
                    break;
                case "Costo del Formato":
                    comparer = (x, y) => x.Cost.ToCUC().CompareTo(y.Cost.ToCUC());
                    break;
            }

            if (comparer == null) return;
            var items = new List<SupplyVM>(Supplies);
            Supplies.Clear();
            items.Sort(comparer);
            if (direction == ListSortDirection.Descending)
                items.Reverse();
            items.ForEach(s => Supplies.Add(s));

        }

        #endregion

        public void RemoveSupply(SupplyVM supply)
        {
            var supp = supply.Supply;
            if (supp == null)
            {
                MessageBox.Show("Es necesario tener seleccionada un insumo antes de eliminar.", "Error eliminando",
                               MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            var affecedTemplates = Context.RemoveSupply(supp);
            if (affecedTemplates.Count > 0)
            {
                var message = affecedTemplates.Aggregate("Las siguientes fichas de costo serán modificadas tras la eliminación de este insumo. Desea continuar con la eliminación?\n\n", (current, affecedTemplate) => current + ("\t" + affecedTemplate + "\n"));

                var answer = MessageBox.Show(message, "Eliminación de insumos", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (answer == MessageBoxResult.OK)
                    Context.RemoveSupply(supp, true);
            }
        }
    }
}
