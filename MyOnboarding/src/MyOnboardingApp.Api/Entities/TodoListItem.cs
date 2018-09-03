using System;
using System.EnterpriseServices.Internal;
using System.Linq;

namespace MyOnboardingApp.Api.Entities
{
    public class TodoListItem
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        // KA-310 new Guid generates just empty Guid, try Guid.NewGuid
        public TodoListItem(string text, Guid id = new Guid())
        {
            Text = text;
            Id = id;
        }
    }
}