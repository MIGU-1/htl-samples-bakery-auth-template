using Bakery.Core.DTOs;
using Bakery.Persistence;
using Bakery.Wpf.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bakery.Wpf.ViewModels
{
  public class MainWindowViewModel : BaseViewModel
  {
    private IEnumerable<ProductDto> _products;

    public ObservableCollection<ProductDto> Products { get; }


    private ProductDto _selectedProduct;

    public ProductDto SelectedProduct
    {
      get => _selectedProduct;
      set
      {
        _selectedProduct = value;
        OnPropertyChanged();
      }
    }

    /// <summary>
    /// Durchschnittspreis aller angezeigten Produkte
    /// </summary>
    private string _avgPrice;

    public string AvgPrice
    {
      get => _avgPrice;
      set
      {
        _avgPrice = value;
        OnPropertyChanged();
      }
    }

    /// <summary>
    /// Preisfilter "von"
    /// </summary>
    private string _filterPriceFrom;

    public string FilterPriceFrom
    {
      get => _filterPriceFrom;
      set
      {
        _filterPriceFrom = value;
        OnPropertyChanged();
      }
    }

    /// <summary>
    /// Preisfilter "bis"
    /// </summary>
    private string _filterPriceTo;

    public string FilterPriceTo
    {
      get => _filterPriceTo;
      set
      {
        _filterPriceTo = value;
        OnPropertyChanged();
      }
    }

    public ICommand CmdFilterProducts { get; set; }
    public ICommand CmdNewProduct { get; set; }
    public ICommand CmdEditProduct { get; set; }

    public MainWindowViewModel(IWindowController controller) : base(controller)
    {
      Products = new ObservableCollection<ProductDto>();
      LoadCommands();
    }

    private void LoadCommands()
    {
      CmdFilterProducts = new RelayCommand(
          c => FilterProducts(),
          c => AllowFilter());
      CmdEditProduct = new RelayCommand(
          async c => await EditProduct(),
          c => SelectedProduct != null);
      CmdNewProduct = new RelayCommand(
          async c => await CreateProduct(),
          c => true);


    }

    private async Task CreateProduct()
    {
      var model = new EditProductViewModel(Controller, null);
      Controller.ShowWindow(model, true);
      await LoadProducts();
    }

    private async Task EditProduct()
    {
      var model = new EditProductViewModel(Controller, SelectedProduct);
      Controller.ShowWindow(model, true);
      await LoadProducts();
    }


    private bool AllowFilter()
    {
      if (string.IsNullOrEmpty(FilterPriceFrom) || string.IsNullOrEmpty(FilterPriceTo))
      {
        return false;
      }

      try
      {
        var from = 0.0;
        if (!string.IsNullOrEmpty(FilterPriceFrom))
          from = double.Parse(FilterPriceFrom);
        var to = double.PositiveInfinity;
        if (!string.IsNullOrEmpty(FilterPriceTo))
          to = double.Parse(FilterPriceTo);
        return from < to;
      }
      catch (Exception)
      {
        return false;
      }

    }

    private void FilterProducts()
    {
      var from = double.Parse(FilterPriceFrom);
      var to = double.Parse(FilterPriceTo);
      var productsFiltered = _products.Where(p => p.Price >= from && p.Price <= to).ToList();
      Products.Clear();
      productsFiltered.ForEach(Products.Add);
      AvgPrice = $"{Products.Select(x => (double?)x.Price).Average() ?? 0:f2}";
    }

    public static async Task<MainWindowViewModel> Create(IWindowController controller)
    {
      var model = new MainWindowViewModel(controller);
      await model.LoadProducts();
      return model;
    }

    /// <summary>
    /// Produkte laden. Produkte können nach Preis gefiltert werden. 
    /// Preis liegt zwischen from und to
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    private async Task LoadProducts()
    {
      await using var uow = new UnitOfWork();
      _products = await uow.Products.GetAllAsync();
      Products.Clear();
      _products.ToList().ForEach(Products.Add);

      AvgPrice = $"{(Products.Select(x => (double?)x.Price).Average() ?? 0):f2}";

    }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      throw new NotImplementedException();
    }
  }
}
