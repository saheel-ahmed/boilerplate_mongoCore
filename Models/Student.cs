using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDbDemo.Models
{
    public class Student : _Aduit
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Major { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Courses { get; set; }

        [BsonIgnore]
        public List<Course> CourseList { get; set; }
    }
}
