using System;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.InfoSystem.Dtos;
using GrKouk.InfoSystem.Services;
using GrKouk.InfoSystem.ViewModels;
using GrKouk.InfoSystem.Views;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Logging;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GrKouk.InfoSystem
{
    public partial class App : PrismApplication
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
            containerRegistry.Register<IDataStore<Transactor, Transactor, Transactor>, TransactorsDataStore>();
            containerRegistry.Register<IDataStore<FinDiaryTransactionDto, FinDiaryTransactionCreateDto, FinDiaryTransactionDto>, TransactionsDataStore>();
            containerRegistry.Register<IDataStore<FinTransCategory, FinTransCategory, FinTransCategory>, FinTransCategoryDataSource>();
            containerRegistry.Register<IDataStore<Company, Company, Company>, CompanyDataStore>();
            containerRegistry.Register<IDataStore<CostCentre, CostCentre, CostCentre>, CostCentreDataStore>();
            containerRegistry.Register<IAutoCompleteDataSource<Transactor>, TransactorsAutoCompleteDs>();
            containerRegistry.Register<IAutoCompleteDataSource<FinTransCategory>, CategoryAutoCompleteDs>();
            containerRegistry.Register<IAutoCompleteDataSource<Company>, CompanyAutoCompleteDs>();
            containerRegistry.Register<IAutoCompleteDataSource<CostCentre>, CostCentreAutoCompleteDs>();
            containerRegistry.Register<IAutoCompleteDataSource<RevenueCentre>, RevenueCentreAutoCompleteDs>();
            containerRegistry.Register<ITransactionDataStore<FinDiaryTransactionDto, FinDiaryTransactionCreateDto, FinDiaryTransactionModifyDto>, TransactionsDataStoreEx>();


            containerRegistry.RegisterForNavigation<MenuPage, MenuPageViewModel>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<FinTransCategoryPage, FinTransCategoryPageViewModel>();
            containerRegistry.RegisterForNavigation<FinDiaryTransExpencePage, FinDiaryTransExpencePageViewModel>();
           
            containerRegistry.RegisterForNavigation<CategorySearchListPage, CategorySearchListPageViewModel>();
            containerRegistry.RegisterForNavigation<FinDiaryTransExpencesPage, FinDiaryTransExpencesPageViewModel>();
            containerRegistry.RegisterForNavigation<CompaniesSearchListPage, CompaniesSearchListPageViewModel>();
            containerRegistry.RegisterForNavigation<CostCentresSearchListPage, CostCentresSearchListPageViewModel>();
            containerRegistry.RegisterForNavigation<TransactorsSearchListPage, TransactorsSearchListPageViewModel>();
            containerRegistry.RegisterForNavigation<TransactionsPage, TransactionsPageViewModel>();
            containerRegistry.RegisterForNavigation<TransactorPage, TransactorPageViewModel>();
            containerRegistry.RegisterForNavigation<CategoryPage, CategoryPageViewModel>();
            containerRegistry.RegisterForNavigation<CompanyPage, CompanyPageViewModel>();
            containerRegistry.RegisterForNavigation<CostCentrePage, CostCentrePageViewModel>();
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync(nameof(MenuPage) + "/" + nameof(NavigationPage) + "/" + nameof(Views.MainPage));
        }
    }
}
