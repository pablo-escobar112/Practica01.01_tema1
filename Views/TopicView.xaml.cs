using System.Windows.Controls;

namespace InventoryWpfApp
{
    public partial class TopicView : UserControl
    {
        public TopicView()
        {
            InitializeComponent();

            // Подписываемся на изменение DataContext
            DataContextChanged += TopicView_DataContextChanged;
        }

        private void TopicView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            // Отписываемся от старого VM
            if (e.OldValue is TopicViewModel oldVm)
            {
                oldVm.PropertyChanged -= Vm_PropertyChanged;
            }

            // Подписываемся на новый VM
            if (e.NewValue is TopicViewModel newVm)
            {
                newVm.PropertyChanged += Vm_PropertyChanged;
                UpdateDocument(newVm);
            }
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TopicViewModel.Content))
            {
                UpdateDocument(sender as TopicViewModel);
            }
        }

        private void UpdateDocument(TopicViewModel vm)
        {
            if (vm?.Content != null)
            {
                ContentBox.Document = vm.Content;
            }
        }
    }
}