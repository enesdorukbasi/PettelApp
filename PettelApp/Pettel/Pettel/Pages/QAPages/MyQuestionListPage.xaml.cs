using Pettel.Models.QuestionAnswer;
using Pettel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pettel.Pages.QAPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyQuestionListPage : ContentPage
    {
        QuestionAnswerRepository _questionAnswerRepository = new QuestionAnswerRepository();

        public MyQuestionListPage()
        {
            InitializeComponent();

            lstQuestion.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
        }

        protected async override void OnAppearing()
        {
            string email = Preferences.Get("currentUserEmail", string.Empty);
            var questionList = await _questionAnswerRepository.GetAllQuestionByEmail(email);

            if (questionList.Count <= 0)
            {
                lblMessage.Text = "Listelenecek sorunuz bulunmamaktadır.";
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

        private async void btnCreateQuestion_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateQuestionPage());
        }

        private void ToolBarItemSearch_Clicked(object sender, EventArgs e)
        {
            if (QuestionSearch.IsVisible == true)
            {
                QuestionSearch.IsVisible = false;
            }
            else
            {
                QuestionSearch.IsVisible = true;
            }
        }

        private async void QuestionSearch_SearchButtonPressed(object sender, EventArgs e)
        {
            string search = QuestionSearch.Text;
            string email = Preferences.Get("currentUserEmail", string.Empty);
            if (!String.IsNullOrEmpty(search))
            {
                var searchToList = await _questionAnswerRepository.GetAllQuestionByEmailAndTitle(email,search);
                if(searchToList.Count <= 0)
                {
                    lblMessage.Text = "Aranan kriterlere uygun bir soru bulunamadı.";
                    lblMessage.IsVisible = false;
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
            string email = Preferences.Get("currentUserEmail", string.Empty);
            if (!String.IsNullOrEmpty(search))
            {
                var searchToList = await _questionAnswerRepository.GetAllQuestionByEmailAndTitle(email, search);
                if (searchToList.Count <= 0)
                {
                    lblMessage.Text = "Aranan kriterlere uygun bir soru bulunamadı.";
                    lblMessage.IsVisible = false;
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

        private async void btnEdit_Clicked(object sender, EventArgs e)
        {
            string id = ((ImageButton)sender).CommandParameter.ToString();
            var question = await _questionAnswerRepository.GetById(id);

            if (question == null)
            {
                await DisplayAlert("Uyarı!", "Veri kayboldu.", "Tamam");
            }
            question.Id = id;

            await Navigation.PushModalAsync(new QuestionEditPage(question));
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            var isOk = await DisplayAlert("Sil", "Seçilen ilanı silmek istediğinize emin misiniz?", "Evet", "Hayır");

            if (isOk)
            {
                string id = ((ImageButton)sender).CommandParameter.ToString();
                var isComplete = await _questionAnswerRepository.DeleteQuestion(id);

                if (isComplete)
                {
                    await DisplayAlert("Başarılı.", "Silme işlemi başarılı.", "Tamam");
                }
                else
                {
                    await DisplayAlert("Başarısız.", "Silme işlemi başarısız.", "Tamam");
                }
                OnAppearing();
            }
        }

    }
}