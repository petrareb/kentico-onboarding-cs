using System;

namespace MyOnboardingApp.Contracts.Models
{
    public class TodoListItem
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public override string ToString() => $"{nameof(Id)}: {Id}, {nameof(Text)}: {Text}";
    }
}
