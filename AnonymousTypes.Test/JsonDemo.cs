using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace AnonymousTypes.Test {
    public class JsonDemo {
        private readonly ITestOutputHelper _output;

        public JsonDemo(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void json_demo() {
            var userData = new {
                FirstName = "John",
                LastName = "Doe",

                Hobbies = new[] {
                    "Skateboarding",
                    "Poetry"
                },

                Languages = new[] {
                    new {
                        Name = "English",
                        Proficiency = "Professional working proficiency"
                    },
                    new {
                        Name = "Polish",
                        Proficiency = "Native or bilingual proficiency",
                    }
                }
            };

            var json = JsonConvert.SerializeObject(userData, Formatting.Indented);

            _output.WriteLine(json);
            /*
            This produces output:

            {
                "FirstName": "John",
                "LastName": "Doe",
                "Hobbies": [
                    "Skateboarding",
                    "Poetry"
                ],
                "Languages": [
                    {
                        "Name": "English",
                        "Proficiency": "Professional working proficiency"
                    },
                    {
                        "Name": "Polish",
                        "Proficiency": "Native or bilingual proficiency"
                    }
                ]
            }
            */
        }
    }
}