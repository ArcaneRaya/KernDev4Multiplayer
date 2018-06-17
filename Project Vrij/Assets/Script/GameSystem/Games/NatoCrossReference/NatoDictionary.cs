using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class NatoDictionary
{
    private static Dictionary<char, string> letterWordPairs = new Dictionary<char, string>
    {
        {'a',"alpha"},
        {'b',"bravo"},
        {'c',"charlie"},
        {'d',"delta"},
        {'e',"echo"},
        {'f',"foxtrot"},
        {'g',"golf"},
        {'h',"hotel"},
        {'i',"india"},
        {'j',"juliet"},
        {'k',"kilo"},
        {'l',"lima"},
        {'m',"mike"},
        {'n',"november"},
        {'o',"oscar"},
        {'p',"papa"},
        {'q',"quebec"},
        {'r',"romeo"},
        {'s',"sierra"},
        {'t',"tango"},
        {'u',"uniform"},
        {'v',"victor"},
        {'w',"whiskey"},
        {'x',"x-ray"},
        {'y',"yankee"},
        {'z',"zulu"}
    };

    public static string ProvideWord(char letter)
    {
        if (!letterWordPairs.ContainsKey(letter))
        {
            throw new System.ArgumentException("NatoDictionary does not contain a value for: " + letter);
        }
        return letterWordPairs[letter];
    }

    public static string ProvideSentence(string word)
    {
        if (word == null)
        {
            throw new System.ArgumentNullException("word");
        }
        string returnString = "";
        for (int i = 0; i < word.Length; i++)
        {
            if (i > 0)
            {
                returnString += " ";
            }
            returnString += NatoDictionary.ProvideWord(word[i]);
        }
        return returnString;
    }
}
