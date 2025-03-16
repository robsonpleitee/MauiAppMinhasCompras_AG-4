using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views
{
    public partial class ListaProduto : ContentPage
    {
        ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
        private Produto _itemSelecionado;

        public ListaProduto()
        {
            InitializeComponent();
            lst_produtos.ItemsSource = lista;
            lst_produtos.ItemSelected += Lst_produtos_ItemSelected;
        }

        private void Lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _itemSelecionado = e.SelectedItem as Produto;
        }

        protected async override void OnAppearing()
        {
            await CarregarLista();
        }

        private async Task CarregarLista()
        {
            lista.Clear();
            var produtos = await App.Db.GetAll();
            foreach (var produto in produtos)
            {
                lista.Add(produto);
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new Views.NovoProduto());
            }
            catch (Exception ex)
            {
                DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string q = e.NewTextValue;
            lista.Clear();
            List<Produto> tmp = await App.Db.Search(q);
            tmp.ForEach(i => lista.Add(i));
        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            double soma = lista.Sum(i => i.Total);
            string msg = $"O total é {soma:C}";
            DisplayAlert("Total dos Produtos", msg, "OK");
        }

        private async void ToolbarItem_Clicked_2(object sender, EventArgs e)
        {
            try
            {
                if (_itemSelecionado != null)
                {
                    await App.Db.Delete(_itemSelecionado.Id);
                    lista.Remove(_itemSelecionado);
                    _itemSelecionado = null;
                }
                else
                {
                    await DisplayAlert("Aviso", "Nenhum item selecionado para remover.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Ocorreu um erro ao remover o item: {ex.Message}", "OK");
            }
        }

        private async void MenuItem_Clicked2(object sender, EventArgs e)
        {
            try
            {
                var menuItem = sender as MenuItem;
                var produto = menuItem.CommandParameter as Produto;
                if (produto != null)
                {
                    await App.Db.Delete(produto.Id);
                    lista.Remove(produto);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Ocorreu um erro ao remover o item: {ex.Message}", "OK");
            }
        }

     
    }
}