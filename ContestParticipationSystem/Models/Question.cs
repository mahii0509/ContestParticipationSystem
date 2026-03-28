using System.Collections.Generic;

namespace ContestParticipationSystem.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int ContestId { get; set; }
        public Contest Contest { get; set; }

        public string Text { get; set; }      
        public string Type { get; set; }      

        public ICollection<Option> Options { get; set; }
    }
}