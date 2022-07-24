using System;
using System.Collections.Generic;
using System.Text;

namespace Pettel.Models.QuestionAnswer
{
    public enum QuestionStatus
    {
        Çözüldü = 0,
        Çözülmedi = 1
    }

    public class Question
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string PetType { get; set; }
        public string EMail { get; set; }
        public string Datetime { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
    }
}
