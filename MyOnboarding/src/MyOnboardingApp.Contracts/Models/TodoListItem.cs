using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MyOnboardingApp.Contracts.Models
{
    public class TodoListItem
    { 
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("creation_time")]
        public DateTime CreationTime { get; set; }

        [BsonElement("last_update_time")]
        public DateTime LastUpdateTime { get; set; }


        public override string ToString()
        => $"{nameof(Id)}: {Id}, " +
        $"{nameof(Text)}: {Text}, " +
        $"{nameof(CreationTime)}: {CreationTime:0:MM/dd/yy H:mm:ss}" +
        $"{nameof(LastUpdateTime)}: {LastUpdateTime:0:MM/dd/yy H:mm:ss}";
    }
}
