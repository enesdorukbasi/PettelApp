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
    public partial class QuestionDetailsPage : ContentPage
    {
        Question CurrentQuestion = new Question();
        QuestionAnswerRepository _questionAnswerRepository = new QuestionAnswerRepository();

        public QuestionDetailsPage(Question question, bool isMyQuestion)
        {
            InitializeComponent();

            CurrentQuestion = question;

            lblTitle.Text = question.Title;
            lblContent.Text = question.Content;
            lblPetType.Text = question.PetType;
            lblDatetime.Text = question.Datetime;

            lstAnswer.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
        }

        private async void btnAddAnswer_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddAnswerPage(CurrentQuestion.Id));
        }

        protected async override void OnAppearing()
        {
            var answerList = await _questionAnswerRepository.GetAllAnswerByQuestionId(CurrentQuestion.Id);

            if(answerList.Count <= 0)
            {
                lblMessage.Text = "Bu soru henüz cevaplanmamış.";
                lblMessage.IsVisible = true;
                lstAnswer.ItemsSource = null;
                lstAnswer.IsRefreshing = false;
            }
            else
            {
                lblMessage.IsVisible = false;
                lstAnswer.ItemsSource = null;
                lstAnswer.ItemsSource = answerList;
                lstAnswer.IsRefreshing = false;
            }
        }
    }
}