using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Text;
using TestExpandoObject;
using Xunit;
using Xunit.Abstractions;

// https://www.oreilly.com/learning/building-c-objects-dynamically
// Working with the Dynamic Type in C#
// https://www.red-gate.com/simple-talk/dotnet/c-programming/working-with-the-dynamic-type-in-c/

// ExpandoObject Class
// https://docs.microsoft.com/en-us/dotnet/api/system.dynamic.expandoobject?view=netframework-4.8

/*
ExpandoObject allows you to write code that is more readable than typical reflection code with GetProperty(“Field”) syntax. 
It can be useful when dealing with XML or JSON for quickly setting up a type to program against instead of always having to create data transfer objects. 
The ability for ExpandoObject to support data binding through INotifyPropertyChanged is a huge win for anyone using WPF, MVC, 
or any other binding framework in .NET as it allows you to use these “objects on the fly” as well as other statically typed classes. 
 */

namespace TestExpandoObjectTests
{
    public class ExpandoObjectTest
    {
        private readonly ITestOutputHelper output;

        public ExpandoObjectTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void AddPropertiesToExpandoObjectTest()
        {
            dynamic expando = new ExpandoObject();
            expando.Name = "Brian";
            expando.Country = "USA";
            expando.Skill = new ExpandoObject();
            expando.Skill.Description = "Computer Science";
            expando.Skill.Level = "Great";

            Assert.Equal("Brian", expando.Name);
            Assert.Equal("USA", expando.Country);
            Assert.Equal("Computer Science", expando.Skill.Description);
            Assert.Equal("Great", expando.Skill.Level);
        }

        [Fact]
        public void AddPropertiesDynamicallyToExpandoObjectTest()
        {
            dynamic expando = new ExpandoObject();
            expando.Name = "Brian";
            expando.Country = "USA";

            dynamic skill = new ExpandoObject();
            skill.Description = "Computer Science";
            skill.Level = "Great";

            // Add properties dynamically to expando
            ExpandoObjectHelper.AddProperty(expando, "Language", "English");
            ExpandoObjectHelper.AddProperty(expando, "Skill", skill);

            Assert.Equal("Brian", expando.Name);
            Assert.Equal("USA", expando.Country);
            Assert.Equal("English", expando.Language);
            Assert.Equal("Computer Science", expando.Skill.Description);
            Assert.Equal("Great", expando.Skill.Level);
        }

        [Fact]
        public void AddMethodToExpandoObjectTest()
        {
            dynamic expando = new ExpandoObject();
            expando.Name = "Brian";
            expando.Country = "USA";

            // Add method to expando
            expando.IsValid = (Func<bool>)(() =>
            {
                // Check that they supplied a name
                if (string.IsNullOrWhiteSpace(expando.Name))
                    return false;
                return true;
            });

            if (!expando.IsValid())
            {
                // Don't allow continuation...
            }

            Assert.Equal("Brian", expando.Name);
            Assert.Equal("USA", expando.Country);
            Assert.True(expando.IsValid());
        }


