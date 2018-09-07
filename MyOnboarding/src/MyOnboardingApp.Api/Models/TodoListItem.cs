using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MyOnboardingApp.Api.Models
{
    public class TodoListItem
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}