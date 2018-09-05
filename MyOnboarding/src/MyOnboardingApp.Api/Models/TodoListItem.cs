using System;
using System.Runtime.Serialization;

namespace MyOnboardingApp.Api.Models
{
    public class TodoListItem
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public TodoListItem(string text, Guid id = new Guid())
        {
            Text = text;
            Id = (id == Guid.Empty) ? Guid.NewGuid() : id;
        }
    }
}