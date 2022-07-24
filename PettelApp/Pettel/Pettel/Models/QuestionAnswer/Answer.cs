using System;
using System.Collections.Generic;
using System.Text;

namespace Pettel.Models.QuestionAnswer
{
    public class Answer
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string EMail { get; set; }
        public string Datetime { get; set; }
        public bool CorrectAnswer { get; set; }
        public string QuestionId { get; set; }
    }
}
