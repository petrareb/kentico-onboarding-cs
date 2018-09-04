using System;
using System.Runtime.Serialization;

namespace MyOnboardingApp.Api.Models
{
    [DataContract]
    public class TodoListItem
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Text { get; set; }

        public TodoListItem(string text, Guid id = new Guid())
        {
            Text = text;
            Id = (id == Guid.Empty) ? Guid.NewGuid() : id;
        }
    }
}