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
    public partial class AddAnswerPage : ContentPage
    {
        QuestionAnswerRepository _questionAnswerRepository = new QuestionAnswerRepository();

        public AddAnswerPage(string QuestionId)
        {
            InitializeComponent();

            lblQuestionId.Text = QuestionId;
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            string content, email, datetime, questionId;
            bool correctAnswer;

            content = txtContent.Text;
            email = Preferences.Get("currentUserEmail", string.Empty);
            datetime = DateTime.Now.ToString();
            questionId = lblQuestionId.Text;
            correctAnswer = true;

            if (String.IsNullOrEmpty(content))
            {
                await DisplayAlert("Uyarı!", "İçerik alanı boş bırakılamaz.", "Tamam");
                return;
            }
            else if (String.IsNullOrEmpty(email))
            {
                await DisplayAlert("Hata!", "Bir hata oluştu. Uygulamayı kapatıp yeniden açınız.", "Tamam");
                return;
            }
            else if (String.IsNullOrEmpty(questionId))
            {
                await DisplayAlert("Hata!", "Bir hata oluştu. Uygulamayı kapatıp yeniden açınız.", "Tamam");
                return;
            }

            Answer answer = new Answer();
            answer.Content = content;
            answer.EMail = email;
            answer.Datetime = datetime;
            answer.QuestionId = questionId;
            answer.CorrectAnswer = correctAnswer;

            var isSaved = await _questionAnswerRepository.SaveAnswer(answer);

            if (isSaved)
            {
                await DisplayAlert("Başarılı.", "Cevap başarıyla kaydedildi.", "Tamam");
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Başarısız.", "Cevap kaydı başarısız.", "Tamam");
            }
        }
    }
}