        [Fact]
        public void AddEventHandlersToExpandoObjectTest()
        {
            dynamic expando = new ExpandoObject();
            expando.Name = "Brian";
            expando.Country = "USA";

            // Add properties dynamically to expando
            ExpandoObjectHelper.AddProperty(expando, "Language", "English");

            // You can also add event handlers to expando objects
            var eventHandler =
                new Action<object, EventArgs>((sender, eventArgs) =>
                {
                    dynamic exp = sender as ExpandoObject;
                    var langArgs = eventArgs as LanguageChangedEventArgs;
                    Console.WriteLine($"Setting Language to : {langArgs?.Language}");
                    exp.Language = langArgs?.Language;
                });

            // Add a LanguageChanged event and predefined event handler
            ExpandoObjectHelper.AddEvent(expando, "LanguageChanged", eventHandler);

            // Add a CountryChanged event and an inline event handler
            ExpandoObjectHelper.AddEvent(expando, "CountryChanged",
                new Action<object, EventArgs>((sender, eventArgs) =>
                {
                    dynamic exp = sender as ExpandoObject;
                    var ctryArgs = eventArgs as CountryChangedEventArgs;
                    string newLanguage = string.Empty;
                    switch (ctryArgs?.Country)
                    {
                        case "France":
                            newLanguage = "French";
                            break;
                        case "China":
                            newLanguage = "Mandarin";
                            break;
                        case "Spain":
                            newLanguage = "Spanish";
                            break;
                    }
                    Console.WriteLine($"Country changed to {ctryArgs?.Country}, " + $"changing Language to {newLanguage}");
                    exp?.LanguageChanged(sender, new LanguageChangedEventArgs() { Language = newLanguage });
                }));

            #region ExpandoObject supports INotifyPropertyChanged which is the foundation of data binding to properties in .NET

            ((INotifyPropertyChanged)expando).PropertyChanged += new PropertyChangedEventHandler((sender, eventArg) =>
                {
                    dynamic exp = sender as dynamic;
                    var pcea = eventArg as PropertyChangedEventArgs;
                    if (pcea?.PropertyName == "Country")
                        exp.CountryChanged(exp, new CountryChangedEventArgs() { Country = exp.Country });
                });

            #endregion

            Assert.Equal("Brian", expando.Name);
            Assert.Equal("USA", expando.Country);
            Assert.Equal("English", expando.Language);

            // Changing Country to France...
            expando.Country = "France";

            Assert.Equal("Brian", expando.Name);
            Assert.Equal("France", expando.Country);
            Assert.Equal("French", expando.Language);

            // Changing Country to China...
            expando.Country = "China";

            Assert.Equal("Brian", expando.Name);
            Assert.Equal("China", expando.Country);
            Assert.Equal("Mandarin", expando.Language);

            // Changing Country to Spain...
            expando.Country = "Spain";

            Assert.Equal("Brian", expando.Name);
            Assert.Equal("Spain", expando.Country);
            Assert.Equal("Spanish", expando.Language);
        }


        [Fact]
        public void ClassHierarchyTest()
        {
            output.WriteLine("ExpandoObject inherits from System.Object: " +
                typeof(ExpandoObject).IsSubclassOf(typeof(Object)));

            output.WriteLine("DynamicObject inherits from System.Object: " +
                typeof(DynamicObject).IsSubclassOf(typeof(Object)));

            Assert.True(typeof(ExpandoObject).IsSubclassOf(typeof(Object)));
            Assert.True(typeof(DynamicObject).IsSubclassOf(typeof(Object)));

        }

        [Fact]
        public void DeserializeJsonToExpandoObjectTest()
        {
            string jsonString = "{\"a\":1}";
            dynamic exObj = JsonConvert.DeserializeObject<ExpandoObject>(jsonString) as dynamic;

            output.WriteLine($"exObj.a = {exObj?.a}, type of {exObj?.a.GetType()}");

            Assert.NotNull(exObj);
            Assert.Equal(1, exObj.a);
        }

        [Fact]
        public void SerializeExpandoObjectToJsonTest()
        {
            dynamic expando = new ExpandoObject();
            expando.Name = "Brian";
            expando.Country = "USA";
            expando.Skill = new ExpandoObject();
            expando.Skill.Description = "Computer Science";
            expando.Skill.Level = "Great";

            string jsonString = JsonConvert.SerializeObject(expando);

            output.WriteLine($"{jsonString}");

            Assert.NotNull(jsonString);
        }

        [Fact]
        public void IDictionaryTest()
        {
            string jsonString = "{\"a\":1}";
            dynamic exObj = JsonConvert.DeserializeObject<ExpandoObject>(jsonString) as dynamic;

            IDictionary<string, object> dict = exObj as IDictionary<string, object> ?? new Dictionary<string, object>();
            foreach (var exObjProp in dict)
            {
                output.WriteLine($"IDictionary = {exObjProp.Key}: {exObjProp.Value}");
            }

            // Be carefull, we get a long integer. So 1L
            Assert.Equal(1L, dict["a"]);
        }

