using Pettel.Models.QuestionAnswer;
using Pettel.Repositories;
using Pettel.Repositories.PetPropertyRepositories;
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
    public partial class QuestionEditPage : ContentPage
    {
        QuestionAnswerRepository _questionAnswerRepository = new QuestionAnswerRepository();
        PetTypeRepository _petTypeRepository = new PetTypeRepository();
        Question currentQuestion = new Question();

        public QuestionEditPage(Question question)
        {
            InitializeComponent();
            currentQuestion = question;

            pckPetType.ItemsSource = _petTypeRepository.GetAll();

            txtTitle.Text = question.Title;
            txtContent.Text = question.Content;
            pckPetType.SelectedItem = question.PetType;

            if(question.Status == QuestionStatus.Çözüldü.ToString())
            {
                swStatus.IsToggled = true;
                lblStatus.Text = QuestionStatus.Çözüldü.ToString();
            }
            else
            {
                swStatus.IsToggled = false;
                lblStatus.Text = QuestionStatus.Çözülmedi.ToString();
            }
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            string title, content, petType, email, datetime, status;
            bool isActive;

            title = txtTitle.Text;
            content = txtContent.Text;
            petType = pckPetType.SelectedItem.ToString();
            email = currentQuestion.EMail;
            datetime = currentQuestion.Datetime;
            isActive = currentQuestion.IsActive;

            if (swStatus.IsToggled)
            {
                status = QuestionStatus.Çözüldü.ToString();
            }
            else
            {
                status = QuestionStatus.Çözülmedi.ToString();
            }

            Question question = new Question();
            question.Id = currentQuestion.Id;
            question.Title = title;
            question.Content = content;
            question.PetType = petType;
            question.EMail = email;
            question.Datetime = datetime;
            question.IsActive = isActive;
            question.Status = status;

            var isSaved = await _questionAnswerRepository.Update(question);

            if (isSaved)
            {
                await DisplayAlert("Başarılı.", "Düzenleme başarıyla kaydedildi.", "Tamam");
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Hata!", "Düzenleme yapılırken bir hata ile karşılaşıldı.", "Tamam");
            }

        }

        private void swStatus_Toggled(object sender, ToggledEventArgs e)
        {
            if (swStatus.IsToggled)
            {
                lblStatus.Text = QuestionStatus.Çözüldü.ToString();
            }
            else
            {
                lblStatus.Text = QuestionStatus.Çözülmedi.ToString();
            }
        }
    }
}