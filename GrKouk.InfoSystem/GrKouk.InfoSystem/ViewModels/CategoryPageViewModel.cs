using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Services;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;

namespace GrKouk.InfoSystem.ViewModels
{
	public class CategoryPageViewModel : ViewModelBase
    {

        private readonly IPageDialogService _dialogService;
        private readonly IDataStore<FinTransCategory, FinTransCategory, FinTransCategory> _itemDs;

        public CategoryPageViewModel(INavigationService navigationService
            , IPageDialogService dialogService
            , IDataStore<FinTransCategory, FinTransCategory, FinTransCategory> itemDs
        ) : base(navigationService)
        {
            _dialogService = dialogService;
            _itemDs = itemDs;
            Title = "Εισαγωγή Κατηγορίας";
        }
        #region IsBusy

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

        #region Bindable Properties

        private string _code;

        public string Code
        {
            get { return _code; }
            set { SetProperty(ref _code, value); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        #endregion
        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(async () => await SaveDataCmd()));

        private async Task SaveDataCmd()
        {
            NavigationParameters navigationParams = new NavigationParameters();

            var newCategory = new FinTransCategory
            {
                Name = _name,
                Code = _code
            };
            try
            {
                var newItem = await _itemDs.AddItemAsync2(newCategory);
                navigationParams.Add("RefreshView", "True");
                navigationParams.Add("NewCategory", newItem);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _dialogService.DisplayAlertAsync("Error", e.ToString(), "Ok");
                //throw;
            }
            await NavigationService.GoBackAsync(navigationParams);
        }
        #region SaveCommand

        #endregion
        #region OnNavigatedTo

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                if (parameters.ContainsKey("CategoryName"))
                {
                    Name = parameters["CategoryName"].ToString();
                }
            }
        }

        #endregion

    }
}
