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
               $"{nameof(CreationTime)}: {CreationTime:0:MM/dd/yy H:mm:ss}" +
               $"{nameof(LastUpdateTime)}: {LastUpdateTime:0:MM/dd/yy H:mm:ss}";
    }
}