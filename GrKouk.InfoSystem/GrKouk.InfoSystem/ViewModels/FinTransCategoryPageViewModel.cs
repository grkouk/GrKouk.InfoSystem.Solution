using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Services;
using Prism.Navigation;
using Prism.Services;

namespace GrKouk.InfoSystem.ViewModels
{
	public class FinTransCategoryPageViewModel : ViewModelBase
    {
        //private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IDataStore<FinTransCategory, FinTransCategory, FinTransCategory> _itemsDs;

        public FinTransCategoryPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
                                             IDataStore<FinTransCategory,FinTransCategory,FinTransCategory> itemsDs    
            ) :base(navigationService)
        {
            //_navigationService = navigationService;
            _dialogService = dialogService;
            _itemsDs = itemsDs;
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
        #region Save Command
        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(async () => await SaveDataCommand()));

        private async Task SaveDataCommand()
        {

            NavigationParameters navigationParams = new NavigationParameters();

            var newFinCategory = new FinTransCategory
            {
                Code = _code,
                Name = _name
                
            };
            try
            {
                var newItem = await _itemsDs.AddItemAsync2(newFinCategory);
                //navigationParams.Add("RefreshView", "True");
                navigationParams.Add("NewFinCategory", newItem);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await _dialogService.DisplayAlertAsync("Error", e.ToString(), "Ok");
                //throw;
            }

            await NavigationService.GoBackAsync(navigationParams);

        }
        #endregion
       
        #region Data Bind Properties
        private string _code;
        public string Code
        {
            get => _code;
            set => SetProperty(ref _code, value);
        }
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        
        #endregion

    }
}
