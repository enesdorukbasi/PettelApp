using Pettel.Models.QuestionAnswer;
using Pettel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pettel.Pages.QAPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QAListPage : ContentPage
    {
        QuestionAnswerRepository _questionAnswerRepository = new QuestionAnswerRepository();

        public QAListPage()
        {
            InitializeComponent();

            lstQuestion.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
        }

        protected override async void OnAppearing()
        {
            var questionList = await _questionAnswerRepository.GetAllQuestion();

            if(questionList.Count <= 0)
            {
                lblMessage.Text = "Listelenecek soru bulunamadı.";
                lblMessage.IsVisible = true;
                lstQuestion.ItemsSource = null;
                lstQuestion.IsRefreshing = false;
            }
            else
            {
                lblMessage.IsVisible = false;
                lstQuestion.ItemsSource = null;
                lstQuestion.ItemsSource = questionList;
                lstQuestion.IsRefreshing = false;
            }

        }

        private void ToolBarItemSearch_Clicked(object sender, EventArgs e)
        {
            if(QuestionSearch.IsVisible == true)
            {
                QuestionSearch.IsVisible = false;
            }
            else
            {
                QuestionSearch.IsVisible = true;
            }
        }

        private void ToolBarItemFilter_Clicked(object sender, EventArgs e)
        {

        }

        private async void btnCreateQuestion_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateQuestionPage());
        }

        private async void QuestionSearch_SearchButtonPressed(object sender, EventArgs e)
        {
            string search = QuestionSearch.Text;
            if (!String.IsNullOrEmpty(search))
            {
                var searchToList = await _questionAnswerRepository.GetAllQuestionByTitle(search);
                if(searchToList.Count <= 0)
                {
                    lblMessage.Text = "Aranan kriterlere uygun bir soru bulunamadı.";
                    lblMessage.IsVisible = true;
                    lstQuestion.ItemsSource = null;
                    lstQuestion.ItemsSource = searchToList;
                    lstQuestion.IsRefreshing = false;
                }
                else
                {
                    lblMessage.IsVisible = false;
                    lstQuestion.ItemsSource = null;
                    lstQuestion.ItemsSource = searchToList;
                    lstQuestion.IsRefreshing = false;
                }
            }
            else
            {
                OnAppearing();
            }
        }

        private async void QuestionSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = QuestionSearch.Text;
            if (!String.IsNullOrEmpty(search))
            {
                var searchToList = await _questionAnswerRepository.GetAllQuestionByTitle(search);
                if (searchToList.Count <= 0)
                {
                    lblMessage.Text = "Aranan kriterlere uygun bir soru bulunamadı.";
                    lblMessage.IsVisible = true;
                    lstQuestion.ItemsSource = null;
                    lstQuestion.ItemsSource = searchToList;
                    lstQuestion.IsRefreshing = false;
                }
                else
                {
                    lblMessage.IsVisible = false;
                    lstQuestion.ItemsSource = null;
                    lstQuestion.ItemsSource = searchToList;
                    lstQuestion.IsRefreshing = false;
                }
            }
            else
            {
                OnAppearing();
            }
        }

        private async void lstQuestion_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
            var question = e.Item as Question;
            await Navigation.PushModalAsync(new NavigationPage(new QuestionDetailsPage(question, false)));

            ((ListView)sender).SelectedItem = null;
        }
    }
}