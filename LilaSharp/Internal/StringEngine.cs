using System.Collections.Generic;
using System.Diagnostics;

namespace LilaSharp.Internal
{
    internal static class StringEngine
    {
        /// <summary>
        /// Gets the indicies.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        public static List<int> GetIndicies(string data, string pattern)
        {
            string left = data;
            int nextIndex = -1;
            int total = 0;
            List<int> indicies = new List<int>();
            while ((nextIndex = left.IndexOf(pattern)) != -1)
            {
                indicies.Add(total + nextIndex);
                if (nextIndex < left.Length - 1)
                {
                    left = left.Substring(nextIndex + 1);
                    total += nextIndex + 1;
                }
                else
                {
                    break;
                }
            }

            return indicies;
        }

        /// <summary>
        /// Gets the inside of a string between brackets
        /// </summary>
        /// <param name="data">The data to search.</param>
        /// <param name="start">The starting index.</param>
        /// <returns>The string inside balanced curly brackets.</returns>
        public static string GetInside(string data, int start)
        {
            int s = start;
            bool inQuotes = false;
            bool hitFirst = false;
            int level = 0;
            int endIndex = data.Length;
            char[] characters = data.ToCharArray();
            for (int i = start; i < characters.Length; i++)
            {
                if (!inQuotes)
                {
                    if (characters[i] == '{')
                    {
                        level++;
                        if (!hitFirst)
                        {
                            s = i;
                            hitFirst = true;
                        }
                    }
                    else if (characters[i] == '}')
                    {
                        level--;
                    }
                }

                if (level == 0 && hitFirst)
                {
                    endIndex = i;
                    break;
                }

                //Ignore brackets when in quotes
                if (characters[i] == '\"')
                {
                    inQuotes = !inQuotes;
                }
            }

            string inside = data.Substring(s, endIndex - s + 1);
            if (level == 0)
            {
                Debug.WriteLine("--------------");
                Debug.WriteLine(" ");
                Debug.WriteLine(inside);
                Debug.WriteLine(" ");
                Debug.WriteLine("--------------");
                return inside;
            }

            return null;
        }

        /// <summary>
        /// Flattens the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="lines">The lines.</param>
        public static string Flatten(string data, int lines)
        {
            char[] characters = data.ToCharArray();
            char[] flattened = new char[characters.Length];

            int index = 0;
            int left = lines;
            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i] == '\n')
                {
                    if (left > 1)
                    {
                        flattened[index++] = characters[i];
                    }

                    left--;
                }
                else
                {
                    flattened[index++] = characters[i];
                }
            }

            return new string(flattened, 0, index);
        }
    }
}
