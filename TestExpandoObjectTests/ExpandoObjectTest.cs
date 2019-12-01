using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Text;
using TestExpandoObject;
using Xunit;

// https://www.oreilly.com/learning/building-c-objects-dynamically

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

    }
}
