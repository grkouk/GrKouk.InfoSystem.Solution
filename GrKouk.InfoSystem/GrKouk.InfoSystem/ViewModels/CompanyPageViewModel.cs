﻿using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Services;
using Prism.Navigation;
using Prism.Services;

namespace GrKouk.InfoSystem.ViewModels
{
	public class CompanyPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _dialogService;
        private readonly IDataStore<Company, Company, Company> _itemDs;

        public CompanyPageViewModel(INavigationService navigationService
            , IPageDialogService dialogService
            , IDataStore<Company, Company, Company> itemDs
        ) : base(navigationService)
        {
            _dialogService = dialogService;
            _itemDs = itemDs;
            Title = "Εισαγωγή Εταιρείας";
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

            var newEntity = new Company
            {
                Name = _name,
                Code = _code
            };
            try
            {
                var newItem = await _itemDs.AddItemAsync2(newEntity);
                navigationParams.Add("RefreshView", "True");
                navigationParams.Add("NewCompany", newItem);
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
                if (parameters.ContainsKey("CompanyName"))
                {
                    Name = parameters["CompanyName"].ToString();
                }
            }
        }

        #endregion
    }
}