        [Fact]
        public void ToExpandoObjectTest()
        {
            var anonymous = new { Id = 123, Text = "Abc123", Test = true };

            dynamic dynamicObject = anonymous.ToExpandoObject();
            Assert.Equal(123, dynamicObject.Id);
            Assert.Equal("Abc123", dynamicObject.Text);
            Assert.Equal(true, dynamicObject.Test);


            ExpandoObject expandoObject = anonymous.ToExpandoObject();

            IDictionary<string, object> dict = expandoObject as IDictionary<string, object>;
            Assert.Equal(123, dict["Id"]);
            Assert.Equal("Abc123", dict["Text"]);
            Assert.Equal(true, dict["Test"]);
        }


        [Fact]
        public void AddingNewMembers()
        {
            dynamic sampleObject = new ExpandoObject();
            sampleObject.test = "Dynamic Property";
            output.WriteLine(sampleObject.test);
            output.WriteLine(sampleObject.test.GetType());
        }

        [Fact]
        public void AddingMethod()
        {
            dynamic sampleObject = new ExpandoObject();
            sampleObject.number = 10;
            sampleObject.Increment = (Action)(() => { sampleObject.number++; });

            // Before calling the Increment method.
            output.WriteLine(sampleObject.number);

            sampleObject.Increment();

            // After calling the Increment method.
            output.WriteLine(sampleObject.number);
            // This code example produces the following output:
            // 10
            // 11
        }

        [Fact]
        public void AddEvent()
        {
            dynamic sampleObject = new ExpandoObject();

            // Create a new event and initialize it with null.  
            sampleObject.sampleEvent = null;

            // Add an event handler.  
            sampleObject.sampleEvent += new EventHandler(SampleHandler);

            // Raise an event for testing purposes.  
            sampleObject.sampleEvent(sampleObject, new EventArgs());
        }

        // Event handler.  
        void SampleHandler(object sender, EventArgs e)
        {
            output.WriteLine("SampleHandler for {0} event", sender);
        }

        [Fact]
        public void PassingAsParameter()
        {
            dynamic employee, manager;

            employee = new ExpandoObject();
            employee.Name = "John Smith";
            employee.Age = 33;

            manager = new ExpandoObject();
            manager.Name = "Allison Brown";
            manager.Age = 42;
            manager.TeamSize = 10;

            WritePerson(manager);
            WritePerson(employee);
        }

        private void WritePerson(dynamic person)
        {
            output.WriteLine("{0} is {1} years old.",
                              person.Name, person.Age);
            // The following statement causes an exception
            // if you pass the employee object.
            // Console.WriteLine("Manages {0} people", person.TeamSize);

        }

        [Fact]
        public void EnumeratingAndDeletingMembers()
        {
            dynamic employee = new ExpandoObject();
            employee.Name = "John Smith";
            employee.Age = 33;

            foreach (var property in (IDictionary<String, Object>)employee)
            {
                output.WriteLine(property.Key + ": " + property.Value);
            }
            // This code example produces the following output:
            // Name: John Smith
            // Age: 33

            dynamic employee2 = new ExpandoObject();
            employee2.Name = "John Smith";
            employee2.Age = 33;
            ((IDictionary<String, Object>)employee2).Remove("Name");

        }


        [Fact]
        public void ReceivingNotificationsOfPropertyChanges()
        {
            dynamic employee = new ExpandoObject();
            ((INotifyPropertyChanged)employee).PropertyChanged +=
                new PropertyChangedEventHandler(HandlePropertyChanges);
            employee.Name = "John Smith";
        }

        private void HandlePropertyChanges(
            object sender, PropertyChangedEventArgs e)
        {
            output.WriteLine("{0} has changed.", e.PropertyName);
        }

    }
}
