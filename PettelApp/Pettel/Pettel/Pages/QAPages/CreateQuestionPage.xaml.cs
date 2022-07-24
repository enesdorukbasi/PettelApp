using Pettel.Models.QuestionAnswer;
using Pettel.Repositories;
using Pettel.Repositories.PetPropertyRepositories;
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
    public partial class CreateQuestionPage : ContentPage
    {
        PetTypeRepository _petTypeRepository = new PetTypeRepository();
        QuestionAnswerRepository _questionAnswerRepository = new QuestionAnswerRepository();

        public CreateQuestionPage()
        {
            InitializeComponent();

            pckPetType.ItemsSource = _petTypeRepository.GetAll();
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            string title, content, petType, email, datetime;
            bool isActive;

            title = txtTitle.Text;
            content = txtContent.Text;
            petType = pckPetType.SelectedItem.ToString();
            email = Preferences.Get("currentUserEmail", string.Empty);
            datetime = DateTime.Now.ToString();
            isActive = true;

            if (String.IsNullOrEmpty(email))
            {
                await DisplayAlert("Hata!", "Bir hatayla karşılaşıldı, uygulamayı kapatıp yeniden açmanız önerilir.", "Tamam");

                Application.Current.Quit();
            }

            Question question = new Question();
            question.Title = title;
            question.Content = content;
            question.PetType = petType;
            question.EMail = email;
            question.Datetime = datetime;
            question.Status = QuestionStatus.Çözülmedi.ToString();
            question.IsActive = isActive;

            var isSaved = await _questionAnswerRepository.SaveQuestion(question);

            if (isSaved)
            {
                await DisplayAlert("Paylaşma Tamamlandı", "İlan başarıyla paylaşıldı.", "Tamam");
                try
                {
                    await Navigation.PopAsync();
                }
                catch { }
            }
            else
            {
                await DisplayAlert("Hata!", "Bir hata ile karşılaşıldı.", "Tamam");
            }
        }

    }
}