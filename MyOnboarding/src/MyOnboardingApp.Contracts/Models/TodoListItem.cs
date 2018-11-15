using System;

namespace MyOnboardingApp.Contracts.Models
{
    public class TodoListItem
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastUpdateTime { get; set; }


        public override string ToString()
            => $"{nameof(Id)}: {Id}, " +
               $"{nameof(Text)}: {Text}, " +
               $"{nameof(CreationTime)}: {CreationTime:s}" +
               $"{nameof(LastUpdateTime)}: {LastUpdateTime:s}";
    }
}