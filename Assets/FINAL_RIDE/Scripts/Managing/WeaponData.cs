using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace Assets.Scripts
{
    public static class WeaponData
    {
        public const int MAX_UPGRADE_NUMBER = 3;

        private static Dictionary<string, List<string>> weaponCombinations = new Dictionary<string, List<string>>();

        //public static WeaponData Load(string path)

        public static void CreateTotalShortNames()
        {

            string[] names = Enum.GetNames(typeof(AttackType));
            
            List<string> attackTypes = new List<string>(names.Length);

            for (int i = 0; i < names.Length; i++)
            {
                string shortName = new string(Regex.Replace(names[i], "(?<!^)[aeoui](?!$)", "").ToLower().ToCharArray(), 0, 2);

                attackTypes.Add(shortName);
            }
            
            List<string> result = new List<string>();

            GenerateCombinationsWithRepetition(attackTypes, MAX_UPGRADE_NUMBER, 0, new List<string>(MAX_UPGRADE_NUMBER), result);

            int s = result.Count;
            //List<List<string>> combinations = GetCombinations(attackTypes, MAX_GRADE_COUNT);


            //foreach (List<string> attackTypeCombination in combinations)
            //{
            //    StringBuilder stringBuilder = new StringBuilder();

            //    foreach (string attackTypeName in attackTypeCombination)
            //    {
            //        string shortName = new string(Regex.Replace(attackTypeName, "(?<!^)[aeoui](?!$)", "").ToLower().ToCharArray(), 0, MAX_GRADE_COUNT);

            //        stringBuilder.Append(shortName + "-");
            //    }

            //    string key = stringBuilder.ToString().TrimEnd('-');

            //    weaponCombinations.Add(key, attackTypeCombination);

            //    stringBuilder.Clear();
            //}            

            //using (FileStream file = new FileStream("Assets\\FINAL_RIDE\\Scripts\\Managing\\combinations.json", FileMode.OpenOrCreate))
            //{
            //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Dictionary<string, List<string>>));

            //    DataContractJsonSerializerSettings dataContract = new DataContractJsonSerializerSettings() { UseSimpleDictionaryFormat = true };

            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        serializer.WriteObject(file, weaponCombinations);
            //    }
            //}
        }

        //static List<List<T>> GetCombinations<T>(List<T> list, int length)
        //{
        //    var result = new List<List<T>>();
        //    Permute(list, length, new List<T>(), result);
        //    return result;
        //}

        //static void Permute<T>(List<T> list, int length, List<T> current, List<List<T>> result)
        //{
        //    if (current.Count == length)
        //    {
        //        result.Add(new List<T>(current));
        //        return;
        //    }

        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        current.Add(list[i]);
        //        Permute(list, length, current, result);
        //        current.RemoveAt(current.Count - 1); // Убираем последний элемент для следующей итерации
        //    }
        //}

        static void GenerateCombinationsWithRepetition(List<string> shortNames, int k, int startIndex, List<string> currentShortNames, List<string> result)
        {
            StringBuilder stringBuilder = new StringBuilder(MAX_UPGRADE_NUMBER);
            if (k == 0)
            {
                int currentIndex = currentShortNames.Count - MAX_UPGRADE_NUMBER;

                stringBuilder.Append(currentShortNames[currentIndex] + '-' + currentShortNames[currentIndex + 1] + '-' + currentShortNames[currentIndex + 2]);
                result.Add(stringBuilder.ToString());
                stringBuilder.Clear();
                return;
            }

            for (int i = startIndex; i < shortNames.Count; i++)
            {
                currentShortNames.Add(shortNames[i]);
                GenerateCombinationsWithRepetition(shortNames, k - 1, i, currentShortNames, result);
            }
        }

        public static string GetShortName(string someName)
        {
            return !string.IsNullOrEmpty(someName) ? new string(Regex.Replace(someName, "(?<!^)[aeoui](?!$)", "").ToLower().ToCharArray(), 0, 2) : string.Empty;
        }
    }
}