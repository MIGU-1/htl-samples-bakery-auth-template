using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Input;
using Bakery.Core.DTOs;
using Bakery.Core.Entities;
using Bakery.Persistence;
using Bakery.Wpf.Common;

namespace Bakery.Wpf.ViewModels
{
  public class EditProductViewModel : BaseViewModel
    {
        private ProductDto _originalProduct;

        private string _titleString;

        public string TitleString
        {
            get => _titleString;
            set
            {
                _titleString = value;
                OnPropertyChanged();
            }
        }

        private string _editProductNr;

        [Required(ErrorMessage = "Produktnummer muss angegeben werden")]
        public string EditProductNr
        {
            get => _editProductNr;
            set
            {
                _editProductNr = value;
                OnPropertyChanged();
                ValidateViewModelProperties();
            }
        }

        private string _editName;

        [Required(ErrorMessage = "Produktname muss angegeben werden")]
        [MinLength(1, ErrorMessage = "Produktname muss mindestens 1 Zeichen lang sein")]
        [MaxLength(20, ErrorMessage = "Produktname darf maximal 20 Zeichen lang sein")]
        public string EditName
        {
            get => _editName;
            set
            {
                _editName = value;
                OnPropertyChanged();
                ValidateViewModelProperties();
            }
        }

        private double _editPrice;

        public double EditPrice
        {
            get => _editPrice;
            set
            {
                _editPrice = value;
                OnPropertyChanged();
            }
        }

        public ICommand CmdSave { get; set; }

        public ICommand CmdUndo{ get; set; }

        public EditProductViewModel(IWindowController controller, ProductDto product) : base(controller)
        {
            
            if (product != null)
            {
                _originalProduct = product;
                EditName = _originalProduct.Name;
                EditProductNr = _originalProduct.ProductNr;
                EditPrice = _originalProduct.Price;
                TitleString = "Produkt bearbeiten";
            }
            else
            {
                TitleString = "Produkt anlegen";
            }

            LoadCommands();
        }

        private void LoadCommands()
        {
            CmdSave = new RelayCommand(
                async c => await SaveProduct(),
                c => _originalProduct == null || (HasChanged() && IsValid));
            CmdUndo = new RelayCommand(
                c => UndoChanges(),
                c => _originalProduct != null && HasChanged());
        }

        private void UndoChanges()
        {
            EditName = _originalProduct.Name;
            EditProductNr = _originalProduct.ProductNr;
            EditPrice = _originalProduct.Price;
        }

        private bool HasChanged()
        {

            return EditName != _originalProduct.Name ||
                   EditPrice != _originalProduct.Price ||
                   EditProductNr != _originalProduct.ProductNr;
        }

        private async Task SaveProduct()
        {
            await using var uow = new UnitOfWork();
            Product dbProduct = null;
            if (_originalProduct == null)
            {
                dbProduct = new Product
                {
                    Name = EditName,
                    ProductNr = EditProductNr,
                    Price = EditPrice
                };
                await uow.Products.AddAsync(dbProduct);

            }
            else
            {
                dbProduct = await uow.Products.GetAsync(_originalProduct.Id);
                if (dbProduct == null)
                {
                    DbError = "Produkt existiert nicht";
                    return;
                }
                dbProduct.Name = EditName;
                dbProduct.Price = EditPrice;
                dbProduct.ProductNr = EditProductNr;
            }
            
            try
            {
                await uow.SaveChangesAsync();
                _originalProduct = new ProductDto(dbProduct);
                Controller.CloseWindow(this);
            }
            catch (ValidationException e)
            {
                DbError = e.Message;
            }

        }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ValidationResult.Success;
        }
    }
}
