using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DataAccess;
using SupplyStock;

namespace BackeryApp.ViewModel
{
    public class IndexProductionsVM
    {
        #region Constructor

        public IndexProductionsVM(MContext context)
        {
            Context = context;
            Productions = new ObservableCollection<ProductionVM>();
            SearchPattern = "";
            FilterProductions();
            InitIndexTemplates();
        }

        #endregion
        #region Properties

        public ObservableCollection<ProductionVM> Productions { get; set; }
        public MContext Context { get; set; }
        public string SearchPattern { get; set; }

        #endregion
        #region Methods

        private void InitIndexTemplates()
        {
            Context.LocalProductions.CollectionChanged += (sender, args) =>
            {
                if (args.NewItems != null)
                {
                    foreach (var newItem in args.NewItems)
                        if (PassTheFilter((Production)newItem))
                            Productions.Add(new ProductionVM(Context, (Production)newItem));
                }
                if (args.OldItems != null)
                {
                    FilterProductions();
                }
            };
        }

        private bool PassTheFilter(Production production)
        {
            return production.Name.ToLower().Contains(SearchPattern.ToLower());
        }

        public void FilterProductions()
        {
            Productions.Clear();
            foreach (var prod in Context.LocalProductions.Where(PassTheFilter))
                Productions.Add(new ProductionVM(Context, prod));
        }

        #endregion

        public void RemoveProduction(ProductionVM productionVM)
        {
            if (productionVM == null)
            {
                MessageBox.Show("Es necesario tener seleccionado una producción antes de eliminar.", "Error eliminando",
                               MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            Context.RemoveProduction(productionVM.Production.ProductionId);
        }
    }
}
