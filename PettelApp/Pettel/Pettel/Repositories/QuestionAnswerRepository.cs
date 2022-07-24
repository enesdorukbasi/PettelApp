using Firebase.Database;
using Newtonsoft.Json;
using Pettel.Models.QuestionAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Pettel.Repositories
{
    public class QuestionAnswerRepository
    {
        FirebaseClient firebaseClient = new FirebaseClient("<Your db url>");

        public async Task<bool> SaveQuestion(Question question)
        {
            var data = await firebaseClient.Child(nameof(Question)).PostAsync(JsonConvert.SerializeObject(question));

            if (!string.IsNullOrEmpty(data.Key))
            {
                Preferences.Set("QuestionKey", data.Key);
                return true;
            }
            return false;
        }
        public async Task<bool> SaveAnswer(Answer answer)
        {
            var data = await firebaseClient.Child(nameof(Answer)).PostAsync(JsonConvert.SerializeObject(answer));

            if (!string.IsNullOrEmpty(data.Key))
            {
                Preferences.Set("AnswerKey", data.Key);
                return true;
            }
            return false;
        }

        public async Task<List<Question>> GetAllQuestion()
        {
            return (await firebaseClient.Child(nameof(Question)).OnceAsync<Question>()).Select(x => new Question
            {
                Id = x.Key,
                Title = x.Object.Title,
                Content = x.Object.Content,
                PetType = x.Object.PetType,
                EMail = x.Object.EMail,
                Datetime = x.Object.Datetime,
                IsActive = x.Object.IsActive,
                Status = x.Object.Status
            }).ToList();
        }
        public async Task<List<Question>> GetAllQuestionByTitle(string title)
        {
            return (await firebaseClient.Child(nameof(Question)).OnceAsync<Question>()).Select(x => new Question
            {
                Id = x.Key,
                Title = x.Object.Title,
                Content = x.Object.Content,
                PetType = x.Object.PetType,
                EMail = x.Object.EMail,
                Datetime = x.Object.Datetime,
                IsActive = x.Object.IsActive,
                Status = x.Object.Status
            }).Where(x=>x.Title.ToLower().Contains(title.ToLower())).ToList();
        }
        public async Task<List<Answer>> GetAllAnswerByQuestionId(string questionId)
        {
            return (await firebaseClient.Child(nameof(Answer)).OnceAsync<Answer>()).Select(x => new Answer
            {
                Id = x.Key,
                Content = x.Object.Content,
                EMail = x.Object.EMail,
                Datetime = x.Object.Datetime,
                CorrectAnswer = x.Object.CorrectAnswer,
                QuestionId = x.Object.QuestionId
            }).Where(x=>x.QuestionId.ToLower() == questionId.ToLower()).ToList();
        }
        public async Task<List<Question>> GetAllQuestionByEmail(string email)
        {
            return (await firebaseClient.Child(nameof(Question)).OnceAsync<Question>()).Select(x => new Question
            {
                Id = x.Key,
                Title = x.Object.Title,
                Content = x.Object.Content,
                PetType = x.Object.PetType,
                EMail = x.Object.EMail,
                Datetime = x.Object.Datetime,
                IsActive = x.Object.IsActive,
                Status = x.Object.Status
            }).Where(x=>x.EMail.ToLower() == email.ToLower()).ToList();
        }
        public async Task<List<Question>> GetAllQuestionByEmailAndTitle(string email,string title)
        {
            return (await firebaseClient.Child(nameof(Question)).OnceAsync<Question>()).Select(x => new Question
            {
                Id = x.Key,
                Title = x.Object.Title,
                Content = x.Object.Content,
                PetType = x.Object.PetType,
                EMail = x.Object.EMail,
                Datetime = x.Object.Datetime,
                IsActive = x.Object.IsActive,
                Status = x.Object.Status
            }).Where(x => x.EMail.ToLower() == email.ToLower()).Where(x=>x.Title.ToLower().Contains(title.ToLower())).ToList();
        }

        public async Task<Question> GetById(string id)
        {
            return (await firebaseClient.Child(nameof(Question) + "/" + id).OnceSingleAsync<Question>());
        }

        public async Task<bool> DeleteQuestion(string id)
        {
            await DeleteAnswer(id);
            await firebaseClient.Child(nameof(Question) + "/" + id).DeleteAsync();

            return true;
        }

        public async Task<bool> DeleteAnswer(string id)
        {
            List<Answer> answerList = (await firebaseClient.Child(nameof(Answer)).OnceAsync<Answer>()).Select(x => new Answer
            {
                Id = x.Key,
                Content = x.Object.Content,
                EMail = x.Object.EMail,
                Datetime = x.Object.Datetime,
                CorrectAnswer = x.Object.CorrectAnswer,
                QuestionId = x.Object.QuestionId
            }).Where(x => x.QuestionId == id).ToList();
            
            foreach (var i in answerList)
            {
                await firebaseClient.Child(nameof(Answer) + "/" + i.Id).DeleteAsync();
            }

            return true;
        }

        public async Task<bool> Update(Question question)
        {
            await firebaseClient.Child(nameof(Question) + "/" + question.Id).PutAsync(JsonConvert.SerializeObject(question));
            return true;
        }
    }
}
