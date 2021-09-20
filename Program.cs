using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
//using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace JsonTransReader
{
    class Program
    {
        public async static Task Main()
        {
            Console.WriteLine("Loading json file...");

            string fileName = "script_workshop_premium_pl.json";

            using FileStream openStream = File.OpenRead(fileName);

            script_workshop_premium_pl[] script_workshop_premium_pl =
                await System.Text.Json.JsonSerializer.DeserializeAsync<script_workshop_premium_pl[]>(openStream);

            Console.WriteLine("Printing and saving to txt file values...");

            using StreamWriter file = new("TRANS.txt");

            foreach (var item in script_workshop_premium_pl)
            {
                Type classType = item.GetType();

                await file.WriteLineAsync($"{classType}, stepHeader, {item.stepHeader}\n" +
                                          $"{classType}, stepBody, {item.stepBody}\n" +
                                          $"{classType}, workshopNetwork, {item.workshopNetwork}\n" +
                                          $"{classType}, workshopNetworkShown, {item.workshopNetworkShown}"
                );

                // Console.WriteLine($"stepHeader, {item.stepHeader} \n" +
                //                   $"stepBody, {item.stepBody} \n" +
                //                   $"workshopNetwork, {item.workshopNetwork} \n" +
                //                   $"workshopNetworkShown, {item.workshopNetworkShown}"
                //);

                foreach (var script in item.scripts)
                {
                    classType = script.GetType();

                    await file.WriteLineAsync($"{classType}, scriptName, {script.scriptName}");

                    //Console.WriteLine($"scriptName, {script.scriptName}");

                    foreach (var nestedscript in script.scripts)
                    {
                        classType = nestedscript.GetType();

                        await file.WriteLineAsync($"{classType}, scriptOrder, {nestedscript.scriptOrder}\n" +
                                                  $"{classType}, scriptHeader, {nestedscript.scriptHeader}\n" +
                                                  $"{classType}, scriptBody, {nestedscript.scriptBody}"
                );

                        // Console.WriteLine($"scriptOrder, {nestedscript.scriptOrder} \n" +
                        //                   $"scriptHeader, {nestedscript.scriptHeader} \n" +
                        //                   $"scriptBody, {nestedscript.scriptBody}");

                    }
                }
            }

            await file.DisposeAsync();

            using (var reader = new StreamReader("TRANS.txt"))
            {
                List<string> classNames = new List<string>();
                List<string> classProperties = new List<string>();
                List<string> translatedText = new List<string>();

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    classNames.Add(values[0]);
                    classProperties.Add(values[1]);
                    translatedText.Add(values[2]);

                }

                var name = classNames[0];
                var property = classProperties[0];
                //var text = translatedText[0];

                Type t = Type.GetType(name);

                var instance =  Activator.CreateInstance(t);
                var nestedInstanceProperties = instance.GetType().GetProperties();              


                //var count = instance.GetType().GetProperties().Count();

                for (var j = 0; j < nestedInstanceProperties.Length; j++)
                {
                    var text = translatedText[j];
                    //if (nestedInstanceProperties[j] != null) 

                    if (nestedInstanceProperties[j].PropertyType == typeof(System.Int32)) nestedInstanceProperties[j].SetValue(instance, int.Parse(text));

                    //int number;
                    //if (Int32.TryParse(text, out number)) nestedInstanceProperties[j].SetValue(instance, number);
                    if (nestedInstanceProperties[j].PropertyType == typeof(System.String)) nestedInstanceProperties[j].SetValue(instance, text);
                    //if(nestedInstanceProperties[j].PropertyType == typeof(Array)) nestedInstanceProperties[j].SetValue(instance, text);

                    if(nestedInstanceProperties[j].PropertyType.IsArray) 
                    //Console.WriteLine("jest");
                    instance = Program.reccurentAssignmet( 4 ,instance, classNames, translatedText ); 
                    //JsonTransReader
                    //if (nestedInstanceProperties[j].PropertyType == typeof(JsonTransReader.script)) nestedInstanceProperties[j].SetValue(instance, int.Parse(text);

                }

                
            }


            //string _ResultObj = JsonConvert.SerializeObject(MyObjectsList);  //here get your json string


            #region
            //scripts.Select(new {x.scripts.ToList().ForEach(d => d.scriptName)})
            //script_workshop_premium_pl.ToList().ForEach(x => Console.Write(x.stepHeader + "\n" + x.stepBody + "\n"));
            //script_workshop_premium_pl.ToList()
            //.ForEach(async x => await file.WriteLineAsync(x.stepHeader + "\n" + x.stepBody + "\n" + x.scripts.ToList().ForEach(v => v.scriptName) ));
            //script_workshop_premium_pl.SelectMany(m => m.scripts).SelectMany(t => t.scripts);

            //script_workshop_premium_pl.SelectMany(m => m.scripts).Select(p => p.scriptName);

            //script_workshop_premium_pl.ToList().Select(x => Console.Write(x.stepHeader + "\n" + x.stepBody + "\n"));
            //script_workshop_premium_pl.SelectMany(m => m.scripts).SelectMany(t => t.scripts);
            #endregion
        }


        public static object reccurentAssignmet(int rowNumber, object instance, List<string> classNames, List<string> translatedText)
        {
                int number;

                var name = classNames[rowNumber];
                Type t = Type.GetType(name);
                var nestedInstance =  Activator.CreateInstance(t);
                
                var nestedArrayofInstances =  Array.CreateInstance(t, 1);

                nestedArrayofInstances.SetValue(nestedInstance, 0);

                //var nestedInstanceProperties = nestedInstance.GetValue(0).GetType().GetProperties(); 
                var nestedInstanceProperties = nestedInstance.GetType().GetProperties(); 

                for (var j = 0; j < (nestedInstanceProperties.Length - 1); j++)
                {
                    var text = translatedText[rowNumber + j]; //next text 
                    //if (nestedInstanceProperties[j].PropertyType == typeof(System.Int32)) nestedInstanceProperties[j].SetValue(nestedInstance, int.Parse(text));
                    
                    if (Int32.TryParse(text, out number)) nestedInstanceProperties[j].SetValue(nestedInstance, number);
                    if (nestedInstanceProperties[j].PropertyType == typeof(System.String)) nestedInstanceProperties[j].SetValue(nestedInstance, text);
                }

                var instanceProperties = instance.GetType().GetProperties();

                for (var j = 0; j < instanceProperties.Length; j++)
                {
                    if(instanceProperties[j].PropertyType.IsArray) instanceProperties[j].SetValue(instance, nestedArrayofInstances);   
                }

                rowNumber = rowNumber + nestedInstanceProperties.Length - 1;

                Program.reccurentAssignmet(rowNumber ,instance, classNames, translatedText);
                 

                return nestedArrayofInstances;
        }
        



    }



    class script_workshop_premium_pl
    {
        public script_workshop_premium_pl() { }
        public string stepHeader { get; set; }
        public string stepBody { get; set; }
        public string workshopNetwork { get; set; }
        public string workshopNetworkShown { get; set; }
        public script[] scripts { get; set; } = null;
    }

    class script
    {
        public script() { }
        public string scriptName { get; set; }
        public nestedscript[] scripts { get; set; }

    }

    class nestedscript
    {
        public nestedscript() { }
        public int scriptOrder { get; set; }
        public string scriptHeader { get; set; }
        public string scriptBody { get; set; }
    }
}
